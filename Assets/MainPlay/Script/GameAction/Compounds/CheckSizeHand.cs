using System.Collections;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class CheckSizeHand : GameAction
    {
        readonly InvestigatorComponent investigator;
        public override string GameActionInfo => "Comprobación del tamaño máximo de cartas en la mano de " + investigator.Name + ".";
        public override GameActionType GameActionType => GameActionType.Compound;

        /*****************************************************************************************/
        public CheckSizeHand(InvestigatorComponent investigator) => this.investigator = investigator;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (investigator.IsDefeat) yield break;
            if (investigator.Hand.ListCards.Count > GameData.MAX_SIZE_HAND)
            {
                yield return new InvestigatorDiscardHand(investigator).RunNow();
                yield return new CheckSizeHand(investigator).RunNow();
            }
        }
    }
}