using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01167 : CardTreachery
    {
        SkillTest skillTest;
        List<CardComponent> AssetsChoosables => GameControl.ActiveInvestigator.ListCardComponent.FindAll(c => c.CardType == CardType.Asset && c.IsInPlay && c.CanBeDiscard);

        /*****************************************************************************************/
        public override IEnumerator Revelation()
        {
            skillTest = new SkillTest
            {
                Title = "Soportando " + ThisCard.Info.Name,
                SkillType = Skill.Willpower,
                CardToTest = ThisCard,
                TestValue = 4
            };
            skillTest.LoseEffect.Add(new CardEffect(
                card: ThisCard,
                effect: LoseEffect,
                animationEffect: LoseEffectAnimation,
                type: EffectType.Choose,
                name: "Efecto de " + ThisCard.Info.Name));
            yield return new SkillTestAction(skillTest).RunNow();
        }

        IEnumerator LoseEffectAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator LoseEffect()
        {
            List<CardEffect> listAssets = new List<CardEffect>();
            foreach (CardComponent asset in AssetsChoosables)
            {
                listAssets.Add(new CardEffect(
                    card: asset,
                    effect: DiscardAsset,
                    type: EffectType.Choose,
                    name: "Descartar " + asset.Info.Name));

                IEnumerator DiscardAsset() => new DiscardAction(asset).RunNow();

            }

            if (listAssets.Count > 0) yield return new ChooseCardAction(listAssets, isOptionalChoice: false).RunNow();
            else yield return new AssignDamageHorror(GameControl.ActiveInvestigator, damageAmount: 2).RunNow();
        }
    }
}