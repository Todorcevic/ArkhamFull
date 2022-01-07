using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class InvestigatorEliminatedAction : GameAction
    {
        public InvestigatorComponent Investigator { get; }

        /*****************************************************************************************/
        public InvestigatorEliminatedAction(InvestigatorComponent investigator) => Investigator = investigator;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            yield return new SelectInvestigatorAction(Investigator).RunNow();
            Investigator.IsBeingEliminated = true;
            Zone currentLocation = Investigator.PlayCard.CurrentZone;
            yield return new AddTokenAction(Investigator.CurrentLocation.CluesToken, Investigator.InvestigatorCardComponent.CluesToken.Amount, Investigator.InvestigatorCardComponent.CluesToken).RunNow();
            yield return new DiscardAction(Investigator.PlayCard, outGame: true, isFast: true, withTopUp: false, withPreview: true, isEliminated: true).RunNow();
            foreach (CardComponent card in Investigator.Threat.ListCards.ToArray())
                if (card.CardType == CardType.Enemy) yield return new MoveCardAction(card, currentLocation).RunNow();
                else yield return new DiscardAction(card, isFast: true, isForced: true, withTopUp: false, withPreview: true, isEliminated: true).RunNow();
            foreach (Zone zone in Investigator.InvestigatorZones)
                foreach (CardComponent card in zone.ListCards.ToArray())
                    yield return new DiscardAction(card, outGame: true, isFast: true, withTopUp: false, withPreview: true, isEliminated: true).RunNow();
            Investigator.gameObject.SetActive(false);
            GameControl.AllInvestigatorsInGame.Remove(Investigator);
            GameControl.InvestigatorsDefeat.Add(Investigator);
            yield return new WaitWhile(() => DOTween.IsTweening("MoveCard"));
            if (GameControl.AllInvestigatorsInGame.Count < 1)
                yield return new FinishGameAction(GameControl.NoResolution).RunNow();
        }
    }
}