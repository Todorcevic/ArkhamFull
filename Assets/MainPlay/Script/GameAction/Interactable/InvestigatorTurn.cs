using System.Collections;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;
using ArkhamGamePlay;

namespace ArkhamGamePlay
{
    public class InvestigatorTurn : InteractableAction
    {
        public override string GameActionInfo => "Turno de " + ActiveInvestigator.InvestigatorCardComponent.Info.Name + ". (" + ActiveInvestigator.ActionsLeft + " acciones restantes.)";
        public override bool CanPlayFastAction => true;
        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            yield return new SelectInvestigatorAction(ActiveInvestigator).RunNow();
            yield return base.ActionLogic();
        }

        public override void SetButton()
        {
            AllComponents.ReadyButton.SetReadyButton(this);
            string textInfoAction = "Acciones: " + ActiveInvestigator.ActionsLeft;
            AllComponents.ReadyButton.ChangeButtonText(textInfoAction);
            ActiveInvestigator.ShowInfo(textInfoAction);
            if (ActiveInvestigator.ActionsLeft <= 0) AllComponents.ReadyButton.State = ButtonState.Ready;
            else AllComponents.ReadyButton.State = ButtonState.StandBy;
        }

        public override void ReadyClicked(ReadyButton button)
        {
            if (cardSelected != null) CardPlay(cardSelected);
            else if (ActiveInvestigator.ActionsLeft > 0)
            {
                new ChooseCardAction(new CardEffect(
                    card: ActiveInvestigator.InvestigatorCardComponent,
                    effect: EndingTurn,
                    type: EffectType.Choose,
                    name: ActiveInvestigator.Name + " perderá " + ActiveInvestigator.ActionsLeft + " acciones.")
                , isOptionalChoice: true).AddActionTo();
                new EffectAction(RepeatThisInteractableAction).AddActionTo(); // We need Add RepeatThisInteractableAction after others GameActions.AddActionTo
            }
            AnyIsClicked = true;

            IEnumerator EndingTurn()
            {
                yield return new UpdateActionsLeft(ActiveInvestigator, -ActiveInvestigator.ActionsLeft).RunNow();
                ContinueNextAction = true;
            }
        }

        protected override void CheckCardEffectsAmount()
        {
            if (!AreThereCardsEnabled) AnyIsClicked = true;
        }

        protected override void PlayableCards()
        {
            if (GameControl.TopDeck)
            {
                CardEffects.Add(new CardEffect(
                    card: GameControl.TopDeck,
                    effect: DrawCard,
                    type: EffectType.Draw,
                    name: "Robar",
                    actionCost: 1)
                    );
                IEnumerator DrawCard()
                {
                    yield return new SelectInvestigatorAction(ActiveInvestigator).RunNow();
                    yield return new DrawAction(ActiveInvestigator, withShow: false).RunNow();
                }
            }
            else
            {
                CardComponent drawCard = ActiveInvestigator.InvestigatorDiscard.ListCards.Last();
                CardEffects.Add(new CardEffect(
                    card: drawCard,
                    effect: DrawCard,
                    type: EffectType.Draw,
                    name: "Devolver descartes al Mazo y Robar. (Esta acción te hace 1 horror)",
                    actionCost: 1)
                    );
                IEnumerator DrawCard()
                {
                    yield return new WaitWhile(() => DOTween.IsTweening(drawCard.transform));
                    yield return new DrawAction(ActiveInvestigator, withShow: false).RunNow();
                }
            }
        }
    }
}