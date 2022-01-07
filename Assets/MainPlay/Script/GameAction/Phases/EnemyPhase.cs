using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class EnemyPhase : PhaseAction
    {
        public override string GameActionInfo => "Fase de Enemigos.";

        /*****************************************************************************************/
        public EnemyPhase() => AllComponents.PhasesUI.SelectPhase(this);
        /*****************************************************************************************/

        protected override IEnumerator PhaseLogic()
        {
            List<CardComponent> allEnemies = GameControl.AllCardComponents.FindAll(c => c.CardType == CardType.Enemy
             && c.IsInPlay && !c.IsExausted && ((CardEnemy)c.CardLogic).IsHunter && !((CardEnemy)c.CardLogic).IsEnganged);

            foreach (CardComponent enemy in allEnemies)
                yield return new MoveHunterEnemy(enemy).RunNow();

            foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame.FindAll(c => c.IsEngangedAndReady))
            {
                yield return new ActiveInvestigatorAction(investigator).RunNow();
                yield return new EnemiesAttack(investigator).RunNow();
                yield return new ActiveInvestigatorAction(null).RunNow();
            }
            yield return new WindowInvestigatorAction().RunNow();
        }
    }
}