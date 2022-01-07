using System.Collections;
using UnityEngine;
using System;

namespace ArkhamGamePlay
{
    public class CardAsset : CardLogic
    {
        int costBonus;
        public int Cost => (ThisCard.Info.Cost ?? 0) + CostBonus;
        public int CostBonus
        {
            get { return costBonus; }
            set
            {
                costBonus = value;
                if (Cost != (ThisCard.Info.Cost ?? 0))
                    ThisCard.CardTools.UpdateResourceCost(Cost);
                else
                    ThisCard.CardTools.HideResourceCost();
            }
        }
        protected virtual bool IsFast => false;
        protected virtual CardEffect PlayFromHand => new CardEffect(
                    card: ThisCard,
                    effect: PlayCardFromHand,
                    animationEffect: PlayCardFromHandAnimation,
                    type: IsFast ? EffectType.Fast : EffectType.Play,
                    name: "Jugar " + ThisCard.Info.Name,
                    actionCost: IsFast ? 0 : 1,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner,
                    resourceCost: Cost
                    );

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (IsFast && gameAction is InteractableAction interactableAction && CheckPlayCardFromHandAsFast(interactableAction))
                interactableAction.CardEffects.Add(PlayFromHand);
            if (!IsFast && gameAction is InvestigatorTurn investigatorTurn && CheckPlayCardFromHand(investigatorTurn))
                investigatorTurn.CardEffects.Add(PlayFromHand);
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is AddTokenAction addToken && CheckDie(addToken))
                new DefeatCardAction(ThisCard).AddActionTo();
        }

        protected virtual bool CheckPlayCardFromHand(InvestigatorTurn investigatorTurn)
        {
            if (ThisCard.CurrentZone != investigatorTurn.ActiveInvestigator.Hand) return false;
            return true;
        }

        protected virtual bool CheckPlayCardFromHandAsFast(InteractableAction interactableAction)
        {
            if (!interactableAction.CanPlayFastAction) return false;
            if (GameControl.TurnInvestigator != ThisCard.VisualOwner) return false;
            if (ThisCard.CurrentZone != interactableAction.ActiveInvestigator.Hand) return false;
            return true;
        }

        protected virtual bool CheckDie(AddTokenAction addTokenAction)
        {
            if (addTokenAction.ToToken != ThisCard.HealthToken && addTokenAction.ToToken != ThisCard.SanityToken
                && addTokenAction.SecundaryToToken != ThisCard.HealthToken && addTokenAction.SecundaryToToken != ThisCard.SanityToken) return false;
            if (!CheckDieByHealth() && !CheckDieBySanity()) return false;
            return true;
        }

        bool CheckDieByHealth()
        {
            if (ThisCard.Info.Health == null) return false;
            if (ThisCard.HealthToken.Amount < ThisCard.Info.Health) return false;
            return true;
        }

        bool CheckDieBySanity()
        {
            if (ThisCard.Info.Sanity == null) return false;
            if (ThisCard.SanityToken.Amount < ThisCard.Info.Sanity) return false;
            return true;
        }

        protected virtual IEnumerator PlayCardFromHandAnimation() => new AnimationCardAction(ThisCard, withReturn: false, audioClip: ThisCard.ShowCardFront).RunNow();

        protected virtual IEnumerator PlayCardFromHand() => new MoveCardAction(ThisCard, ThisCard.VisualOwner.Assets, withPreview: false).RunNow();
    }
}