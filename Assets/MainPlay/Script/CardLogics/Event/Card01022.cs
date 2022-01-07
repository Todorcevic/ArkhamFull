using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01022 : CardEvent
    {
        protected override string nameCardEffect => "Obtener pista";
        protected override bool IsFast => true;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction) { }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is DefeatCardAction die && PlayCard(die))
                die.ChooseCardOptionalAction.ChosenCardEffects.Add(playFromHand = PlayFromHand);
        }

        bool PlayCard(DefeatCardAction dieAction)
        {
            if (!CheckPlayedFromHand()) return false;
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (dieAction.CardToDefeat.CardType != CardType.Enemy) return false;
            if (ThisCard.Owner.CurrentLocation.CluesToken.Amount < 1) return false;
            return true;
        }

        protected override IEnumerator LogicEffect() => new DiscoverCluesAction(ThisCard.VisualOwner, 1).RunNow();

        protected override bool CheckFilterToCancel() => ThisCard.Owner.CurrentLocation.CluesToken.Amount < 1;
    }
}