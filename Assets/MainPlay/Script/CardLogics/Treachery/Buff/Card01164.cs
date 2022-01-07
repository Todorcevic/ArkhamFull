using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01164 : CardTreachery, IBuffable
    {
        bool effectUsed;
        readonly List<CardComponent> cardsAffected = new List<CardComponent>();
        List<CardEffect> filterCardsEffect = new List<CardEffect>();
        List<CardEffect> superCardsEffect = new List<CardEffect>();
        public override bool IsDiscarted => false;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is PlayCardAction playCardAction && CheckAction(playCardAction))
                playCardAction.CancelCardEffect.Name = playCardAction.CardEffect.Name + "<color=#FF2B2B> costará una accion extra. </color>";
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is UpkeepPhase) effectUsed = false;
            if (gameAction is EndInvestigatorTurnAction endInvestigatorTurn && Forced(endInvestigatorTurn))
                new EffectAction(SkillTestToDiscard, DiscardAnimation).AddActionTo();
        }

        bool CheckAction(PlayCardAction playCardAction)
        {
            if (ThisCard.CurrentZone != playCardAction.Investigator.Threat) return false;
            if (effectUsed) return false;
            if (!playCardAction.CardEffect.Type.HasFlag(EffectType.Move)
                && !playCardAction.CardEffect.Type.HasFlag(EffectType.Fight)
                && !playCardAction.CardEffect.Type.HasFlag(EffectType.Evade)) return false;
            return true;
        }

        bool Forced(EndInvestigatorTurnAction endInvestigatorTurn)
        {
            if (ThisCard.CurrentZone != endInvestigatorTurn.Investigator.Threat) return false;
            return true;
        }

        public override IEnumerator Revelation() =>
            new MoveCardAction(ThisCard, GameControl.ActiveInvestigator.Threat, withPreview: false).RunNow();

        IEnumerator DiscardAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator SkillTestToDiscard()
        {
            SkillTest skillTest = new SkillTest
            {
                Title = "Intentando superar " + ThisCard.Info.Name,
                SkillType = Skill.Willpower,
                CardToTest = ThisCard,
                TestValue = 3
            };
            skillTest.WinEffect.Add(new CardEffect(
                card: ThisCard,
                effect: WinEffect,
                type: EffectType.Choose,
                name: "Descartar"));
            yield return new SkillTestAction(skillTest).RunNow();
            IEnumerator WinEffect() => new DiscardAction(ThisCard).RunNow();
        }

        bool IBuffable.ActiveBuffCondition(GameAction gameAction)
        {
            if (!(gameAction is CardEffectsFilterAction filterAction)) return false;
            if (GameControl.TurnInvestigator != ThisCard.VisualOwner) return false;
            if (!ThisCard.IsInPlay) return false;
            if (effectUsed) return false;
            if (!(GameControl.CurrentInteractableAction is InvestigatorTurn)) return false;
            filterCardsEffect = filterAction.CardEffects.FindAll(c => c.Type.HasFlag(EffectType.Move) || c.Type.HasFlag(EffectType.Fight) || c.Type.HasFlag(EffectType.Evade));
            return true;
        }

        void IBuffable.BuffEffect()
        {
            foreach (CardEffect cardEffect in filterCardsEffect)
            {
                cardEffect.ActionCost++;
                cardEffect.Effect = new ActionsTools().JoinEffects(cardEffect.Effect, () => { effectUsed = !cardEffect.IsCancel; return null; });
                superCardsEffect.Add(cardEffect);

                if (!cardsAffected.Contains(cardEffect.Card))
                {
                    cardEffect.Card.CardTools.ShowBuff(ThisCard);
                    cardsAffected.Add(cardEffect.Card);
                }
            }
        }

        void IBuffable.DeBuffEffect()
        {
            foreach (CardComponent card in cardsAffected)
                card.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            cardsAffected.Clear();
        }
    }
}