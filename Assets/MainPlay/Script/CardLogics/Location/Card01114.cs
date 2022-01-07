using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01114 : CardLocation
    {
        public override LocationSymbol MySymbol => LocationSymbol.Plus;
        public override LocationSymbol MovePosibilities => LocationSymbol.Square;

        /*****************************************************************************************/
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is MoveCardAction moveCardAction && MoveIntoLocation(moveCardAction))
                new EffectAction(() => TakeDamage(moveCardAction.ThisCard.Owner), TakeDamageAnimation).AddActionTo();
        }

        bool MoveIntoLocation(MoveCardAction moveCardAction)
        {
            if (moveCardAction.ThisCard.CardType != CardType.PlayCard) return false;
            if (moveCardAction.Zone != ThisCard.MyOwnZone) return false;
            return true;
        }

        IEnumerator TakeDamageAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator TakeDamage(InvestigatorComponent investigator) => new AssignDamageHorror(investigator, damageAmount: 1).RunNow();
    }
}