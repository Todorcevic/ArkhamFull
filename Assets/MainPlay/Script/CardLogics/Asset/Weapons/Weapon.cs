using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public abstract class Weapon : CardAsset
    {
        protected CardComponent enemyToFight;
        protected CardEffect weaponCardEffect;
        protected SettingInvestigatorAttack attackToenemy;
        bool NeedResources => ResourceStarting > 0;
        protected abstract int ResourceStarting { get; }
        protected abstract int ResourceCost { get; }
        protected abstract int ActionsCost { get; }
        protected abstract int Damage { get; }
        protected abstract int Bonus { get; }
        protected virtual List<CardComponent> FightWithThisEnemies => ThisCard.VisualOwner.AllEnemiesInMyLocation;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckFightWithThisWeapon(investigatorTurn))
                BuildingCardEffect(investigatorTurn);
        }

        bool CheckFightWithThisWeapon(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.IsInPlay) return false;
            if (investigatorTurn.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (FightWithThisEnemies.Count < 1) return false;
            if (NeedResources && ThisCard.ResourcesToken.Amount < 1) return false;
            return true;
        }

        protected virtual void BuildingCardEffect(InvestigatorTurn investigatorTurn)
        {
            weaponCardEffect = new CardEffect(
                card: ThisCard,
                effect: FightWithThisWeapon,
                payEffect: PayCostEffect,
                animationEffect: FightEffectAnimation2,
                cancelEffect: CancelCardEffect,
                type: EffectType.Activate | EffectType.Fight,
                name: "Atacar con " + ThisCard.Info.Name,
                actionCost: ActionsCost,
                investigatorImageCardInfoOwner: ThisCard.VisualOwner
                );
            investigatorTurn.CardEffects.Add(weaponCardEffect);
        }

        IEnumerator FightEffectAnimation2() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator FightWithThisWeapon() => new FightWithWeaponAction(ref weaponCardEffect, FightEffect).RunNow();

        IEnumerator FightEffect(CardComponent enemy)
        {
            enemyToFight = enemy;
            SettingFight(enemy);
            yield return attackToenemy.RunNow();
            weaponCardEffect.IsCancel = !attackToenemy.SkillTest.IsComplete ?? false;
        }

        protected virtual void SettingFight(CardComponent enemy)
        {
            attackToenemy = new SettingInvestigatorAttack(enemy, Damage, weaponCardEffect.IsCancelable, Bonus);
            attackToenemy.SkillTest.ExtraCard = ThisCard;
        }

        protected virtual IEnumerator PayCostEffect() => new AddTokenAction(ThisCard.ResourcesToken, -ResourceCost).RunNow();

        protected virtual IEnumerator CancelCardEffect() => new AddTokenAction(ThisCard.ResourcesToken, ResourceCost).RunNow();

        protected override IEnumerator PlayCardFromHand()
        {
            yield return base.PlayCardFromHand();
            yield return new AddTokenAction(ThisCard.ResourcesToken, ResourceStarting).RunNow();
        }
    }
}