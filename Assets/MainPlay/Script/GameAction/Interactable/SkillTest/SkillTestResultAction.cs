using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class SkillTestResultAction : GameAction
    {
        public SkillTest SkillTest { get; set; }
        public SkillTestResultAction(ref SkillTest skillTest) => SkillTest = skillTest;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            SkillTest.IsWin = SkillTest.TotalInvestigatorValue >= SkillTest.TotalTestValue;
            yield return null;
        }
    }
}