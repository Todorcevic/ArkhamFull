using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class CheckCancelSkillTest : GameAction
    {
        readonly CardEffect cardEffect;
        bool CheckCancelTest => GameControl.CurrentSkillTestAction != null
            && GameControl.CurrentSkillTestAction.SkillTest.IsOptional
            && (!GameControl.CurrentSkillTestAction.SkillTest.IsComplete ?? false)
            && !(EffectType.Modifier | EffectType.Choose).HasFlag(cardEffect.Type)
            && !cardEffect.IsCancel;

        public bool CanCancel { get; private set; }

        /*****************************************************************************************/
        public CheckCancelSkillTest(CardEffect cardEffect) => this.cardEffect = cardEffect;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (CheckCancelTest)
            {
                CanCancel = true;
                cardEffect.Effect = new ActionsTools().JoinEffects(CancelSkillTest, cardEffect.Effect);
            }
            yield return null;
        }

        IEnumerator CancelSkillTest()
        {
            GameControl.CurrentSkillTestAction.SkillTest.IsOptional = cardEffect.IsCancel;
            yield return null;
        }
    }
}