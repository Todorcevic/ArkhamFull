using System.Collections;
using System.Collections.Generic;

namespace ArkhamGamePlay
{
    public class InvestigatorDiscardHand : GameAction
    {
        bool isOptional;
        readonly InvestigatorComponent investigator;
        public override GameActionType GameActionType => GameActionType.Compound;
        public bool IsCanceled { get; set; }
        public CardComponent CardDiscarted { get; set; }

        /*****************************************************************************************/
        public InvestigatorDiscardHand(InvestigatorComponent investigator, bool isOptional = false)
        {
            this.investigator = investigator;
            this.isOptional = isOptional;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (investigator.IsDefeat) yield break;
            List<CardEffect> cardEffects = new List<CardEffect>();
            foreach (CardComponent card in investigator.Hand.ListCards.FindAll(c => !c.IsWeakness))
                cardEffects.Add(new CardEffect(
                    card: card,
                    effect: () => new DiscardAction(card, withPreview: true, withTopUp: false).RunNow(),
                    type: EffectType.Choose,
                    name: "Descartar " + card.Info.Name,
                    investigatorImageCardInfoOwner: investigator));
            yield return new SelectInvestigatorAction(investigator).RunNow();
            ChooseCardAction chooseCard = new ChooseCardAction(cardEffects, isOptionalChoice: isOptional, withPreview: false, buttonText: isOptional ? "Descarta una carta o pulsa aquí para cancelar." : "Debes descartar una carta.");
            yield return chooseCard.RunNow();
            IsCanceled = chooseCard.IsCancel;
            CardDiscarted = chooseCard.CardPlayed;
        }
    }
}