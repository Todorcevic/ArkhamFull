using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class DiscardAction : GameAction
    {
        readonly bool isEliminated;
        readonly bool withPreview;
        readonly bool withTopUp;
        public CardComponent ThisCard { get; }
        public bool IsForced { get; }
        public bool Fast { get; }
        public Zone ZoneToDiscard { get; }

        /*****************************************************************************************/
        public DiscardAction(CardComponent card, bool isFast = false, bool outGame = false, bool victoryZone = false, bool withPreview = false, bool isForced = false, bool withTopUp = true, bool isEliminated = false)
        {
            ThisCard = card;
            Fast = isFast;
            IsForced = isForced;
            this.isEliminated = isEliminated;
            this.withPreview = withPreview;
            this.withTopUp = withTopUp;
            if (outGame || (ThisCard.Owner?.IsDefeat ?? false)) ZoneToDiscard = AllComponents.CardBuilder.Zone;
            else ZoneToDiscard = ThisCard.Owner?.InvestigatorDiscard ?? AllComponents.Table.EncounterDiscard;
            if (victoryZone) ZoneToDiscard = AllComponents.Table.VictoryZone;
            if (ThisCard.CurrentZone.ZoneType == Zones.Hand)
            {
                this.withTopUp = false;
                this.withPreview = true;
            }
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (IsForced || ThisCard.CanBeDiscard)
            {
                ThisCard.IsDiscarting = true;
                yield return new SelectInvestigatorAction(ThisCard.VisualOwner).RunNow();
                foreach (TokenComponent token in ThisCard.AllTokens.FindAll(t => t.Amount > 0))
                    yield return new AddTokenAction(token, -token.Amount).RunNow();
                if (withTopUp)
                    yield return ThisCard.transform.DOLocalMoveY(1, GameData.ANIMATION_TIME_DEFAULT).WaitForCompletion();
                yield return new MoveCardAction(ThisCard, ZoneToDiscard, isFast: Fast, withPreview: withPreview).RunNow();
                ThisCard.CardTools.ChangeColor(Color.white);
                ThisCard.IsExausted = false;
                ThisCard.IsDiscarting = false;
                ThisCard.IsEliminated = isEliminated;
            }
            else throw new System.ArgumentException("Cant discard this card with canBeDiscard = false ", ThisCard.ID);
        }
    }
}