using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class CardToken : CardLogic
    {
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InvestigatorTurn investigatorTurn)
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: GiveResource,
                    type: EffectType.Resource,
                    name: "Obtener recursos",
                    actionCost: 1)
                    );
        }

        IEnumerator GiveResource()
        {
            yield return new SelectInvestigatorAction(GameControl.ActiveInvestigator).RunNow();
            ThisCard.gameObject.SetActive(false);
            yield return new AddTokenAction(GameControl.ActiveInvestigator.InvestigatorCardComponent.ResourcesToken, 1).RunNow();
            ThisCard.gameObject.SetActive(true);
        }

        /********************************** Full Scope Effects**************************************/
        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is MoveCardAction moveCard && moveCard.ThisCard.CardLogic is IRevelation && CheckInHand(moveCard))
            {
                new ShowCardAction(moveCard.ThisCard).RunNow();
                new RevelationAction(moveCard.ThisCard).AddActionTo();
            }
        }

        bool CheckInHand(MoveCardAction moveCard)
        {
            if (!GameControl.GameIsStarted) return false;
            if (moveCard.ThisCard.CurrentZone.ZoneType != Zones.Hand) return false;
            return true;
        }
    }
}