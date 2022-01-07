using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class SkillTestActionComplete : GameAction
    {
        List<CardEffect> effectsResult = new List<CardEffect>();
        public override GameActionType GameActionType => GameActionType.Compound;
        public SkillTest SkillTest { get; }

        /*****************************************************************************************/
        public SkillTestActionComplete(SkillTest skillTest) => SkillTest = skillTest;

        /*****************************************************************************************/
        protected override sealed IEnumerator ActionLogic()
        {
            yield return AllComponents.ChaosBag.ReturnAllTokens();//PH.8.2
            AllComponents.PanelSkillTest.CleanPanel();
            yield return AllComponents.PanelSkillTest.HideThisPanel();

            if (SkillTest.IsComplete ?? false) effectsResult = SkillTest.IsWin ? SkillTest.WinEffect : SkillTest.LoseEffect; //PH.7
            if (effectsResult.Count > 0)
            {
                UpdatingEffectsResult();
                yield return ChoosingEffects();
            }

            foreach (CardComponent card in SkillTest.Modifiers.OrderBy(c => c.VisualOwner.Name)) //PH.8.1
            {
                if (SkillTest.IsComplete ?? true) yield return new DiscardAction(card, isFast: true).RunNow();
                else yield return new MoveCardAction(card, card.Owner.Hand, withPreview: false, isFast: true).RunNow();
            }
            yield return new WaitWhile(() => DOTween.IsTweening("MoveCard"));
            GameControl.CurrentSkillTestAction = null;
        }

        void UpdatingEffectsResult()
        {
            foreach (CardEffect cardEffect in effectsResult)
                cardEffect.Effect = new ActionsTools().JoinEffects(cardEffect.Effect, () => AddingRemove(cardEffect));

            IEnumerator AddingRemove(CardEffect cardEffect)
            {
                effectsResult.Remove(cardEffect);
                yield return null;
            }
        }

        IEnumerator ChoosingEffects()
        {
            if (effectsResult.Count > 1)
            {
                yield return new ChooseCardAction(effectsResult, isOptionalChoice: false).RunNow();
                yield return ChoosingEffects();
            }
            else yield return new EffectAction(effectsResult[0].Effect, effectsResult[0].AnimationEffect).RunNow();
        }
    }
}