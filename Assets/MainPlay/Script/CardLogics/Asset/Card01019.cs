using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01019 : CardAsset
    {
        CardEffect healDamage;
        CardEffect healHorror;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckActiveEffect(investigatorTurn))
            {
                if (GameControl.AllInvestigatorsInGame.Exists(i => i.CurrentLocation == ThisCard.VisualOwner.CurrentLocation && i.Damage > 0))
                    investigatorTurn.CardEffects.Add(healDamage = new CardEffect(
                        card: ThisCard,
                        effect: HealDamage,
                        animationEffect: HealDamageAnimation,
                        payEffect: PayCostEffect,
                        cancelEffect: CancelCardEffect,
                        type: EffectType.Activate,
                        name: "Curar daño con " + ThisCard.Info.Name,
                        actionCost: 1,
                        investigatorImageCardInfoOwner: ThisCard.VisualOwner));
                if (GameControl.AllInvestigatorsInGame.Exists(i => i.CurrentLocation == ThisCard.VisualOwner.CurrentLocation && i.Horror > 0))
                    investigatorTurn.CardEffects.Add(healHorror = new CardEffect(
                        card: ThisCard,
                        effect: HealHorror,
                        animationEffect: HealDamageAnimation,
                        payEffect: PayCostEffect,
                        cancelEffect: CancelCardEffect,
                        type: EffectType.Activate,
                        name: "Curar horror con " + ThisCard.Info.Name,
                        actionCost: 1,
                        investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            }
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is AddTokenAction && CheckDiscard())
                new DiscardAction(ThisCard).AddActionTo();
        }

        bool CheckActiveEffect(InvestigatorTurn investigatorTurn)
        {
            if (ThisCard.CurrentZone != investigatorTurn.ActiveInvestigator.Assets) return false;
            if (ThisCard.ResourcesToken.Amount < 1) return false;
            return true;
        }

        bool CheckDiscard()
        {
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.ResourcesToken.Amount > 0) return false;
            return true;
        }

        IEnumerator HealDamageAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();


        IEnumerator HealDamage()
        {
            List<CardEffect> allInvestigators = new List<CardEffect>();
            foreach (CardComponent investigator in GameControl.AllInvestigatorsInGame.FindAll(i => i.CurrentLocation == ThisCard.VisualOwner.CurrentLocation && i.Damage > 0).Select(i => i.InvestigatorCardComponent))
                allInvestigators.Add(new CardEffect(
                    card: investigator,
                    effect: () => HealingEffect(investigator.HealthToken),
                    animationEffect: ((CardInvestigator)investigator.CardLogic).ThankYouAnimation,
                    type: EffectType.Choose,
                    name: "Curar daño a " + investigator.Info.Name,
                    investigatorImageCardInfoOwner: investigator.Owner));
            yield return new ChooseCardAction(allInvestigators, cancelableCardEffect: healDamage).RunNow();
        }

        IEnumerator HealHorror()
        {
            List<CardEffect> allInvestigators = new List<CardEffect>();
            foreach (CardComponent investigator in GameControl.AllInvestigatorsInGame.FindAll(i => i.CurrentLocation == ThisCard.VisualOwner.CurrentLocation && i.Horror > 0).Select(i => i.InvestigatorCardComponent))
                allInvestigators.Add(new CardEffect(
                    card: investigator,
                    effect: () => HealingEffect(investigator.SanityToken),
                    animationEffect: ((CardInvestigator)investigator.CardLogic).ThankYouAnimation,
                    type: EffectType.Choose,
                    name: "Curar horror a " + investigator.Info.Name,
                    investigatorImageCardInfoOwner: investigator.Owner));
            yield return new ChooseCardAction(allInvestigators, cancelableCardEffect: healHorror).RunNow();
        }

        IEnumerator HealingEffect(TokenComponent token) => new AddTokenAction(token, -1).RunNow();

        protected virtual IEnumerator PayCostEffect() => new AddTokenAction(ThisCard.ResourcesToken, -1).RunNow();

        protected virtual IEnumerator CancelCardEffect()
        {
            if (!ThisCard.IsInPlay) yield return new MoveCardAction(ThisCard, ThisCard.VisualOwner.Assets).RunNow();
            yield return new AddTokenAction(ThisCard.ResourcesToken, 1).RunNow();
        }

        protected override IEnumerator PlayCardFromHand()
        {
            yield return base.PlayCardFromHand();
            yield return new AddTokenAction(ThisCard.ResourcesToken, 3).RunNow();
        }
    }
}