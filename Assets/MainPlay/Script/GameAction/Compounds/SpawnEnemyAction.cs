using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class SpawnEnemyAction : GameAction
    {
        public CardComponent SpawnSite { get; set; }
        public CardComponent Enemy { get; set; }
        public InvestigatorComponent Investigator { get; set; }

        /*****************************************************************************************/
        public SpawnEnemyAction(CardComponent enemy, InvestigatorComponent investigator = null, CardComponent spawnSite = null)
        {
            Enemy = enemy;
            Investigator = investigator;
            SpawnSite = spawnSite;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (SpawnSite == null) yield return SelectingSpawnSite();
            yield return MovingCard();
        }

        IEnumerator SelectingSpawnSite()
        {
            List<CardComponent> spawnSites = ((CardEnemy)Enemy.CardLogic).SpawnLocation;
            if (spawnSites == null) SpawnSite = Investigator.PlayCard;
            else if (spawnSites.Count > 0)
            {
                List<CardEffect> spawnLocations = new List<CardEffect>();
                foreach (CardComponent location in spawnSites)
                {
                    spawnLocations.Add(new CardEffect(
                        card: location,
                        effect: () => SelectedLocation(location),
                        type: EffectType.Choose,
                        name: "Hacer que " + Enemy.Info.Name + " aparesca en " + location.Info.Name
                        ));
                }
                yield return new ChooseCardAction(spawnLocations, isOptionalChoice: false, withPreview: spawnLocations.Count > 1).RunNow();
            }
            else SpawnSite = null;

            IEnumerator SelectedLocation(CardComponent location)
            {
                SpawnSite = location;
                yield return null;
            }
        }

        IEnumerator MovingCard()
        {
            if (SpawnSite == null || !SpawnSite.IsInPlay)
                yield return new DiscardAction(Enemy, withTopUp: false).RunNow();
            else if (SpawnSite.CardType == CardType.PlayCard)
                yield return new MoveCardAction(Enemy, SpawnSite.Owner.Threat, isBack: false, withPreview: !Enemy.IsInCenterPreview).RunNow();
            else if (SpawnSite.CardType == CardType.Location)
                yield return new MoveCardAction(Enemy, SpawnSite.MyOwnZone, isBack: false, withPreview: !Enemy.IsInCenterPreview).RunNow();
        }
    }
}