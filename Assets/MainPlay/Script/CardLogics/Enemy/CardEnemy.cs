using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class CardEnemy : CardLogic
    {
        CardEffect attack;
        CardEffect evade;
        public bool IsEnganged => ThisCard.CurrentZone.ZoneType == Zones.Threat;
        public bool CanBeEvaded => IsEnganged || !ThisCard.IsExausted;
        public virtual bool IsHunter => false;
        public virtual bool IsRetaliate => false;
        public virtual int Health
        {
            get
            {
                int multiplier = (bool)ThisCard.Info.Health_per_investigator ? GameControl.InvestigatorsStartingAmount : 1;
                return (int)ThisCard.Info.Health * multiplier;
            }
        }
        public virtual List<CardComponent> SpawnLocation => null;

        public InvestigatorComponent InvestigatorEnganged
        {
            get
            {
                if (ThisCard.CurrentZone.ZoneType == Zones.Threat)
                    foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame)
                        if (investigator.Threat.ListCards.Contains(ThisCard))
                            return investigator;
                return null;
            }
        }
        public CardComponent CurrentLocation
        {
            get
            {
                if (ThisCard.CurrentZone.ZoneType == Zones.IntoCard && ThisCard.CurrentZone.ThisCard.CardType == CardType.Location)
                    return ThisCard.CurrentZone.ThisCard;
                if (IsEnganged) return InvestigatorEnganged.CurrentLocation;
                return null;
            }
        }

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InvestigatorTurn investigatorTurn)
            {
                if (CheckCanBeAttacked(investigatorTurn))
                    investigatorTurn.CardEffects.Add(attack = new CardEffect(
                        card: ThisCard,
                        effect: () => BasicAttack(investigatorTurn.ActiveInvestigator),
                        animationEffect: BadEffectForEnemyAnimation,
                        type: EffectType.Fight,
                        name: "Atacar a " + ThisCard.Info.Name + CheckDangerAttackName(),
                        actionCost: 1));
                if (CheckCanBeEngangedByInvestigator(investigatorTurn))
                    investigatorTurn.CardEffects.Add(new CardEffect(
                        card: ThisCard,
                        effect: () => Engange(investigatorTurn.ActiveInvestigator),
                        type: EffectType.Engange,
                        name: "Enfrentarse a " + ThisCard.Info.Name,
                        actionCost: 1));
                if (CheckCanBeEvade(investigatorTurn))
                    investigatorTurn.CardEffects.Add(evade = new CardEffect(
                        card: ThisCard,
                        effect: Evade,
                        animationEffect: BadEffectForEnemyAnimation,
                        type: EffectType.Evade,
                        name: "Evadir a " + ThisCard.Info.Name,
                        actionCost: 1));
            }
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is AddTokenAction addToken && CheckDie(addToken))
                new DefeatCardAction(ThisCard).AddActionTo();
            if (gameAction is MoveCardAction moveCard && OnMoveEnganged(moveCard))
                new EnemyTargetEngangeAction(ThisCard).AddActionTo();
            if (gameAction is ExaustCardAction exaustCard && CheckCanEnemyEngangeWhenReady(exaustCard))
                new EnemyTargetEngangeAction(ThisCard).AddActionTo();
            if (gameAction is SettingInvestigatorAttack attackToEnemy && RetiliateAttack(attackToEnemy))
                new SettingEnemyAttackAction(ThisCard, GameControl.ActiveInvestigator).AddActionTo();
        }

        bool CheckCanBeAttacked(InvestigatorTurn investigatorTurn)
        {
            if (CurrentLocation != investigatorTurn.ActiveInvestigator.CurrentLocation) return false;
            return true;
        }

        bool CheckCanBeEngangedByInvestigator(InvestigatorTurn investigatorTurn)
        {
            if (CurrentLocation != investigatorTurn.ActiveInvestigator.CurrentLocation) return false;
            if (InvestigatorEnganged == investigatorTurn.ActiveInvestigator) return false;
            return true;
        }

        bool CheckCanBeEvade(InvestigatorTurn investigatorTurn)
        {
            if (InvestigatorEnganged != investigatorTurn.ActiveInvestigator) return false;
            return true;
        }

        bool CheckDie(AddTokenAction addTokenAction)
        {
            if (addTokenAction.ToToken != ThisCard.HealthToken) return false;
            if (ThisCard.HealthToken.Amount < Health) return false;
            return true;
        }
        bool OnMoveEnganged(MoveCardAction moveCardAction)
        {
            if (moveCardAction.ThisCard != ThisCard && moveCardAction.ThisCard.CardType != CardType.PlayCard) return false;
            if (ThisCard.IsExausted) return false;
            if (IsEnganged) return false;
            if (!GameControl.AllInvestigatorsInGame.Exists(c => c.PlayCard.CurrentZone == ThisCard.CurrentZone)) return false;
            return true;
        }
        bool CheckCanEnemyEngangeWhenReady(ExaustCardAction exaustCardAction)
        {
            if (exaustCardAction.Card != ThisCard) return false;
            if (ThisCard.IsExausted) return false;
            if (IsEnganged) return false;
            if (!GameControl.AllInvestigatorsInGame.Exists(c => c.PlayCard.CurrentZone == ThisCard.CurrentZone)) return false;
            return true;
        }

        bool RetiliateAttack(SettingInvestigatorAttack attackToEnemy)
        {
            if (!IsRetaliate) return false;
            if (attackToEnemy.Enemy != ThisCard) return false;
            if (ThisCard.IsExausted) return false;
            if (!attackToEnemy.SkillTest.IsComplete ?? false) return false;
            if (attackToEnemy.SkillTest.IsWin) return false;
            return true;
        }

        IEnumerator BasicAttack(InvestigatorComponent investigator)
        {
            SettingInvestigatorAttack attackToEnemy = new SettingInvestigatorAttack(ThisCard, investigator.AttackDamage, attack.IsCancelable);
            yield return attackToEnemy.RunNow();
            attack.IsCancel = !attackToEnemy.SkillTest.IsComplete ?? false;
        }

        IEnumerator Engange(InvestigatorComponent investigator) =>
            new EngangeAction(ThisCard, investigator).RunNow();

        public IEnumerator BadEffectForEnemyAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect4).RunNow();

        IEnumerator Evade()
        {
            EvadeEnemySkillTest evadeEnemy = new EvadeEnemySkillTest(ThisCard, evade.IsCancelable);
            yield return evadeEnemy.RunNow();
            evade.IsCancel = !evadeEnemy.SkillTest.IsComplete ?? false;
        }

        public virtual List<InvestigatorComponent> Prey(List<InvestigatorComponent> SearchInThisList)
            => SearchInThisList;

        public string CheckDangerAttackName()
        {
            if (IsEnganged && InvestigatorEnganged != GameControl.ActiveInvestigator)
                return "<color=#FF2B2B> con riesgo de dañar a " + InvestigatorEnganged.Name + "</color>";
            else return string.Empty;
        }
    }
}