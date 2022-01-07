using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class AssignDamageHorror : InteractableCenterShow
    {
        public int DamageAmount { get; }
        public int HorrorAmount { get; }
        int TotalDamageAssign => ChosenCardEffects.Select(c => c.Card.HealthToken.AssignValue).Sum();
        int TotalHorrorAssign => ChosenCardEffects.Select(c => c.Card.SanityToken.AssignValue).Sum();
        public InvestigatorComponent Investigator { get; set; }

        /*****************************************************************************************/
        public AssignDamageHorror(InvestigatorComponent investigator, int damageAmount = 0, int horrorAmount = 0)
        {
            Investigator = investigator;
            DamageAmount = damageAmount;
            HorrorAmount = horrorAmount;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (ActiveInvestigator.IsDefeat) yield break;
            PlayableCards();
            if (ChosenCardEffects.Count > 1)
            {
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
            }
            else
            {
                ChosenCardEffects[0].Card.HealthToken.AssignValue = DamageAmount;
                ChosenCardEffects[0].Card.SanityToken.AssignValue = HorrorAmount;
            }
            yield return DoDamageAndHorror();
        }

        IEnumerator DoDamageAndHorror()
        {
            foreach (CardComponent card in ChosenCardEffects.FindAll(c => c.Card.SanityToken.AssignValue > 0 || c.Card.HealthToken.AssignValue > 0).Select(c => c.Card))
            {
                yield return new AddTokenAction(card.HealthToken, card.HealthToken.AssignValue, secundaryToToken: card.SanityToken, secundaryAmount: card.SanityToken.AssignValue).RunNow();
                card.HealthToken.AssignValue = 0;
                card.SanityToken.AssignValue = 0;
            }
        }

        public override void SetButton()
        {
            if (TotalDamageAssign >= DamageAmount && TotalHorrorAssign >= HorrorAmount)
                AllComponents.ReadyButton.SetReadyButton(this, state: ButtonState.Ready);
            else
            {
                AllComponents.ReadyButton.SetReadyButton(this, state: ButtonState.Off);
                AllComponents.ReadyButton.ChangeButtonText((DamageAmount - TotalDamageAssign) + " de daño por asignar." + Environment.NewLine + (HorrorAmount - TotalHorrorAssign) + " de horror por asignar.");
            }

            foreach (CardComponent card in ChosenCardEffects.Select(c => c.Card))
            {
                card.HealthToken.TokenText((card.HealthToken.Amount + card.HealthToken.AssignValue).ToString());
                card.HealthToken.ButtonUpActive(card.HealthToken.Amount + card.HealthToken.AssignValue < (card.Info.Health ?? 0) && DamageAmount - TotalDamageAssign > 0);
                card.HealthToken.ButtonDownActive(card.HealthToken.AssignValue > 0);

                card.SanityToken.TokenText((card.SanityToken.Amount + card.SanityToken.AssignValue).ToString());
                card.SanityToken.ButtonUpActive(card.SanityToken.Amount + card.SanityToken.AssignValue < (card.Info.Sanity ?? 0) && HorrorAmount - TotalHorrorAssign > 0);
                card.SanityToken.ButtonDownActive(card.SanityToken.AssignValue > 0);
            }
        }

        void ActiveTokens(bool isActive)
        {
            foreach (CardComponent card in ChosenCardEffects.Select(c => c.Card))
            {
                if (isActive)
                {
                    card.ResourcesToken.ShowToken(false);
                    card.DoomToken.ShowToken(false);
                    card.CluesToken.ShowToken(false);
                    card.HealthToken.ShowToken(true);
                    card.SanityToken.ShowToken(true);
                    SetButton();
                }
                else
                {
                    card.HealthToken.ShowAmount();
                    card.HealthToken.HideButtons();
                    card.SanityToken.ShowAmount();
                    card.SanityToken.HideButtons();
                    card.ResourcesToken.ShowAmount();
                    card.DoomToken.ShowAmount();
                    card.CluesToken.ShowAmount();
                }
            }
        }

        protected override void PlayableCards()
        {
            List<CardComponent> ListCardsToAssign = new List<CardComponent>();
            if (DamageAmount > TotalDamageAssign)
                ListCardsToAssign.AddRange(Investigator.Assets.ListCards.FindAll(c => c.Info.Health > 0));
            if (HorrorAmount > TotalDamageAssign)
                ListCardsToAssign.AddRange(Investigator.Assets.ListCards.FindAll(c => c.Info.Sanity > 0).Except(ListCardsToAssign));
            ListCardsToAssign.Add(Investigator.InvestigatorCardComponent);
            foreach (CardComponent card in ListCardsToAssign)
                ChosenCardEffects.Add(new CardEffect(
                    card: card,
                    effect: () => null,
                    type: EffectType.Choose,
                    name: "Asignar puntos"));
        }

        public override void ReadyClicked(ReadyButton button) => AnyIsClicked = true;

        public override void CardSelected(CardComponent card) { }
        public override void CardPlay(CardComponent card) { }
    }
}