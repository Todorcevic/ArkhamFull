using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class RevelationAction : GameAction
    {
        public CardComponent RevelationCard { get; }

        /*****************************************************************************************/
        public RevelationAction(CardComponent card) => RevelationCard = card;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (((IRevelation)RevelationCard.CardLogic).IsDiscarted)
                yield return new MoveCardAction(RevelationCard, AllComponents.Table.PlayCardZone, withPreview: false).RunNow();
            yield return ((IRevelation)RevelationCard.CardLogic).Revelation();
            if (((IRevelation)RevelationCard.CardLogic).IsDiscarted)
                yield return new DefeatCardAction(RevelationCard).RunNow();
        }
    }
}