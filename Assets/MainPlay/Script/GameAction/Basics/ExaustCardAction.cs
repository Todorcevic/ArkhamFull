using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class ExaustCardAction : GameAction
    {
        readonly bool withPause;
        public bool IsExausted { get; set; }
        public CardComponent Card { get; set; }

        /*****************************************************************************************/
        public ExaustCardAction(CardComponent card, bool isExausted = true, bool withPause = true)
        {
            this.withPause = withPause;
            Card = card;
            IsExausted = isExausted;
        }

        protected override IEnumerator ActionLogic()
        {
            if (Card.IsOutGame || IsExausted == Card.IsExausted) yield break;
            if (withPause) yield return new SelectInvestigatorAction(Card.VisualOwner).RunNow();
            Card.Exhaust(IsExausted);
            if (withPause) yield return new WaitWhile(() => DOTween.IsTweening("Exhausting"));
        }
    }
}