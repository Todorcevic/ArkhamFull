using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01024 : CardEvent
    {
        protected override IEnumerator LogicEffect()
        {
            List<CardComponent> locationsToChoose;
            List<CardEffect> locationsToEffect = new List<CardEffect>();
            LocationSymbol thisLocationSymbol = ((CardLocation)ThisCard.VisualOwner.CurrentLocation.CardLogic).MySymbol;
            locationsToChoose = GameControl.AllCardComponents.FindAll(c => c.CardType == CardType.Location && c.IsInPlay && ((CardLocation)c.CardLogic).MovePosibilities.HasFlag(thisLocationSymbol));
            locationsToChoose.Add(ThisCard.VisualOwner.CurrentLocation);
            foreach (CardComponent location in locationsToChoose)
                locationsToEffect.Add(new CardEffect(
                    card: location,
                    effect: () => Explosion(location),
                    type: EffectType.Choose,
                    name: "Lanzar dinamita a " + location.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            yield return new ChooseCardAction(locationsToEffect, cancelableCardEffect: playFromHand).RunNow();

            IEnumerator Explosion(CardComponent location)
            {
                foreach (CardComponent enemy in GameControl.AllCardComponents.FindAll(c => c.CardType == CardType.Enemy && ((CardEnemy)c.CardLogic).CurrentLocation == location))
                    yield return new DamageEnemyAction(enemy, 3).RunNow();
                foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame.FindAll(i => i.CurrentLocation == location))
                    yield return new AssignDamageHorror(investigator, damageAmount: 3).RunNow();
            }
        }
    }
}