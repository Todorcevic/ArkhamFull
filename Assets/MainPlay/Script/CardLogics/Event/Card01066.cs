using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01066 : CardEvent
    {
        bool effectToExecuteIOnlyOne;
        SkillTest thisSkillTest;
        protected virtual int DamageToEnemy => 1;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InvestigatorTurn investigatorturn && PlayingCard(investigatorturn))
            {
                playFromHand = PlayFromHand;
                playFromHand.Type = EffectType.Play | EffectType.Evade;
                investigatorturn.CardEffects.Add(playFromHand);
            }
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is RevealChaosTokenAction throwChaosToken && CheckChaosToken(throwChaosToken))
                new EffectAction(ChaosTokenReveled, ChaosTokenReveledAnimation).AddActionTo();
        }

        protected override bool PlayingCard(InvestigatorTurn investigatorturn)
        {
            if (!CheckPlayedFromHand()) return false;
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (!ThisCard.VisualOwner.IsEnganged) return false;
            return true;
        }

        bool CheckChaosToken(RevealChaosTokenAction throwChaosToken)
        {
            if (!effectToExecuteIOnlyOne) return false;
            if (throwChaosToken.SkillTest != thisSkillTest) return false;
            if (!((ChaosTokenType.Cultist | ChaosTokenType.Kthulu | ChaosTokenType.Skull | ChaosTokenType.Tablet | ChaosTokenType.Fail).HasFlag(throwChaosToken.SkillTest.TokenThrow.Type))) return false;
            return true;
        }

        IEnumerator ChaosTokenReveledAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect2).RunNow();

        protected virtual IEnumerator ChaosTokenReveled()
        {
            yield return new UpdateActionsLeft(ThisCard.VisualOwner, -1).RunNow();
            effectToExecuteIOnlyOne = false;
        }

        protected override IEnumerator LogicEffect()
        {
            List<CardEffect> listCardEffects = new List<CardEffect>();
            foreach (CardComponent enemy in ThisCard.VisualOwner.Threat.ListCards.FindAll(c => c.CardType == CardType.Enemy))
                listCardEffects.Add(new CardEffect(
                    card: enemy,
                    effect: () => Evade(enemy),
                    animationEffect: ((CardEnemy)enemy.CardLogic).BadEffectForEnemyAnimation,
                    type: EffectType.Choose,
                    name: ThisCard.Info.Name + " a " + enemy.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            yield return new ChooseCardAction(listCardEffects, cancelableCardEffect: playFromHand).RunNow();

            IEnumerator Evade(CardComponent enemy)
            {
                effectToExecuteIOnlyOne = true;
                EvadeEnemySkillTest evadeEnemy = new EvadeEnemySkillTest(enemy, playFromHand.IsCancelable);
                thisSkillTest = evadeEnemy.SkillTest;
                evadeEnemy.SkillTest.SkillType = Skill.Willpower;
                evadeEnemy.SkillTest.ExtraCard = ThisCard;
                evadeEnemy.SkillTest.WinEffect.Add(new CardEffect(
                    card: ThisCard,
                    effect: DoDamage,
                    animationEffect: DoDamageAnimation,
                    type: EffectType.Choose,
                    name: "Hacer daño",
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
                yield return evadeEnemy.RunNow();
                playFromHand.IsCancel = !evadeEnemy.SkillTest.IsComplete ?? false;

                IEnumerator DoDamageAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect3).RunNow();

                IEnumerator DoDamage() => new DamageEnemyAction(enemy, DamageToEnemy).RunNow();
            }
        }
    }
}