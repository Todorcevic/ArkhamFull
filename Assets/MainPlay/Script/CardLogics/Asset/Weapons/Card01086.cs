using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01086 : Weapon
    {
        CardEffect throwAttack;
        protected override int ResourceStarting => 0;
        protected override int ResourceCost => 0;
        protected override int ActionsCost => 1;
        protected override int Damage => ThisCard.VisualOwner.AttackDamage;
        protected override int Bonus => 1;

        /*****************************************************************************************/
        protected override void BuildingCardEffect(InvestigatorTurn investigatorTurn)
        {
            base.BuildingCardEffect(investigatorTurn);
            throwAttack = new CardEffect(
                card: ThisCard,
                effect: ThrowThisWeapon,
                animationEffect: ThrowFightAnimation,
                payEffect: PayThrowEffect,
                cancelEffect: CancelThrowEffect,
                type: EffectType.Activate | EffectType.Fight,
                name: "Lanzar " + ThisCard.Info.Name,
                actionCost: 1,
                investigatorImageCardInfoOwner: ThisCard.VisualOwner);
            investigatorTurn.CardEffects.Add(throwAttack);
        }

        IEnumerator ThrowFightAnimation()
        {
            ThisCard.CardTools.PlayOneShotSound(ThisCard.Effect2);
            yield return new WaitWhile(() => ThisCard.CardTools.AudioSource.isPlaying);
        }

        IEnumerator ThrowThisWeapon() => new FightWithWeaponAction(ref throwAttack, ThrowFight).RunNow();

        IEnumerator ThrowFight(CardComponent enemy)
        {
            SettingInvestigatorAttack attackToenemy = new SettingInvestigatorAttack(enemy, GameControl.ActiveInvestigator.AttackDamage + 1, throwAttack.IsCancelable, bonus: 2);
            yield return attackToenemy.RunNow();
            throwAttack.IsCancel = !attackToenemy.SkillTest.IsComplete ?? false;
        }

        IEnumerator PayThrowEffect() => new DiscardAction(ThisCard, withTopUp: true).RunNow();

        IEnumerator CancelThrowEffect() => new MoveCardAction(ThisCard, ThisCard.Owner.Assets, withPreview: false).RunNow();
    }
}