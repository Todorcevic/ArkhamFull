using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class CardScenario : CardLogic
    {
        protected bool IsHardDifficulty => ThisCard.IsBack;
        protected SkillTest SkillTest => SkillTestAction.SkillTest;
        protected SkillTestAction SkillTestAction => GameControl.CurrentSkillTestAction;

        /*****************************************************************************************/
        public virtual int SkullTokenValue() => 0;
        public virtual int CultistTokenValue() => 0;
        public virtual int TabletTokenValue() => 0;
        public virtual int KthuluTokenValue() => 0;
        public virtual int WinTokenValue() =>
            ((CardInvestigator)GameControl.ActiveInvestigator.InvestigatorCardComponent.CardLogic).ChaosTokenWinValue();
        public virtual int FailTokenValue() => 0;
        public virtual IEnumerator SkullTokenEffect() => null;
        public virtual IEnumerator CultistTokenEffect() => null;
        public virtual IEnumerator TabletTokenEffect() => null;
        public virtual IEnumerator KthuluTokenEffect() => null;
        public virtual IEnumerator WinTokenEffect() =>
            ((CardInvestigator)GameControl.ActiveInvestigator.InvestigatorCardComponent.CardLogic).ChaosTokenWinEffect();
        public virtual IEnumerator FailTokenEffect()
        {
            SkillTest.AutoResultWinLose = false;
            yield return null;
        }

        public int GetTokenValue(ChaosTokenType tokenType)
        {
            switch (tokenType)
            {
                case ChaosTokenType.Skull: return SkullTokenValue();
                case ChaosTokenType.Cultist: return CultistTokenValue();
                case ChaosTokenType.Tablet: return TabletTokenValue();
                case ChaosTokenType.Kthulu: return KthuluTokenValue();
                case ChaosTokenType.Win: return WinTokenValue();
                case ChaosTokenType.Fail: return FailTokenValue();
                default: throw new System.ArgumentException("Wrong typeToken", tokenType.ToString());
            }
        }

        public Effect GetTokenEffect(ChaosTokenType tokenType)
        {
            switch (tokenType)
            {
                case ChaosTokenType.Skull: return SkullTokenEffect;
                case ChaosTokenType.Cultist: return CultistTokenEffect;
                case ChaosTokenType.Tablet: return TabletTokenEffect;
                case ChaosTokenType.Kthulu: return KthuluTokenEffect;
                case ChaosTokenType.Win: return WinTokenEffect;
                case ChaosTokenType.Fail: return FailTokenEffect;
                default: throw new System.ArgumentException("Wrong typeToken", tokenType.ToString());
            }
        }
    }
}