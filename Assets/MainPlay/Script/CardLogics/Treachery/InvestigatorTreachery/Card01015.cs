using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01015 : InvestigatorTreachery
    {
        public override IEnumerator Revelation()
        {
            yield return new AddTokenAction(Investigator.InvestigatorCardComponent.SanityToken, 2).RunNow();
            yield return new MoveDeck(Investigator.InvestigatorDiscard.ListCards, AllComponents.CardBuilder.Zone).RunNow();
        }
    }
}