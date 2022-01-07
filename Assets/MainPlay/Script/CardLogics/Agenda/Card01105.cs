using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01105 : CardAgenda
    {
        public override IEnumerator BackFace()
        {
            List<CardEffect> cardEffects = new List<CardEffect>()
        {
        new CardEffect(
            card: ThisCard,
            effect: InvestigatorsDiscard,
            type: EffectType.Choose,
            name: "Descartar cartas",
            investigatorImageCardInfoOwner: GameControl.LeadInvestigator),
        new CardEffect(
            card: ThisCard,
            effect: InvestigatorTakeHorror,
            type: EffectType.Choose,
            name: "Recibir horror",
            investigatorImageCardInfoOwner: GameControl.LeadInvestigator)
        };
            yield return new ActiveInvestigatorAction(GameControl.LeadInvestigator).RunNow();
            yield return new TurnCardAction(ThisCard, true, withPause: true).RunNow();
            yield return new MultiCastAction(cardEffects, isOptionalChoice: false).RunNow();
        }

        IEnumerator InvestigatorsDiscard()
        {
            foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame)
            {
                List<CardComponent> cardsInHand = investigator.Hand.ListCards.FindAll(c => c.CanBeDiscard);
                yield return new DiscardAction(cardsInHand[Random.Range(0, cardsInHand.Count - 1)], withPreview: true, withTopUp: false).RunNow();
            }
        }

        IEnumerator InvestigatorTakeHorror() =>
             new AssignDamageHorror(GameControl.LeadInvestigator, horrorAmount: 2).RunNow();
    }
}