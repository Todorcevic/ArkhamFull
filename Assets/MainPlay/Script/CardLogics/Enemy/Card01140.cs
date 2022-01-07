using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01140 : CardEnemy
    {
        CardComponent North => GameControl.GetCard("01134");
        public override List<CardComponent> SpawnLocation => new List<CardComponent>() { North };

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckToParley(investigatorTurn))
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: Parley,
                    animationEffect: ParleyAnimation,
                    type: EffectType.Activate | EffectType.Parley,
                    name: "Convencer a " + ThisCard.Info.Name,
                    actionCost: 1,
                    resourceCost: 5));
        }

        bool CheckToParley(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.IsInPlay) return false;
            if (((CardEnemy)ThisCard.CardLogic).CurrentLocation != investigatorTurn.ActiveInvestigator.CurrentLocation) return false;
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
    }
}
