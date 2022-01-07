using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class RevealChaosTokenAction : GameAction
    {
        public ChaosTokenComponent Token { get; set; }
        public SkillTest SkillTest { get; set; }

        /*****************************************************************************************/
        public RevealChaosTokenAction(ref SkillTest skillTest, ChaosTokenComponent token = null)
        {
            Token = token ?? AllComponents.ChaosBag.RandomChaosToken();
            SkillTest = skillTest;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            SkillTest.TokenThrow = Token;
            yield return AllComponents.ChaosBag.DropToken(Token);
            yield return new WaitUntil(Token.RigidBody.IsSleeping);
        }
    }
}