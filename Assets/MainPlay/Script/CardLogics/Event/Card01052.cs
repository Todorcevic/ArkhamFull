using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01052 : CardEvent
    {
        protected override bool CheckPlayedFromHand()
        {
            if (!ThisCard.VisualOwner.AllEnemiesInMyLocation.Exists(c => c.IsExausted)) return false;
            return base.CheckPlayedFromHand();
        }

        protected override IEnumerator LogicEffect()
        {
            List<CardEffect> listCardEffects = new List<CardEffect>();
            foreach (CardComponent enemy in ThisCard.VisualOwner.AllEnemiesInMyLocation.FindAll(c => c.IsExausted))
                listCardEffects.Add(new CardEffect(
                    card: enemy,
                    effect: () => new DamageEnemyAction(enemy, 2).RunNow(),
                    animationEffect: ((CardEnemy)enemy.CardLogic).BadEffectForEnemyAnimation,
                    type: EffectType.Choose,
                    name: "Inflingir daño a " + enemy.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));

            if (listCardEffects.Count == 1)
                yield return new EffectAction(new DamageEnemyAction(listCardEffects[0].Card, 2).RunNow, ((CardEnemy)listCardEffects[0].Card.CardLogic).BadEffectForEnemyAnimation).RunNow();
            else yield return new ChooseCardAction(listCardEffects, cancelableCardEffect: playFromHand).RunNow();
        }
    }
}