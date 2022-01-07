using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01120 : CardScenario
    {
        List<CardComponent> AllCultists =>
            GameControl.AllCardComponents.FindAll(c => c.CardLogic is CardEnemy && c.IsInPlay && c.KeyWords.Contains("Cultist"));

        /*****************************************************************************************/
        public override int SkullTokenValue()
        {
            if (IsHardDifficulty)
            {
                int totalDoomAmount = 0;
                GameControl.AllCardComponents.ForEach(c => totalDoomAmount -= c.DoomToken.Amount);
                return totalDoomAmount;
            }
            else
                return (AllCultists.Count > 0) ? -AllCultists.Max(c => c.DoomToken.Amount) : 0;
        }

        public override int CultistTokenValue() => -2;

        public override int TabletTokenValue()
        {
            if (IsHardDifficulty) return -4;
            else return -3;
        }

        public override IEnumerator CultistTokenEffect()
        {
            if (IsHardDifficulty)
            {
                foreach (CardComponent cultist in AllCultists)
                    yield return new AddTokenAction(cultist.DoomToken, 1).RunNow();

                if (AllCultists.Count < 1)
                {
                    SkillTest skillTest = SkillTest;
                    yield return new RevealChaosTokenAction(ref skillTest).RunNow();
                    yield return new SkillTestChaosTokenEffect(skillTest.TokenThrow).RunNow();
                }
            }
            else if (AllCultists.Count > 0)
            {
                (CardComponent acolite, int distance) acolites = (null, 99);
                foreach (CardComponent culti in AllCultists)
                {
                    int distance = new MoveHunterEnemy(null).InitializerFindPath(new List<CardComponent>() { GameControl.ActiveInvestigator.CurrentLocation }, ((CardEnemy)culti.CardLogic).CurrentLocation).distance;
                    if (distance <= acolites.distance) acolites = (culti, distance);
                }
                yield return new AddTokenAction(acolites.acolite.DoomToken, 1).RunNow();
            }
        }

        public override IEnumerator TabletTokenEffect()
        {
            if (GameControl.ActiveInvestigator.Clues > 0)
                SkillTest.LoseEffect.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => new AddTokenAction(GameControl.ActiveInvestigator.CurrentLocation.CluesToken, IsHardDifficulty ? GameControl.ActiveInvestigator.Clues : 1, GameControl.ActiveInvestigator.InvestigatorCardComponent.CluesToken).RunNow(),
                    type: EffectType.Choose,
                    name: "Soltar pistas"));
            yield return null;
        }
    }
}