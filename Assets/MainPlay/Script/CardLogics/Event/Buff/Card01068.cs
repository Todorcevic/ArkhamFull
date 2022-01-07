using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01068 : CardEvent, IBuffable
    {
        CardComponent cardAffected;
        CardLogic cardLogicBackup;
        protected override bool IsFast => true;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is PhaseAction phaseAction && PlayingCard())
                phaseAction.ChooseCardOptionalAction.ChosenCardEffects.Add(playFromHand = PlayFromHand);
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is PhaseAction) BuffActive = false;
        }

        bool PlayingCard()
        {
            if (!CheckPlayedFromHand()) return false;
            if (ThisCard.VisualOwner.AllEnemiesInMyLocation.FindAll(c => !c.KeyWords.Contains("Elite")).Count < 1) return false;
            return true;
        }

        protected override IEnumerator LogicEffect()
        {
            List<CardEffect> listCardEffects = new List<CardEffect>();
            foreach (CardComponent enemy in ThisCard.VisualOwner.AllEnemiesInMyLocation.FindAll(c => !c.KeyWords.Contains("Elite")))
                listCardEffects.Add(new CardEffect(
                    card: enemy,
                    effect: () => CleanCard(enemy),
                    animationEffect: ((CardEnemy)enemy.CardLogic).BadEffectForEnemyAnimation,
                    type: EffectType.Choose,
                    name: ThisCard.Info.Name + " a " + enemy.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            yield return new ChooseCardAction(listCardEffects, cancelableCardEffect: playFromHand).RunNow();

            IEnumerator CleanCard(CardComponent enemy)
            {
                cardAffected = enemy;
                BuffActive = true;
                yield return null;
            }
        }

        bool IBuffable.ActiveBuffCondition(GameAction gameAction) => BuffActive;

        void IBuffable.BuffEffect()
        {
            cardAffected.CardTools.ShowBuff(ThisCard);
            cardLogicBackup = cardAffected.CardLogic;
            if (cardAffected.CardLogic is ICleanCard card) card.CleanCard();
            cardAffected.CardLogic = new CardEnemy().WithThisCard(cardAffected);
            cardAffected.VictoryPointModier = -cardAffected.VictoryPoint;
        }

        void IBuffable.DeBuffEffect()
        {
            cardAffected.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            cardAffected.CardLogic = cardLogicBackup;
            cardAffected.VictoryPointModier = 0;
            cardAffected = null;
        }

        protected override bool CheckFilterToCancel() => !PlayingCard();
    }
}