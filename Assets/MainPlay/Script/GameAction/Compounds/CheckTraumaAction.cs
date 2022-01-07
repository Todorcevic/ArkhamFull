using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class CheckTraumaAction : GameAction
    {
        List<CardEffect> traumasCardEffects;
        CardComponent investigator;

        /*****************************************************************************************/
        public CheckTraumaAction(CardComponent investigator) => this.investigator = investigator;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (investigator.HealthToken.Amount >= investigator.Info.Health)
            {
                traumasCardEffects.Add(
                new CardEffect(
                    card: investigator,
                    effect: Physical,
                    type: EffectType.Choose,
                    name: "Recibiras un trauma físico",
                    investigatorImageCardInfoOwner: investigator.Owner));
            }

            if (investigator.SanityToken.Amount >= investigator.Info.Sanity)
            {
                traumasCardEffects.Add(
                new CardEffect(
                    card: investigator,
                    effect: Mental,
                    type: EffectType.Choose,
                    name: "Recibiras un trauma mental",
                    investigatorImageCardInfoOwner: investigator.Owner));
            }
            yield return new ChooseCardAction(traumasCardEffects, isOptionalChoice: false).RunNow();
        }

        IEnumerator Physical()
        {
            investigator.Owner.PhysicalTraumas++;
            yield return null;
        }

        IEnumerator Mental()
        {
            investigator.Owner.MentalTraumas++;
            yield return null;
        }
    }
}