using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class Card01106 : CardAgenda
    {
        public override IEnumerator BackFace()
        {
            yield return AddEncounterDiscard();
            yield return SearchGhoul();
        }

        IEnumerator AddEncounterDiscard()
        {
            yield return new MoveDeck(AllComponents.Table.EncounterDiscard.ListCards, AllComponents.Table.EncounterDeck, isBack: true, withMoveUp: true).RunNow();
            yield return new ShuffleAction(AllComponents.Table.EncounterDeck).RunNow();
        }

        IEnumerator SearchGhoul()
        {
            CardComponent cardDraw;
            do
            {
                cardDraw = AllComponents.Table.EncounterDeck.ListCards.LastOrDefault();
                if (cardDraw == null) yield break;
                yield return cardDraw.TurnDown(false);
                yield return new DiscardAction(cardDraw).RunNow();
            }
            while (cardDraw.CardType != CardType.Enemy || !cardDraw.KeyWords.Contains("Ghoul"));
            yield return new DrawAction(GameControl.LeadInvestigator, cardDraw, withShow: true).RunNow();
        }
    }
}