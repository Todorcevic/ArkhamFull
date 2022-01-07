using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01039 : CardSkill
    {
        bool isActive;
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is SkillTestActionComplete skillTestComplete && CheckSkillTestWin(skillTestComplete))
                isActive = true;
            if (gameAction is DiscoverCluesAction discoverClues && isActive)
                new EffectAction(() => DiscoverClue(discoverClues), DiscaoverClueAnimation).AddActionTo();
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is SkillTestActionComplete && isActive) isActive = false;
        }

        protected override bool CheckSkillTestWin(SkillTestActionComplete skillTestComplete)
        {
            if (!skillTestComplete.SkillTest.Modifiers.Contains(ThisCard)) return false;
            if (skillTestComplete.SkillTest.SkillTestType != SkillTestType.Investigate) return false;
            if (!skillTestComplete.SkillTest.IsWin) return false;
            return true;
        }

        IEnumerator DiscaoverClueAnimation() =>
            new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();


        IEnumerator DiscoverClue(DiscoverCluesAction discoverClues)
        {
            discoverClues.Amount++;
            yield return null;
        }
    }
}