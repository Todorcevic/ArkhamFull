using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01124 : CardLocation
    {
        bool effectUsed;
        CardComponent GhoulPriest = GameControl.GetCard("01116");
        public override LocationSymbol MySymbol => LocationSymbol.Bar;
        public override LocationSymbol MovePosibilities => LocationSymbol.Circle;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is SpawnEnemyAction spawnEnemyAction && CheckGhoulPriest(spawnEnemyAction))
                spawnEnemyAction.SpawnSite = ThisCard;
            if (gameAction is InvestigatorTurn investigatorTurn && CheckIfUsed(investigatorTurn))
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => TakeCardAndResource(investigatorTurn),
                    animationEffect: TakeCardAndResourceAnimation,
                    type: EffectType.Activate,
                    name: "Robar carta y recurso",
                    actionCost: 1,
                    investigatorImageCardInfoOwner: investigatorTurn.ActiveInvestigator));
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is EndInvestigatorTurnAction) effectUsed = false;
        }

        bool CheckGhoulPriest(SpawnEnemyAction spawnEnemyAction)
        {
            if (!ThisCard.IsInPlay) return false;
            if (spawnEnemyAction.Enemy != GhoulPriest) return false;
            return true;
        }

        bool CheckIfUsed(InvestigatorTurn investigatorTurn)
        {
            if (effectUsed) return false;
            if (investigatorTurn.ActiveInvestigator.CurrentLocation != ThisCard) return false;
            return true;
        }

        IEnumerator TakeCardAndResourceAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator TakeCardAndResource(InvestigatorTurn investigatorTurn)
        {
            effectUsed = true;
            yield return new DrawAction(investigatorTurn.ActiveInvestigator).RunNow();
            yield return new AddTokenAction(investigatorTurn.ActiveInvestigator.InvestigatorCardComponent.ResourcesToken, 1).RunNow();
        }
    }
}
