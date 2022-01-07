using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01107 : CardAgenda
    {
        CardComponent Parlor => GameControl.GetCard("01115");
        CardComponent Hallway => GameControl.GetCard("01112");
        List<CardComponent> ListGhoulsToMove => GameControl.AllCardComponents
            .FindAll(c => c.CardLogic is CardEnemy cardEnemy && c.KeyWords.Contains("Ghoul") && !cardEnemy.IsEnganged && c.IsInPlay);
        List<CardComponent> ListGhoulsToDoom => GameControl.AllCardComponents
            .FindAll(c => c.CardLogic is CardEnemy cardEnemy && c.KeyWords.Contains("Ghoul") && (cardEnemy.CurrentLocation == Parlor || cardEnemy.CurrentLocation == Hallway));
        /*****************************************************************************************/
        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is PhaseAction phase) CheckCondition(phase);
        }

        void CheckCondition(PhaseAction phase)
        {
            if (GameControl.CurrentAgenda != this) return;
            if (phase is EnemyPhase) new EffectAction(MoveEnemies, MoveEnemiesAnimation).AddActionTo();
            else if (phase is UpkeepPhase) new EffectAction(TakingDoom, TakingDoomAnimation).AddActionTo();
        }

        IEnumerator MoveEnemiesAnimation()
        {
            if (ListGhoulsToMove.Count > 0)
                yield return new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();
        }

        IEnumerator MoveEnemies()
        {
            foreach (CardComponent enemy in ListGhoulsToMove)
                yield return new MoveHunterEnemy(enemy, Parlor).RunNow();
        }

        IEnumerator TakingDoomAnimation()
        {
            if (ListGhoulsToDoom.Count > 0)
                yield return new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect2).RunNow();
        }

        IEnumerator TakingDoom() => new AddTokenAction(ThisCard.DoomToken, ListGhoulsToDoom.Count).RunNow();

        public override IEnumerator BackFace()
        {
            if (GameControl.CurrentAct is Card01110)
                foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame.ToArray())
                {
                    yield return new InvestigatorTraumaAction(investigator, investigator.Name + " quedó derrotado y sufre un trauma físico.", isPhysical: true).RunNow();
                    yield return new InvestigatorEliminatedAction(investigator).RunNow();
                }
            else yield return new FinishGameAction(new CoreScenario1().R3).RunNow();
        }
    }
}