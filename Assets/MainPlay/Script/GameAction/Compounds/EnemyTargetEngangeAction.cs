using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class EnemyTargetEngangeAction : GameAction
    {
        readonly CardComponent enemy;
        InvestigatorComponent investigatorToEngange = null;
        public override GameActionType GameActionType => GameActionType.Compound;

        /*****************************************************************************************/
        public EnemyTargetEngangeAction(CardComponent enemy) => this.enemy = enemy;

        /*****************************************************************************************/

        protected override IEnumerator ActionLogic()
        {
            yield return SelectInvestigatorToEngange(enemy.CurrentZone);
            if (investigatorToEngange != null)
            {
                yield return new SelectInvestigatorAction(investigatorToEngange).RunNow();
                yield return new EngangeAction(enemy, investigatorToEngange).RunNow();
            }
        }

        IEnumerator SelectInvestigatorToEngange(Zone location)
        {
            List<InvestigatorComponent> investigatorsTarget = ((CardEnemy)enemy.CardLogic).Prey(GameControl.AllInvestigatorsInGame.FindAll(i => i.PlayCard.CurrentZone == location));
            if (investigatorsTarget.Count < 1) investigatorsTarget = GameControl.AllInvestigatorsInGame.FindAll(i => i.PlayCard.CurrentZone == location);

            if (investigatorsTarget.Count == 1) investigatorToEngange = investigatorsTarget[0];
            else if (investigatorsTarget.Count > 1)
            {
                List<CardEffect> cardEffects = new List<CardEffect>();
                foreach (CardComponent investigator in investigatorsTarget.Select(c => (c.PlayCard)).ToList())
                    cardEffects.Add(new CardEffect(
                        card: investigator,
                        effect: () => SelectInvestigator(investigator),
                        type: EffectType.Choose,
                        name: "Enfrentar " + enemy.Info.Name + " con " + investigator.Info.Name,
                        investigatorImageCardInfoOwner: investigator.VisualOwner));
                yield return new ChooseCardAction(cardEffects, isOptionalChoice: false).RunNow();
            }

            IEnumerator SelectInvestigator(CardComponent investigator)
            {
                investigatorToEngange = investigator.Owner;
                yield return null;
            }
        }
    }
}