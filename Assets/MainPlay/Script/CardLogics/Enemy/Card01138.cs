using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01138 : CardEnemy
    {
        CardEffect cardEffect;
        List<CardComponent> cardsDiscarted = new List<CardComponent>();
        CardComponent Graveyard => GameControl.GetCard("01133");
        public override List<CardComponent> SpawnLocation => new List<CardComponent>() { Graveyard };

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckToParley(investigatorTurn))
                investigatorTurn.CardEffects.Add(cardEffect = new CardEffect(
                    card: ThisCard,
                    effect: Parley,
                    animationEffect: ParleyAnimation,
                    payEffect: () => PayEffect(investigatorTurn),
                    cancelEffect: () => CancelEffect(investigatorTurn),
                    type: EffectType.Activate | EffectType.Parley,
                    name: "Convencer a " + ThisCard.Info.Name,
                    actionCost: 1));
        }

        bool CheckToParley(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.IsInPlay) return false;
            if (((CardEnemy)ThisCard.CardLogic).CurrentLocation != investigatorTurn.ActiveInvestigator.CurrentLocation) return false;
            if (investigatorTurn.ActiveInvestigator.Hand.ListCards.FindAll(c => !c.IsWeakness).Count < 4) return false;
            return true;
        }

        IEnumerator ParleyAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1, withReturn: false).RunNow();

        IEnumerator Parley()
        {
            yield return new DiscardAction(ThisCard, victoryZone: true, withTopUp: false).RunNow();
            yield return new AddVictoryCardAction(
                ThisCard,
                ThisCard.Info.Name + " otorga " + ThisCard.VictoryPoint + " puntos de experiencia a cada investigador.",
                ThisCard.VictoryPoint).RunNow();
        }

        IEnumerator PayEffect(InvestigatorTurn investigatorTurn)
        {
            for (int i = 0; i < 4; i++)
            {
                InvestigatorDiscardHand investigatorDiscard = new InvestigatorDiscardHand(investigatorTurn.ActiveInvestigator, isOptional: true);
                yield return investigatorDiscard.RunNow();
                if (investigatorDiscard.IsCanceled)
                {
                    cardEffect.IsCancel = true;
                    break;
                }
                cardsDiscarted.Add(investigatorDiscard.CardDiscarted);
            }
        }

        IEnumerator CancelEffect(InvestigatorTurn investigatorTurn)
        {
            foreach (CardComponent card in cardsDiscarted)
                yield return new MoveCardAction(card, investigatorTurn.ActiveInvestigator.Hand, withPreview: false, isFast: true).RunNow();
        }
    }
}
