using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01070 : CardAsset, IBuffable
    {
        CardEffect cardEffect;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckPlayCardInGame(investigatorTurn))
                investigatorTurn.CardEffects.Add(cardEffect = new CardEffect(
                    card: ThisCard,
                    effect: AddCharge,
                    animationEffect: AddChargetAnimation,
                    type: EffectType.Activate,
                    name: "Activar " + ThisCard.Info.Name,
                    actionCost: 1,
                    needExhaust: true,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
        }

        bool CheckPlayCardInGame(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.VisualOwner != investigatorTurn.ActiveInvestigator) return false;
            if (!ThisCard.VisualOwner.Assets.ListCards.Exists(c => c.CardType == CardType.Asset && c.KeyWords.Contains("Spell"))) return false;
            return true;
        }

        IEnumerator AddChargetAnimation() =>
                new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator AddCharge()
        {
            List<CardEffect> spellAssets = new List<CardEffect>();
            foreach (CardComponent spell in ThisCard.VisualOwner.Assets.ListCards.FindAll(c => c.CardType == CardType.Asset && c.KeyWords.Contains("Spell")))
                spellAssets.Add(new CardEffect(
                    card: spell,
                    effect: () => ChargeEffect(spell),
                    type: EffectType.Choose,
                    name: "Agregar carga a " + spell.Info.Name,
                    investigatorImageCardInfoOwner: spell.VisualOwner));
            yield return new ChooseCardAction(spellAssets, cancelableCardEffect: cardEffect).RunNow();

            IEnumerator ChargeEffect(CardComponent spell) =>
                new AddTokenAction(spell.ResourcesToken, 1).RunNow();
        }

        bool IBuffable.ActiveBuffCondition(GameAction gameAction) => ThisCard.IsInPlay;

        void IBuffable.BuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);
            ThisCard.VisualOwner.Slots.ArcaneSlot++;
        }

        void IBuffable.DeBuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            ThisCard.VisualOwner.Slots.ArcaneSlot--;
        }
    }
}