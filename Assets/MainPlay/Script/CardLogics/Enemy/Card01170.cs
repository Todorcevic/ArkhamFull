using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01170 : CardEnemy
    {
        public override bool IsHunter => true;
        public override bool IsRetaliate => true;
        public override List<CardComponent> SpawnLocation =>
            GameControl.AllCardComponents.FindAll(c => c.IsInPlay && c.CardLogic is CardLocation location && location.IsEmpty);

        /*****************************************************************************************/
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is MythosPhase && CheckIsInPlay())
                new EffectAction(AddingDoom, AddingDoomAnimation).AddActionTo();
        }

        bool CheckIsInPlay()
        {
            if (!ThisCard.IsInPlay) return false;
            return true;
        }

        IEnumerator AddingDoomAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();
        IEnumerator AddingDoom() => new AddTokenAction(ThisCard.DoomToken, 1).RunNow();
    }
}
