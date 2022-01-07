using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01043 : CardEvent
    {
        protected override bool IsFast => true;

        /*****************************************************************************************/
        protected override bool CheckPlayedFromHand()
        {
            if (!(GameControl.CurrentPhase is InvestigationPhase)) return false;
            if (GameControl.TurnInvestigator != ThisCard.VisualOwner) return false;
            if (!(GameControl.CurrentInteractableAction is InvestigatorTurn) && !GameControl.CurrentInteractableAction.CanPlayFastAction) return false;
            return base.CheckPlayedFromHand();
        }

        protected override IEnumerator LogicEffect()
        {
            List<CardEffect> cardEffects = new List<CardEffect>();
            foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame.FindAll(i => i.CurrentLocation == ThisCard.VisualOwner.CurrentLocation))
                cardEffects.Add(new CardEffect(
                    card: investigator.PlayCard,
                    effect: () => DrawCards(investigator.PlayCard),
                    animationEffect: () => DrawCardAnimation(investigator),
                    type: EffectType.Choose,
                    name: investigator.PlayCard.Info.Name + " robara cartas",
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            yield return new ChooseCardAction(cardEffects, cancelableCardEffect: playFromHand).RunNow();
        }

        IEnumerator DrawCardAnimation(InvestigatorComponent investigator) => new AnimationCardAction(investigator.PlayCard, audioClip: investigator.InvestigatorCardComponent.Effect8).RunNow();

        IEnumerator DrawCards(CardComponent investigator)
        {
            for (int i = 0; i < 3; i++)
                yield return new DrawAction(investigator.Owner, withShow: false).RunNow();
        }
    }
}