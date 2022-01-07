using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace ArkhamGamePlay
{
    public abstract class InteractableAction : GameAction, IButtonClickable
    {
        protected CardComponent cardSelected;
        protected bool AreThereCardsEnabled => CardEffects.Count > 0;
        public sealed override GameActionType GameActionType => GameActionType.Interactable;
        public InvestigatorComponent ActiveInvestigator => GameControl.ActiveInvestigator;
        protected bool AnyIsClicked { get; set; }
        protected bool ContinueNextAction { get; set; }
        public virtual bool CanPlayFastAction => false;
        public List<CardEffect> CardEffects { get; set; } = new List<CardEffect>();

        /*****************************************************************************************/
        protected abstract void PlayableCards();
        public abstract void ReadyClicked(ReadyButton button);
        public virtual void SetButton() { }

        protected virtual IEnumerator ActionLogicBegin()
        {
            PlayableCards();
            yield return new CardEffectsFilterAction(CardEffects).RunNow();
        }

        protected override IEnumerator ActionLogic()
        {
            if (ActiveInvestigator.IsDefeat) yield break;
            yield return ActionLogicBegin();
            CheckCardEffectsAmount();
            if (!AnyIsClicked)
            {
                ActivateCards(true);
                SetButton();
            }
            yield return new WaitUntil(() => AnyIsClicked);
            ActionLogicEnd();
        }

        protected virtual void ActionLogicEnd()
        {
            ActivateCards(false);
            cardSelected = null;
            AnyIsClicked = false;
            AllComponents.ReadyButton.State = ButtonState.Off;
            AllComponents.PanelSkillTest.ReadyButton.State = ButtonState.Off;
        }

        protected virtual void CheckCardEffectsAmount()
        {
            if (!AreThereCardsEnabled) AnyIsClicked = true;
        }

        void ActivateCards(bool activate)
        {
            GameControl.FlowIsRunning = !activate;
            if (activate)
            {
                CardSensor.onMouseOverThisCard?.OnMouseEnter();
                foreach (CardComponent card in CardEffects.Select(c => c.Card).Distinct())
                    card.CanBePlayedNow = true;
                foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame)
                    investigator.CheckIsSelectable();
            }
            else
            {
                foreach (CardComponent card in GameControl.AllCardComponents)
                {
                    card.CardSensor.OnMouseExit();
                    card.CanBePlayedNow = false;
                }
                foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame)
                    investigator.SelectableGlow.enabled = false;
            }
        }

        public virtual void CardPlay(CardComponent card)
        {
            ResolvingPlayCardRules(card);
            new EffectAction(RepeatThisInteractableAction).AddActionTo(); // We need Add RepeatThisInteractableAction after others GameActions.AddActionTo
            AnyIsClicked = true;
        }

        void ResolvingPlayCardRules(CardComponent card)
        {
            List<CardEffect> listCardEffects = CardEffects.FindAll(c => c.Card == card);
            if (listCardEffects.Count > 1) new MultiCastAction(listCardEffects).AddActionTo();
            else
            {
                new PlayCardAction(listCardEffects[0]).AddActionTo();
                new ExecuteCardAction(listCardEffects[0]).AddActionTo();
            }
        }

        protected IEnumerator RepeatThisInteractableAction()
        {
            if (!ContinueNextAction && !ActiveInvestigator.IsDefeat)
            {
                CardEffects.Clear();
                GameAction parent = ActionParent;
                AddActionTo();
                ActionParent = parent;
            }
            yield return null;
        }

        public virtual void CardSelected(CardComponent card)
        {
            if (cardSelected) cardSelected.CanBePlayedNow = true;
            if (card == cardSelected || card == null)
            {
                cardSelected = null;
                SetButton();
            }
            else
            {
                card.CardState = CardState.Selected;
                cardSelected = card;
                string fullText = null;
                AllComponents.ReadyButton.SetReadyButton(this, state: ButtonState.Ready);
                foreach (CardEffect cardEffect in CardEffects.FindAll(c => c.Card == card))
                    fullText += cardEffect.TakeEffectTypeIcon() + cardEffect.Name + Environment.NewLine;
                AllComponents.ReadyButton.ChangeButtonText(fullText);
            }
        }
    }
}