using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class DefeatCardAction : GameAction
    {
        public CardComponent CardToDefeat { get; set; }

        /*****************************************************************************************/
        public DefeatCardAction(CardComponent cardToDie) => CardToDefeat = cardToDie;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (CardToDefeat.VictoryPoint > 0)
            {
                yield return new DiscardAction(CardToDefeat, victoryZone: true).RunNow();
                yield return new AddVictoryCardAction(
                    CardToDefeat,
                    CardToDefeat.Info.Name + " otorga " + CardToDefeat.VictoryPoint + " puntos de experiencia a cada investigador.",
                    CardToDefeat.VictoryPoint).RunNow();
            }
            else yield return new DiscardAction(CardToDefeat).RunNow();
        }

        protected override IEnumerator Animation()
        {
            CardToDefeat.CardTools.PlayOneShotSound(CardToDefeat.Effect6);
            yield return new WaitWhile(() => CardToDefeat.CardTools.AudioSource.isPlaying);
        }
    }
}