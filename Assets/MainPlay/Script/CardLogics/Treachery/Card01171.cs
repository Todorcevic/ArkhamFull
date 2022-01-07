using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01171 : CardTreachery
    {
        List<CardComponent> AllCultists =>
            GameControl.AllCardComponents.FindAll(c => c.CardLogic is CardEnemy && c.IsInPlay && c.KeyWords.Contains("Cultist"));

        /*****************************************************************************************/
        public override IEnumerator Revelation()
        {
            if (AllCultists.Count > 0) yield return new EffectAction(PutDoom).RunNow();
            else yield return new EffectAction(SearchCultist).RunNow();
        }

        IEnumerator PutDoom()
        {
            (CardComponent acolite, int distance) acolites = (null, 99);
            foreach (CardComponent culti in AllCultists)
            {
                int distance = new MoveHunterEnemy(null).InitializerFindPath(new List<CardComponent>() { GameControl.ActiveInvestigator.CurrentLocation }, ((CardEnemy)culti.CardLogic).CurrentLocation).distance;
                if (distance <= acolites.distance) acolites = (culti, distance);
            }
            yield return new AddTokenAction(acolites.acolite.DoomToken, 2).RunNow();
        }

        IEnumerator SearchCultist()
        {
            List<CardComponent> cultistListCards = AllComponents.Table.EncounterDeck.ListCards
                .Concat(AllComponents.Table.EncounterDiscard.ListCards).ToList()
                .FindAll(c => c.CardType == CardType.Enemy && c.KeyWords.Contains("Cultist"));
            List<CardEffect> cardEffects = new List<CardEffect>();
            foreach (CardComponent card in cultistListCards)
            {
                yield return new TurnCardAction(card, isBack: false).RunNow();
                cardEffects.Add(new CardEffect(
                    card: card,
                    effect: () => DrawCultist(card),
                    type: EffectType.Choose,
                    name: "Robar " + card.Info.Name));
            }
            yield return new ChooseCardAction(cardEffects, isOptionalChoice: false).RunNow();
            yield return new MoveDeck(cultistListCards.FindAll(c => c.CurrentZone.ZoneType == Zones.EncounterDeck), AllComponents.Table.EncounterDeck, isBack: true, withMoveUp: true).RunNow();
            yield return new ShuffleAction(AllComponents.Table.EncounterDeck).RunNow();

            IEnumerator DrawCultist(CardComponent card)
            {
                yield return new DrawAction(GameControl.ActiveInvestigator, card, withShow: true).RunNow();
                cultistListCards.Remove(card);
            }
        }
    }
}