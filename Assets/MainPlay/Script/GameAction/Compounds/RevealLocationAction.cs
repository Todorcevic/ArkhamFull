using System.Collections;
using DG.Tweening;
using System;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class RevealLocationAction : GameAction
    {
        readonly CardComponent cardLocation;
        readonly CardLocation location;
        public override GameActionType GameActionType => GameActionType.Compound;

        /*****************************************************************************************/
        public RevealLocationAction(CardComponent cardLocation)
        {
            this.cardLocation = cardLocation;
            location = ((CardLocation)cardLocation.CardLogic);
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            yield return new WaitWhile(() => DOTween.IsTweening("MoveCard"));
            location.IsRevealed = true;
            yield return new ShowCardAction(cardLocation, isBack: true, withReturn: true).RunNow();
            yield return new AddTokenAction(cardLocation.CluesToken, location.Clues).RunNow();
        }
    }
}