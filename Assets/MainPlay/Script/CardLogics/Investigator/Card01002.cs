using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01002 : CardInvestigator, IBuffable
    {
        bool effectUsed;
        List<CardEffect> filterCardsEffect = new List<CardEffect>();
        List<CardComponent> AllCardsTome => ThisCard.Owner.ListCardComponent.FindAll(c => c.KeyWords.Contains("Tome") && c.IsInPlay);
        public override int ChaosTokenWinValue() => 0;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is UpdateActionsLeft updateAction && LosingExtraAction(updateAction))
                effectUsed = true; ;
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is UpkeepPhase) effectUsed = false;
        }

        bool LosingExtraAction(UpdateActionsLeft updateAction)
        {
            if (updateAction.Investigator != ThisCard.Owner) return false;
            if (ThisCard.Owner.ActionsLeft + updateAction.Amount >= 0) return false;
            if (effectUsed) return false;
            return true;
        }

        public override IEnumerator ChaosTokenWinEffect()
        {
            CardEffect cardEffect = new CardEffect(
                card: ThisCard,
                effect: DrawCards,
                animationEffect: DrawCardAnimation,
                type: EffectType.Choose,
                name: "Robar cartas",
                investigatorImageCardInfoOwner: ThisCard.Owner);
            if (AllCardsTome.Count > 0) GameControl.CurrentSkillTestAction.SkillTest.WinEffect.Add(cardEffect);

            IEnumerator DrawCardAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect4).RunNow();

            IEnumerator DrawCards()
            {
                for (int i = 0; i < AllCardsTome.Count; i++)
                    yield return new DrawAction(ThisCard.Owner).RunNow();
            }
            yield return null;
        }

        bool IBuffable.ActiveBuffCondition(GameAction gameAction)
        {
            if (!(gameAction is CardEffectsFilterAction cardEffectFilter)) return false;
            if (GameControl.TurnInvestigator != ThisCard.Owner) return false;
            if (effectUsed) return false;
            if (!(GameControl.CurrentInteractableAction is InvestigatorTurn)) return false;
            filterCardsEffect = cardEffectFilter.CardEffects.FindAll(c => c.Card.KeyWords.Contains("Tome") && c.Card.IsInPlay && c.Type.HasFlag(EffectType.Activate));
            return true;
        }

        void IBuffable.BuffEffect()
        {
            foreach (CardEffect cardEffect in filterCardsEffect)
            {
                cardEffect.Effect = new ActionsTools().JoinEffects(cardEffect.Effect, () => { effectUsed = !cardEffect.IsCancel; return null; });
                cardEffect.ActionCost--;
                cardEffect.Card.CardTools.ShowBuff(ThisCard);
            }
        }

        void IBuffable.DeBuffEffect()
        {
            foreach (CardEffect cardEffect in filterCardsEffect)
                cardEffect.Card.CardTools.HideBuff(ThisCard.UniqueId.ToString());
        }
    }
}