using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace ArkhamGamePlay
{
    public class Card01104 : CardScenario
    {
        bool skullTokenThrowed;
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is SkillTestActionComplete skillTestComplete && CheckSkullTokenHard(skillTestComplete))
                new EffectAction(HardSkullTokenEffect, HardSkullTokenAnimation).AddActionTo();
        }

        bool CheckSkullTokenHard(SkillTestActionComplete skillTestComplete)
        {
            if (!skullTokenThrowed) return false;
            skullTokenThrowed = false;
            if (!skillTestComplete.SkillTest.IsComplete ?? true) return false;
            if (skillTestComplete.SkillTest.IsWin) return false;
            if (GameControl.ActiveInvestigator.IsDefeat) return false;
            return true;
        }

        IEnumerator HardSkullTokenAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect4).RunNow();

        IEnumerator HardSkullTokenEffect()
        {
            List<CardComponent> ghoulListCards = AllComponents.Table.EncounterDeck.ListCards
                .Concat(AllComponents.Table.EncounterDiscard.ListCards).ToList()
                .FindAll(c => c.CardType == CardType.Enemy && c.KeyWords.Contains("Ghoul"));
            List<CardEffect> cardEffects = new List<CardEffect>();
            foreach (CardComponent card in ghoulListCards)
            {
                yield return new TurnCardAction(card, isBack: false).RunNow();
                cardEffects.Add(new CardEffect(
                    card: card,
                    effect: () => DrawGhoul(card),
                    type: EffectType.Choose,
                    name: "Robar " + card.Info.Name));
            }
            yield return new ChooseCardAction(cardEffects, isOptionalChoice: false).RunNow();
            yield return new MoveDeck(ghoulListCards, AllComponents.Table.EncounterDeck, isBack: true, withMoveUp: true).RunNow();
            yield return new ShuffleAction(AllComponents.Table.EncounterDeck).RunNow();

            IEnumerator DrawGhoul(CardComponent card)
            {
                yield return new DrawAction(GameControl.ActiveInvestigator, card, withShow: true).RunNow();
                ghoulListCards.Remove(card);
            }
        }

        public override int SkullTokenValue()
        {
            if (IsHardDifficulty) return -2;
            else return -GameControl.ActiveInvestigator.AllEnemiesInMyLocation.FindAll(c => c.KeyWords.Contains("Ghoul")).Count();
        }

        public override int CultistTokenValue()
        {
            if (IsHardDifficulty) return 0;
            else return -1;
        }

        public override int TabletTokenValue()
        {
            if (IsHardDifficulty) return -4;
            else return -2;
        }

        public override IEnumerator CultistTokenEffect()
        {
            int horrorAmount = 1;
            if (IsHardDifficulty)
            {
                horrorAmount = 2;
                SkillTest skillTest = SkillTest;
                yield return new RevealChaosTokenAction(ref skillTest).RunNow();
                yield return new SkillTestChaosTokenEffect(skillTest.TokenThrow).RunNow();
            }

            SkillTest.LoseEffect.Add(new CardEffect(
                card: ThisCard,
                effect: () => new AssignDamageHorror(GameControl.ActiveInvestigator, horrorAmount: horrorAmount).RunNow(),
                type: EffectType.Choose,
                name: "Recibir horror"));
        }

        public override IEnumerator TabletTokenEffect()
        {
            if (GameControl.ActiveInvestigator.AllEnemiesInMyLocation.Exists(c => c.KeyWords.Contains("Ghoul")))
            {
                yield return new AssignDamageHorror(GameControl.ActiveInvestigator, damageAmount: 1).RunNow();
                if (IsHardDifficulty)
                    yield return new AssignDamageHorror(GameControl.ActiveInvestigator, horrorAmount: 1).RunNow();
            }
        }

        public override IEnumerator SkullTokenEffect()
        {
            if (IsHardDifficulty) skullTokenThrowed = true;
            yield return null;
        }
    }
}