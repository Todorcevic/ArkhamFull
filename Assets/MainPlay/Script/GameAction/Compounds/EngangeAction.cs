using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class EngangeAction : GameAction
    {
        readonly CardComponent enemy;
        readonly InvestigatorComponent investigator;

        /*****************************************************************************************/
        public EngangeAction(CardComponent enemy, InvestigatorComponent investigator)
        {
            this.enemy = enemy;
            this.investigator = investigator;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic() =>
            new MoveCardAction(enemy, investigator.Threat, withPreview: false).RunNow();

        protected override IEnumerator Animation()
        {
            yield return new WaitWhile(() => DOTween.IsTweening("Exhausting"));
            yield return new AnimationCardAction(enemy, withReturn: false, audioClip: enemy.Effect5).RunNow();
        }

    }
}