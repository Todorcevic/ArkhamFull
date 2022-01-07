using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ArkhamGamePlay
{
    public class FightWithWeaponAction : GameAction
    {
        CardEffect weaponCardEffect;
        readonly Func<CardComponent, IEnumerator> fight;
        InvestigatorComponent Investigator => weaponCardEffect.PlayOwner;
        CardComponent Weapon => weaponCardEffect.Card;

        /*****************************************************************************************/
        public FightWithWeaponAction(ref CardEffect weaponCardEffect, Func<CardComponent, IEnumerator> fight)
        {
            this.weaponCardEffect = weaponCardEffect;
            this.fight = fight;
        }

        protected override IEnumerator ActionLogic()
        {
            List<CardEffect> enemiesToChoose = new List<CardEffect>();
            foreach (CardComponent enemy in Investigator.AllEnemiesInMyLocation)
                enemiesToChoose.Add(new CardEffect(
                    card: enemy,
                    effect: () => fight(enemy),
                    animationEffect: ((CardEnemy)enemy.CardLogic).BadEffectForEnemyAnimation,
                    type: EffectType.Choose,
                    name: "Atacar con " + Weapon.Info.Name + " a " + enemy.Info.Name + ((CardEnemy)enemy.CardLogic).CheckDangerAttackName(),
                    investigatorImageCardInfoOwner: Investigator)
                    );

            if (enemiesToChoose.Count == 1 && ((CardEnemy)enemiesToChoose[0].Card.CardLogic).CheckDangerAttackName() == string.Empty)
                yield return new EffectAction(() => fight(enemiesToChoose[0].Card), ((CardEnemy)enemiesToChoose[0].Card.CardLogic).BadEffectForEnemyAnimation).RunNow();
            else yield return new ChooseCardAction(enemiesToChoose, cancelableCardEffect: weaponCardEffect).RunNow();
        }
    }
}