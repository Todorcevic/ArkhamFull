using ArkhamGamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01123 : CardAct
    {
        int cluesAmountToActive = GameControl.InvestigatorsStartingAmount * 2;
        CardEffect cardEffect;
        readonly List<CardComponent> cultistsSpawned = new List<CardComponent>();
        List<CardComponent> cultistToSpawn => CoreScenario2.CultistDeck.FindAll(c => c.ID != "01121b").Except(cultistsSpawned).ToList();

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InvestigatorTurn investigatorTurn && CheckGiveClues())
                investigatorTurn.CardEffects.Add(cardEffect = new CardEffect(
                    card: ThisCard,
                    effect: () => AssignClues(cluesAmountToActive),
                    type: EffectType.Activate,
                    name: "Entregar " + cluesAmountToActive + " pistas y robar carta de sectario",
                    actionCost: 1
                    ));
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is AddVictoryCardAction addVictoryAction && CheckVictoryCards())
                new EffectAction(ActAdvance, ActAdvanceAnimation).AddActionTo();
        }

        bool CheckGiveClues()
        {
            if (GameControl.AllInvestigatorsInGame.Sum(c => c.Clues) < cluesAmountToActive) return false;
            if (cultistToSpawn.Count < 1) return false;
            return true;
        }

        bool CheckVictoryCards()
        {
            if (GameControl.CurrentAct.ThisCard != ThisCard) return false;
            if (AllComponents.PanelCampaign.VictoryCards.FindAll(c => c.CardLogic is CardEnemy && c.KeyWords.Contains("Cultist")).Count < 6) return false;
            return true;
        }

        protected override IEnumerator BackFace() => new FinishGameAction(new CoreScenario2().R1).RunNow();

        IEnumerator AssignClues(int clues)
        {
            AssignClues assign = new AssignClues(GameControl.AllInvestigatorsInGame.FindAll(c => c.InvestigatorCardComponent.CluesToken.Amount > 0), cluesLimit: clues);
            yield return assign.RunNow();
            if (!assign.IsCancel)
            {
                yield return ActAdvanceAnimation();
                CardComponent cultist = cultistToSpawn[Random.Range(0, CoreScenario2.CultistDeck.Count - 1)];
                yield return new ShowCardAction(cultist).RunNow();
                yield return new SpawnEnemyAction(cultist).RunNow();
                cultistsSpawned.Add(cultist);

            }
            else cardEffect.IsCancel = true;
        }
    }
}
