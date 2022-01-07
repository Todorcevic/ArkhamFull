using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ArkhamGamePlay
{
    public class CardInvestigator : CardLogic
    {
        public virtual int ChaosTokenWinValue() => 0;
        public virtual IEnumerator ChaosTokenWinEffect() => null;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is MoveCardAction moveCardAction && CheckSlots(moveCardAction))
                new CheckDiscardAssetsAction(ThisCard.Owner, moveCardAction.ThisCard).AddActionTo();
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is MoveCardAction)
                new CheckControlAssetSlotsAction(ThisCard.Owner).AddActionTo();
            if (gameAction is AddTokenAction addTokenAction && CheckDie(addTokenAction))
                new EffectAction(Dying).AddActionTo();
        }

        bool CheckSlots(MoveCardAction moveCardAction)
        {
            if (moveCardAction.ThisCard.Info.Slot == null) return false;
            if (moveCardAction.Zone != ThisCard.Owner.Assets && moveCardAction.Zone != ThisCard.Owner.Threat) return false;
            return true;
        }

        bool CheckDie(AddTokenAction addTokenAction)
        {
            if (addTokenAction.ToToken != ThisCard.HealthToken && addTokenAction.ToToken != ThisCard.SanityToken
                && addTokenAction.SecundaryToToken != ThisCard.HealthToken && addTokenAction.SecundaryToToken != ThisCard.SanityToken) return false;
            if (ThisCard.HealthToken.Amount < ThisCard.Info.Health && ThisCard.SanityToken.Amount < ThisCard.Info.Sanity) return false;
            if (ThisCard.Owner.IsBeingEliminated) return false;
            return true;
        }

        IEnumerator Dying()
        {
            if (ThisCard.HealthToken.Amount >= ThisCard.Info.Health && ThisCard.SanityToken.Amount >= ThisCard.Info.Sanity)
            {
                List<CardEffect> traumasCardEffect = new List<CardEffect>()
            {
            new CardEffect(
                card: ThisCard,
                effect: PhysicalTrauma,
                type: EffectType.Choose,
                name: "Recibir un trauma físico",
                investigatorImageCardInfoOwner: ThisCard.Owner),

            new CardEffect(
                card: ThisCard,
                effect: MentalTrauma,
                type: EffectType.Choose,
                name: "Recibir un trauma mental",
                investigatorImageCardInfoOwner: ThisCard.Owner)
            };
                yield return new MultiCastAction(traumasCardEffect, isOptionalChoice: false).RunNow();
            }

            else if (ThisCard.HealthToken.Amount >= ThisCard.Info.Health)
                yield return PhysicalTrauma();
            else if (ThisCard.SanityToken.Amount >= ThisCard.Info.Sanity)
                yield return MentalTrauma();
            yield return new InvestigatorEliminatedAction(ThisCard.Owner).RunNow();

            IEnumerator PhysicalTrauma() => new InvestigatorTraumaAction(ThisCard.Owner, ThisCard.Info.Name + " quedó derrotado y sufre 1 trauma físico.", isPhysical: true).RunNow();

            IEnumerator MentalTrauma() => new InvestigatorTraumaAction(ThisCard.Owner, ThisCard.Info.Name + " quedó derrotado y sufre 1 trauma mental.", isPhysical: false).RunNow();
        }

        public IEnumerator ThankYouAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect6).RunNow();
    }
}