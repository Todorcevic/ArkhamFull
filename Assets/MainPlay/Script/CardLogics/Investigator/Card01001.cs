using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01001 : CardInvestigator
    {
        bool effectUsed;

        /*****************************************************************************************/
        public override int ChaosTokenWinValue() => GameControl.ActiveInvestigator.CurrentLocation?.CluesToken.Amount ?? 0;
        public override IEnumerator ChaosTokenWinEffect() => null;

        /*****************************************************************************************/
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is UpkeepPhase) effectUsed = false; ;
            if (gameAction is DefeatCardAction die && OnDefeatEnemy(die))
                die.ChooseCardOptionalAction.ChosenCardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: TakeClue,
                    animationEffect: TakeClueAnimation,
                    checkFilterToCancel: CheckFilterToCancel,
                    type: EffectType.Reaction,
                    name: "Obtener pista",
                    investigatorImageCardInfoOwner: ThisCard.Owner));
        }

        bool OnDefeatEnemy(DefeatCardAction dieAction)
        {
            if (dieAction.CardToDefeat.CardType != CardType.Enemy) return false;
            if (GameControl.ActiveInvestigator != ThisCard.Owner) return false;
            if (ThisCard.Owner.CurrentLocation.CluesToken.Amount < 1) return false;
            if (effectUsed) return false;
            return true;
        }

        IEnumerator TakeClueAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect4).RunNow();

        IEnumerator TakeClue()
        {
            yield return new DiscoverCluesAction(ThisCard.Owner, 1).RunNow();
            effectUsed = true;
        }

        bool CheckFilterToCancel() => ThisCard.Owner.CurrentLocation.CluesToken.Amount < 1;
    }
}