using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class SetInvestigator : GameAction
    {
        readonly InvestigatorComponent investigator;
        public override string GameActionInfo => "Preparando a " + investigator.InvestigatorCardComponent.Info.Real_name;
        public override GameActionType GameActionType => GameActionType.Setting;

        /*****************************************************************************************/
        public SetInvestigator(InvestigatorComponent investigator) => this.investigator = investigator;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            yield return new MoveCardAction(investigator.InvestigatorCardComponent, investigator.InvestigatorZone).RunNow();
            yield return new AddTokenAction(investigator.InvestigatorCardComponent.HealthToken, investigator.PhysicalTraumas).RunNow();
            yield return new AddTokenAction(investigator.InvestigatorCardComponent.SanityToken, investigator.MentalTraumas).RunNow();
            yield return new AddTokenAction(investigator.InvestigatorCardComponent.ResourcesToken, GameData.INITIAL_RESOURCES).RunNow();
            yield return new UpdateActionsLeft(investigator, investigator.InitialActions).RunNow();
        }
    }
}