using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class DrawInitialHand : GameAction
    {
        readonly InvestigatorComponent investigator;

        /*****************************************************************************************/
        public override GameActionType GameActionType => GameActionType.Setting;

        /*****************************************************************************************/
        public DrawInitialHand(InvestigatorComponent investigator) => this.investigator = investigator;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            List<CardComponent> listDraw = investigator.InvestigatorDeck.ListCards.GetRange(investigator.InvestigatorDeck.ListCards.Count - (GameData.DRAW_INITIAL_HAND - investigator.Hand.ListCards.Count), GameData.DRAW_INITIAL_HAND - investigator.Hand.ListCards.Count);
            foreach (CardComponent card in listDraw)
            {
                yield return DOTween.Sequence()
                    .Append(card.Preview())
                    .Join(card.TurnDown(false))
                       .AppendCallback(() => card.MoveTo(investigator.Hand)).SetId("DrawHand")
                       .WaitForPosition(GameData.ANIMATION_TIME_DEFAULT * 1.5f);
            }
            yield return new WaitWhile(() => DOTween.IsTweening("DrawHand"));
            yield return new CheckInitialBasicWeakness(investigator).RunNow();
        }
    }
}