using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class AssignClues : InteractableCenterShow
    {
        readonly int? cluesLimit;
        List<InvestigatorComponent> investigators;
        public bool IsCancel { get; set; }
        public int CluesAmount => ChosenCardEffects.Select(c => c.Card.CluesToken.AssignValue).Sum();

        /*****************************************************************************************/
        public AssignClues(List<InvestigatorComponent> investigators, int? cluesLimit = null)
        {
            this.investigators = investigators;
            this.cluesLimit = cluesLimit;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (ActiveInvestigator.IsDefeat) yield break;
            PlayableCards();
            SetButton();
            ActiveTokens(true);
            yield return ShowPreviewCards();
            AllComponents.ShowHideChooseCard.ICenterShowableAction = this;
            GameControl.FlowIsRunning = false;
            yield return new WaitUntil(() => AnyIsClicked);
            GameControl.FlowIsRunning = true;
            AllComponents.ShowHideChooseCard.ICenterShowableAction = null;
            AllComponents.ReadyButton.State = ButtonState.Off;
            yield return ShowTable();
            ActiveTokens(false);
            AnyIsClicked = false;
            if (!IsCancel) yield return AddingAllClues();
            ChosenCardEffects.ForEach(c => c.Card.CluesToken.AssignValue = 0);
        }

        IEnumerator AddingAllClues()
        {
            foreach (CardComponent card in ChosenCardEffects.FindAll(c => c.Card.CluesToken.AssignValue > 0).Select(c => c.Card))
                yield return new AddTokenAction(GameControl.CurrentAct.ThisCard.CluesToken, card.CluesToken.AssignValue, card.CluesToken).RunNow();
        }

        protected override void PlayableCards()
        {
            foreach (CardComponent investigatorCard in investigators.FindAll(i => i.Clues > 0).Select(i => i.InvestigatorCardComponent))
                ChosenCardEffects.Add(new CardEffect(
                    card: investigatorCard,
                    effect: () => null,
                    type: EffectType.Choose,
                    name: "Asignar pistas",
                    investigatorImageCardInfoOwner: investigatorCard.Owner,
                    needSpendClues: true
                    ));
        }

        public override void SetButton()
        {
            if (CluesAmount < (cluesLimit ?? 1))
            {
                string cluesLeft = string.Empty;
                if (cluesLimit != null) cluesLeft = " (" + (cluesLimit - CluesAmount) + " pistas por entregar)";
                IsCancel = true;
                AllComponents.ReadyButton.SetReadyButton(this, state: ButtonState.StandBy);
                AllComponents.ReadyButton.ChangeButtonText("Cancelar" + cluesLeft);
            }
            else
            {
                IsCancel = false;
                AllComponents.ReadyButton.SetReadyButton(this, state: ButtonState.Ready);
                AllComponents.ReadyButton.ChangeButtonText("Entregar " + CluesAmount + " pistas.");
            }

            foreach (CardComponent card in ChosenCardEffects.Select(c => c.Card))
            {
                card.CluesToken.TokenText((card.CluesToken.Amount - card.CluesToken.AssignValue).ToString());
                card.CluesToken.ButtonUpActive(card.CluesToken.Amount - card.CluesToken.AssignValue > 0 && CluesAmount < (cluesLimit ?? 99));
                card.CluesToken.ButtonDownActive(card.CluesToken.AssignValue > 0);
            }
        }

        public override void ReadyClicked(ReadyButton button) => AnyIsClicked = true;

        void ActiveTokens(bool isActive)
        {
            foreach (CardComponent card in ChosenCardEffects.Select(c => c.Card))
            {
                if (isActive)
                {
                    card.ResourcesToken.ShowToken(false);
                    card.DoomToken.ShowToken(false);
                    card.CluesToken.ShowToken(true);
                    card.HealthToken.ShowToken(false);
                    card.SanityToken.ShowToken(false);
                    SetButton();
                }
                else
                {
                    card.HealthToken.ShowAmount();
                    card.SanityToken.ShowAmount();
                    card.ResourcesToken.ShowAmount();
                    card.DoomToken.ShowAmount();
                    card.CluesToken.ShowAmount();
                    card.CluesToken.HideButtons();
                }
            }
        }

        public override void CardSelected(CardComponent card) { }
        public override void CardPlay(CardComponent card) { }
    }
}