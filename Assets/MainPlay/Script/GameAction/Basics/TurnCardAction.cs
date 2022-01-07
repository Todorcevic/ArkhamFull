using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class TurnCardAction : GameAction
    {
        public sealed override GameActionType GameActionType => GameActionType.Basic;
        public bool IsBack { get; }
        public bool WithPause { get; }
        public CardComponent Card { get; }

        /*****************************************************************************************/
        public TurnCardAction(CardComponent card, bool isBack, bool withPause = false)
        {
            Card = card;
            IsBack = isBack;
            WithPause = withPause;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (Card.IsEliminated) yield break;
            yield return new SelectInvestigatorAction(Card.VisualOwner).RunNow();
            yield return Card.TurnDown(IsBack);
            if (WithPause) yield return new WaitWhile(() => DOTween.IsTweening("IsRotating"));
        }
    }
}