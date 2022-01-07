using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01004 : CardInvestigator
    {
        bool effectUsed;
        public override int ChaosTokenWinValue() => ThisCard.SanityToken.Amount;
        public override IEnumerator ChaosTokenWinEffect() => null;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is PhaseAction) effectUsed = false;
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is AddTokenAction addToken && CheckActiveEffect(addToken))
                addToken.ChooseCardOptionalAction.ChosenCardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: ChooseEnemy,
                    animationEffect: ChooseEnemyAnimation,
                    checkFilterToCancel: CheckFilterToCancel,
                    type: EffectType.Reaction,
                    name: "Activar habilidad de " + ThisCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.Owner));
        }

        bool CheckActiveEffect(AddTokenAction addTokenAction)
        {
            if (ThisCard.Owner.AllEnemiesInMyLocation.Count < 1) return false;
            if (effectUsed) return false;
            if (addTokenAction.ToToken == ThisCard.SanityToken && addTokenAction.Amount > 0) return true;
            if (addTokenAction.SecundaryToToken == ThisCard.SanityToken && addTokenAction.SecundaryAmount > 0) return true;
            return false;
        }

        IEnumerator ChooseEnemyAnimation() =>
            new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect4).RunNow();

        IEnumerator ChooseEnemy()
        {
            List<CardEffect> cardEffects = new List<CardEffect>();
            foreach (CardComponent enemy in ThisCard.Owner.AllEnemiesInMyLocation)
                cardEffects.Add(new CardEffect(
                    card: enemy,
                    effect: () => DoDamage(enemy),
                    animationEffect: ((CardEnemy)enemy.CardLogic).BadEffectForEnemyAnimation,
                    type: EffectType.Choose,
                    name: "Seleccionar " + enemy.Info.Name + " para que reciba el daño",
                    investigatorImageCardInfoOwner: enemy.VisualOwner));
            yield return new ChooseCardAction(cardEffects, isOptionalChoice: true).RunNow();

            IEnumerator DoDamage(CardComponent card)
            {
                yield return new AddTokenAction(card.HealthToken, 1).RunNow();
                effectUsed = true;
            }
        }

        bool CheckFilterToCancel() => ThisCard.Owner.AllEnemiesInMyLocation.Count < 1;
    }
}