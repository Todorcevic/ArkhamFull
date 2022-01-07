using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01050 : CardEvent
    {
        bool isMoveToLocation;
        protected override bool IsFast => true;

        /*****************************************************************************************/
        protected override bool CheckPlayedFromHand()
        {
            if (!(GameControl.CurrentPhase is InvestigationPhase)) return false;
            if (GameControl.TurnInvestigator != ThisCard.VisualOwner) return false;
            if (!ThisCard.VisualOwner.IsEnganged
                && !GameControl.AllCardComponents.Exists(c => c.CardLogic is CardLocation cardLocation && c.IsInPlay
                    && cardLocation.IsRevealed
                    && ThisCard.VisualOwner.CurrentLocation != c
                    && !c.MyOwnZone.ListCards.Exists(i => i.CardType == CardType.Enemy)
                    && !c.MyOwnZone.ListCards.Exists(i => i.CardType == CardType.PlayCard && i.Owner.IsEnganged))) return false;
            return base.CheckPlayedFromHand();
        }

        protected override IEnumerator LogicEffect()
        {
            Zone zoneToStayEnemies = ThisCard.VisualOwner.CurrentLocation.MyOwnZone;
            List<CardEffect> chooseToMove = new List<CardEffect>();
            foreach (CardComponent location in GameControl.AllCardComponents.FindAll(c => c.CardType == CardType.Location && c.IsInPlay
            && ((CardLocation)c.CardLogic).IsRevealed
            && ThisCard.VisualOwner.CurrentLocation != c
            && !c.MyOwnZone.ListCards.Exists(i => i.CardType == CardType.Enemy)
            && !c.MyOwnZone.ListCards.Exists(i => i.CardType == CardType.PlayCard && i.Owner.IsEnganged)))
            {
                chooseToMove.Add(new CardEffect(
                    card: location,
                    effect: () => MoveAndDesengange(location),
                    type: EffectType.Choose,
                    name: "Moverse aquí",
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            }
            yield return new ChooseCardAction(chooseToMove, cancelableCardEffect: playFromHand).RunNow();

            if (!playFromHand.IsCancel && !isMoveToLocation)
            {
                foreach (CardComponent enemy in ThisCard.VisualOwner.AllEnemiesEnganged)
                    yield return new MoveCardAction(enemy, zoneToStayEnemies, withPreview: false).RunNow();
            }

            IEnumerator MoveAndDesengange(CardComponent location)
            {
                foreach (CardComponent enemy in ThisCard.VisualOwner.AllEnemiesEnganged)
                    enemy.MoveTo(zoneToStayEnemies);
                yield return new MoveCardAction(ThisCard.VisualOwner.PlayCard, location.MyOwnZone).RunNow();
                foreach (CardComponent enemy in ThisCard.VisualOwner.AllEnemiesEnganged)
                    yield return new MoveCardAction(enemy, enemy.CurrentZone, withPreview: false).RunNow();
                isMoveToLocation = true;
            }
        }
    }
}