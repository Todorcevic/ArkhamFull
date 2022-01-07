using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public abstract class CardLocation : CardLogic
    {
        int shroudBonus;
        CardEffect investigateCardEffect;
        public int Shroud => (int)ThisCard.Info.Shroud + ShroudBonus;
        public int ShroudBonus
        {
            get { return shroudBonus; }
            set
            {
                shroudBonus = value;
                ThisCard.CardTools.UpdateShroud(Shroud);
            }
        }
        public int Clues
        {
            get
            {
                int multiplier = (bool)ThisCard.Info.Clues_fixed ? 1 : GameControl.InvestigatorsStartingAmount;
                return (int)ThisCard.Info.Clues * multiplier;
            }
        }
        public bool IsEmpty => !ThisCard.MyOwnZone.ListCards.Exists(c => c.CardType == CardType.PlayCard || c.CardType == CardType.Enemy);
        public bool IsRevealed { get; set; }
        public abstract LocationSymbol MySymbol { get; }
        public abstract LocationSymbol MovePosibilities { get; }

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InvestigatorTurn investigatorTurn)
            {
                if (CanBeInvestigated(investigatorTurn)) investigatorTurn.CardEffects.Add(investigateCardEffect = new CardEffect(
                    card: ThisCard,
                    effect: Investigate,
                    type: EffectType.Investigate,
                    name: "Investigar " + ThisCard.Info.Name,
                    actionCost: 1));
                if (CanMoveToThis(investigatorTurn.ActiveInvestigator)) investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => MoveToLocation(investigatorTurn.ActiveInvestigator),
                    type: EffectType.Move,
                    name: "Moverse a " + ThisCard.Info.Name,
                    actionCost: 1));
            }
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is MoveCardAction moveCardAction && CheckRevealLocation(moveCardAction))
                new RevealLocationAction(moveCardAction.Zone.ThisCard).AddActionTo();
        }

        bool CheckRevealLocation(MoveCardAction moveCardAction)
        {
            if (IsRevealed) return false;
            if (moveCardAction.ThisCard.CardType != CardType.PlayCard) return false;
            if (moveCardAction.Zone != ThisCard.MyOwnZone) return false;
            return true;
        }

        bool CanBeInvestigated(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.MyOwnZone.ListCards.Contains(investigatorTurn.ActiveInvestigator.PlayCard)) return false;
            return true;
        }

        public virtual bool CanMoveToThis(InvestigatorComponent investigator)
        {
            if (!((CardLocation)investigator.CurrentLocation.CardLogic)
                .MovePosibilities.HasFlag(MySymbol)) return false;
            return true;
        }

        IEnumerator Investigate()
        {
            InvestigateLocation investigateAction = new InvestigateLocation(ThisCard, investigateCardEffect.IsCancelable);
            yield return investigateAction.RunNow();
            investigateCardEffect.IsCancel = !investigateAction.SkillTest.IsComplete ?? false;
        }

        IEnumerator MoveToLocation(InvestigatorComponent investigator) =>
            new MoveCardAction(investigator.PlayCard, ThisCard.MyOwnZone, withPreview: false).RunNow();
    }
}