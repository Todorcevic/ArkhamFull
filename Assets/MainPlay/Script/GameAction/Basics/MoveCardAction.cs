using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class MoveCardAction : GameAction
    {
        bool needCoroutine;
        public override GameActionType GameActionType => GameActionType.Basic;
        bool IsBack { get; }
        bool ReturnIsBack { get; }
        bool WithPreview { get; }
        bool IsFast { get; }
        float PreviewPause { get; }
        float timeMove;
        InvestigatorComponent investigatorToSelect;
        public CardComponent ThisCard { get; }
        public Zone Zone { get; }
        public Zone OldZone { get; }

        /*****************************************************************************************/
        public MoveCardAction(CardComponent card, Zone zone, bool withPreview = true, bool? isBack = null, bool? returnIsBack = null, bool isFast = false, float previewPause = 0, float timeMove = GameData.ANIMATION_TIME_DEFAULT)
        {
            ThisCard = card;
            Zone = zone;
            WithPreview = withPreview;
            PreviewPause = previewPause;
            IsBack = isBack ?? card.IsBack;
            ReturnIsBack = returnIsBack ?? IsBack;
            IsFast = isFast;
            OldZone = card.CurrentZone;
            this.timeMove = timeMove;
            investigatorToSelect = GameControl.AllInvestigatorsInGame.Find(i => i.InvestigatorZones.Contains(Zone));
            needCoroutine = investigatorToSelect != null
                && investigatorToSelect != ThisCard.VisualOwner
                && (ThisCard.CurrentZone.ZoneType != Zones.CardBuilder || ThisCard.IsInCenterPreview);
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (ThisCard.IsEliminated || (investigatorToSelect?.IsDefeat ?? false)) yield break;
            if (!IsFast) yield return new SelectInvestigatorAction(ThisCard.VisualOwner).RunNow();
            if (WithPreview) yield return DOTween.Sequence().Append(ThisCard.Preview(timePause: PreviewPause))
                 .Join(ThisCard.TurnDown(IsBack)).SetId("MoveCard");
            yield return DOTween.Sequence().Append(ThisCard.MoveTo(Zone, timeAnimation: timeMove))
                 .Join(ThisCard.TurnDown(ReturnIsBack))
                 .SetDelay(WithPreview ? GameData.ANIMATION_TIME_DEFAULT * 2 + PreviewPause : 0)
                 .PrependCallback(ChangingInvestigatorWithPreviewCard)
                 .SetId("MoveCard");

            DOTween.Kill(ThisCard.transform);
            if (!IsFast) yield return new WaitWhile(() => DOTween.IsTweening("MoveCard"));
        }

        void ChangingInvestigatorWithPreviewCard()
        {
            if (needCoroutine)
                AllComponents.InvestigatorManagerComponent.SelectInvestigatorCoroutine(investigatorToSelect);
        }
    }
}