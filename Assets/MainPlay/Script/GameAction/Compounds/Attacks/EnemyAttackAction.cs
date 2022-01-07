using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class EnemyAttackAction : GameAction
    {
        public InvestigatorComponent Investigator { get; }
        public CardComponent Enemy { get; }
        public int EnemyDamage { get; set; }
        public int EnemyHorror { get; set; }

        /*****************************************************************************************/
        public EnemyAttackAction(InvestigatorComponent investigator, CardComponent enemy)
        {
            Investigator = investigator;
            Enemy = enemy;
            EnemyDamage = (int)Enemy.Info.Enemy_damage;
            EnemyHorror = (int)Enemy.Info.Enemy_horror;
        }

        protected override IEnumerator ActionLogic()
        {
            yield return new EffectAction(() => (Enemy.CardLogic as ISpecialAttack)?.Attack()).RunNow();
            yield return new AssignDamageHorror(Investigator, EnemyDamage, EnemyHorror).RunNow();
        }

        protected override IEnumerator Animation() => new AnimationCardAction(Enemy, audioClip: Enemy.Effect7).RunNow();
    }
}