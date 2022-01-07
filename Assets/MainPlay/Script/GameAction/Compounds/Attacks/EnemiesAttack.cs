using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class EnemiesAttack : GameAction
    {
        readonly bool isOportunityAttack;
        readonly List<CardEffect> cardEffects = new List<CardEffect>();
        public InvestigatorComponent Investigator { get; set; }
        public List<CardComponent> Enemys { get; set; }
        public override string GameActionInfo => "Ataque de los Enemigos sobre " + Investigator.InvestigatorCardComponent.Info.Name + ".";

        /*****************************************************************************************/
        public EnemiesAttack(InvestigatorComponent investigator, List<CardComponent> enemys = null, bool isOportunityAttack = false)
        {
            Investigator = investigator;
            Enemys = enemys ?? Investigator.Threat.ListCards.FindAll(c => c.CardType == CardType.Enemy && !c.IsExausted);
            this.isOportunityAttack = isOportunityAttack;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (Investigator.IsDefeat) yield break;
            if (Enemys.Count > 0)
            {
                foreach (CardComponent enemy in Enemys)
                {
                    cardEffects.Add(new CardEffect(
                        card: enemy,
                        effect: EnemyAttack,
                        animationEffect: () => null,
                        type: EffectType.Choose,
                        name: "Recibir ataque de " + enemy.Info.Name,
                        investigatorImageCardInfoOwner: Investigator));

                    IEnumerator EnemyAttack()
                    {
                        yield return new SettingEnemyAttackAction(enemy, GameControl.ActiveInvestigator).RunNow();
                        if (!isOportunityAttack) yield return new ExaustCardAction(enemy, true).RunNow();
                        Enemys.Remove(enemy);
                    }
                }
                yield return new SelectInvestigatorAction(Investigator).RunNow();
                yield return new ChooseCardAction(cardEffects, isOptionalChoice: false, withPreview: cardEffects.Count > 1, isFastAction: !isOportunityAttack).RunNow();
                yield return new EnemiesAttack(Investigator, Enemys, isOportunityAttack).RunNow();
            }
        }
    }
}