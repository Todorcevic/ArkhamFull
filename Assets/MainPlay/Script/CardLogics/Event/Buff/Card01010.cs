using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01010 : CardEvent, IBuffable
    {
        protected override bool IsFast => true;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is SettingEnemyAttackAction enemyAttack && EnemiesCantAttack(enemyAttack))
                new EffectAction(() => StopEnemyAttack(enemyAttack)).AddActionTo();
            if (gameAction is EnemiesAttack enemiesAttack && CheckEnemiesAttack(enemiesAttack))
                enemiesAttack.Enemys.RemoveAll(c => !c.KeyWords.Contains("Elite"));
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is ChooseInvestigator chooseInvestigatorAction && CheckPlayedFromHand())
                chooseInvestigatorAction.ChooseCardOptionalAction.ChosenCardEffects.Add(playFromHand = PlayFromHand);
            if (gameAction is UpkeepPhase) BuffActive = false;
        }

        bool EnemiesCantAttack(SettingEnemyAttackAction enemyAttackAction)
        {
            if (!BuffActive) return false;
            if (enemyAttackAction.Investigator != ThisCard.VisualOwner) return false;
            if (enemyAttackAction.Enemy.KeyWords.Contains("Elite")) return false;
            return true;
        }

        bool CheckEnemiesAttack(EnemiesAttack enemiesAttack)
        {
            if (!BuffActive) return false;
            if (enemiesAttack.Investigator != ThisCard.VisualOwner) return false;
            return true;
        }

        protected override bool CheckPlayedFromHand()
        {
            if (!base.CheckPlayedFromHand()) return false;
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            return true;
        }

        IEnumerator StopEnemyAttack(SettingEnemyAttackAction enemyAttack)
        {
            enemyAttack.IsActionCanceled = true;
            yield return null;
        }

        protected override IEnumerator LogicEffect()
        {
            BuffActive = true;
            yield return null;
        }

        bool IBuffable.ActiveBuffCondition(GameAction gameAction) => BuffActive;

        void IBuffable.BuffEffect() => ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);

        void IBuffable.DeBuffEffect() => ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
    }
}