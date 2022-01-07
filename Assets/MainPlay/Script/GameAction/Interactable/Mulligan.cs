using System.Collections;
using System.Linq;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class Mulligan : InteractableAction
    {
        public override string GameActionInfo => "Cambiar la mano de " + ActiveInvestigator.InvestigatorCardComponent.Info.Real_name;

        /*****************************************************************************************/
        public override void SetButton() => AllComponents.ReadyButton.SetReadyButton(this, state: ButtonState.Ready);

        public override void ReadyClicked(ReadyButton button)
        {
            if (cardSelected != null) CardPlay(cardSelected);
            else
            {
                new SelectInvestigatorAction(ActiveInvestigator).AddActionTo();
                new DrawInitialHand(ActiveInvestigator).AddActionTo();
                new MulliganEnd(ActiveInvestigator).AddActionTo();
            }
            AnyIsClicked = true;
        }

        protected override void PlayableCards()
        {
            foreach (CardComponent card in ActiveInvestigator.Hand.ListCards)
                CardEffects.Add(new CardEffect(
                    card: card,
                    effect: () => new MoveCardAction(card, ActiveInvestigator.InvestigatorDiscard).RunNow(),
                    type: EffectType.Choose,
                    name: "Descartar " + card.Info.Name));

            foreach (CardComponent card in ActiveInvestigator.InvestigatorDiscard.ListCards)
                if (card.Info.Subtype_code != "weakness" && card.Info.Subtype_code != "basicweakness")
                    CardEffects.Add(new CardEffect(
                        card: card,
                        effect: () => ReturnMulliganCard(card),
                        type: EffectType.Choose,
                        name: "Devolver " + card.Info.Name));

            IEnumerator ReturnMulliganCard(CardComponent card)
            {
                if (card.CurrentZone.ListCards.Last() != card)
                    yield return card.transform.DOLocalMoveX(1, GameData.ANIMATION_TIME_DEFAULT).WaitForCompletion();
                yield return new MoveCardAction(card, ActiveInvestigator.Hand).RunNow();
            }
        }
    }
}