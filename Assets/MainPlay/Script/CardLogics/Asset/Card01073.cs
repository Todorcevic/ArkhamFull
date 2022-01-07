using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01073 : CardAsset
    {
        CardEffect cardEffect;
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is SkillTestActionComplete skillTestComplete && ActivateEffect(skillTestComplete))
                skillTestComplete.ChooseCardOptionalAction.ChosenCardEffects.Add(cardEffect = new CardEffect(
                    card: ThisCard,
                    effect: ExhaustAndTakeItem,
                    animationEffect: ExhaustAndTakeItemAnimation,
                    checkFilterToCancel: CheckFilterToCancel,
                    type: EffectType.Reaction,
                    needExhaust: true,
                    name: "Robar carta",
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
        }

        bool ActivateEffect(SkillTestActionComplete skillTestComplete)
        {
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.VisualOwner != GameControl.ActiveInvestigator) return false;
            if (!skillTestComplete.SkillTest.IsWin) return false;
            if (skillTestComplete.SkillTest.SkillTestType != SkillTestType.Investigate) return false;
            if (skillTestComplete.SkillTest.TotalInvestigatorValue - skillTestComplete.SkillTest.TotalTestValue < 2) return false;
            if (!ThisCard.VisualOwner.InvestigatorDiscard.ListCards.Exists(c => c.KeyWords.Contains("Item"))) return false;
            return true;
        }

        IEnumerator ExhaustAndTakeItemAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator ExhaustAndTakeItem()
        {
            List<CardEffect> allItems = new List<CardEffect>();
            foreach (CardComponent item in ThisCard.VisualOwner.InvestigatorDiscard.ListCards.FindAll(c => c.KeyWords.Contains("Item")))
                allItems.Add(new CardEffect(
                    card: item,
                    effect: () => new MoveCardAction(item, ThisCard.VisualOwner.Hand).RunNow(),
                    type: EffectType.Choose,
                    name: "Obtener " + item.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
            yield return new ChooseCardAction(allItems, cancelableCardEffect: cardEffect).RunNow();
        }

        bool CheckFilterToCancel() => !ThisCard.VisualOwner.InvestigatorDiscard.ListCards.Exists(c => c.KeyWords.Contains("Item"));
    }
}