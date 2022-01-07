using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01026 : CardEvent
    {
        protected override bool CheckPlayedFromHand()
        {
            if (!GameControl.AllCardComponents.Exists(c => c.CardType == CardType.Asset && c.IsInPlay && c.VisualOwner?.CurrentLocation == ThisCard.VisualOwner?.CurrentLocation && c.KeyWords.Contains("Firearm"))) return false;
            return base.CheckPlayedFromHand();
        }

        protected override IEnumerator LogicEffect()
        {
            List<CardEffect> weaponsToEffect = new List<CardEffect>();
            foreach (CardComponent fireArm in GameControl.AllCardComponents.FindAll(c => c.CardType == CardType.Asset && c.IsInPlay && c.VisualOwner?.CurrentLocation == ThisCard.VisualOwner?.CurrentLocation && c.KeyWords.Contains("Firearm")))
                weaponsToEffect.Add(new CardEffect(
                    card: fireArm,
                    effect: () => new AddTokenAction(fireArm.ResourcesToken, 3).RunNow(),
                    type: EffectType.Choose,
                    name: "Agregar munición a " + fireArm.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            yield return new ChooseCardAction(weaponsToEffect, cancelableCardEffect: playFromHand).RunNow();
        }
    }
}