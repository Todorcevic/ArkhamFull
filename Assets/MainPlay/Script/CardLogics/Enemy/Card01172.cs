using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01172 : CardEnemy
    {
        SkillTestAction skillTestAction;
        ChaosTokenComponent chaosToken;
        int? oldValue;

        public override bool IsHunter => true;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);

            if (gameAction is SkillTestAction skillTestAction && CheckSkillTest(skillTestAction))
                this.skillTestAction = skillTestAction;

            if (gameAction is SkillTestChaosTokenEffect skillTestChaosTokenEffect && CheckToken(skillTestChaosTokenEffect))
                new EffectAction(() => ModifierChaosTokenValueBy2(skillTestChaosTokenEffect.ChaosToken), ModifierChaosTokenValueBy2Animator).AddActionTo();
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);

            if (gameAction is SkillTestActionComplete skillTestComplete && CheckResetChaosToken(skillTestComplete))
            {
                chaosToken.Value = chaosToken.Type == ChaosTokenType.Basic ? oldValue : null; //Autorefresh
                chaosToken = null;
            }
        }

        bool CheckSkillTest(SkillTestAction skillTestAction)
        {
            if (skillTestAction.SkillTest.CardToTest != ThisCard) return false;
            if (skillTestAction.SkillTest.SkillTestType != SkillTestType.Evade) return false;
            return true;
        }

        bool CheckToken(SkillTestChaosTokenEffect skillTestChaosTokenEffect)
        {
            if (skillTestAction != GameControl.CurrentSkillTestAction) return false;
            if (skillTestChaosTokenEffect.ChaosToken.Value >= 0) return false;
            return true;
        }

        bool CheckResetChaosToken(SkillTestActionComplete skillTestComplete)
        {
            if (chaosToken == null) return false;
            if (skillTestComplete.SkillTest.TokenThrow != chaosToken) return false;
            return true;
        }

        IEnumerator ModifierChaosTokenValueBy2Animator() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator ModifierChaosTokenValueBy2(ChaosTokenComponent chaosToken)
        {
            this.chaosToken = chaosToken;
            oldValue = this.chaosToken.Value;
            this.chaosToken.Value *= 2;
            AllComponents.PanelSkillTest.AddModifier(ThisCard, 0);
            return null;
        }
    }
}
