using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Data;

namespace ArkhamGamePlay
{
    public class MoveHunterEnemy : GameAction
    {
        int distance;
        CardComponent moveToLocation;
        CardComponent[] currentPath = new CardComponent[12];
        List<CardComponent> locationsCheck = new List<CardComponent>();
        Dictionary<InvestigatorComponent, CardComponent> investigatorsTarget = new Dictionary<InvestigatorComponent, CardComponent>();
        public override string GameActionInfo => "Movimiento de los Enemigos.";
        public override GameActionType GameActionType => GameActionType.Compound;
        public Zone LocationToGoNearestInvestigator { get; set; }
        public CardComponent Enemy { get; }
        public List<InvestigatorComponent> InvestigatorsCandidates { get; set; } = new List<InvestigatorComponent>();
        public List<CardComponent> AllLocationsAllowed { get; set; } = new List<CardComponent>();

        /*****************************************************************************************/
        public MoveHunterEnemy(CardComponent enemy, CardComponent moveToLocation = null)
        {
            Enemy = enemy;
            this.moveToLocation = moveToLocation;
            AllLocationsAllowed = GameControl.AllCardComponents.FindAll(c => c.CardType == CardType.Location && c.IsInPlay);
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (((CardEnemy)Enemy.CardLogic).CurrentLocation == moveToLocation) yield break;
            List<CardComponent> enemyLocation = new List<CardComponent>() { Enemy.CurrentZone.ThisCard };
            if (moveToLocation != null)
                LocationToGoNearestInvestigator = InitializerFindPath(enemyLocation, moveToLocation).path.MyOwnZone;
            else
            {
                int? minDistance = GameControl.AllInvestigatorsInGame.Select(i => InitializerFindPath(enemyLocation, i.CurrentLocation).distance)?.Min();
                foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame)
                {
                    (CardComponent path, int distance) investigatorPath = InitializerFindPath(enemyLocation, investigator.CurrentLocation);
                    if (investigatorPath.distance <= minDistance) investigatorsTarget.Add(investigator, investigatorPath.path);
                }

                InvestigatorsCandidates = ((CardEnemy)Enemy.CardLogic).Prey(investigatorsTarget.Keys.ToList());
                if (InvestigatorsCandidates.Count == 1)
                    LocationToGoNearestInvestigator = investigatorsTarget[InvestigatorsCandidates[0]].MyOwnZone;
                else if (InvestigatorsCandidates.Count > 1)
                {
                    List<CardEffect> investigators = new List<CardEffect>();
                    foreach (InvestigatorComponent investigator in InvestigatorsCandidates)
                        investigators.Add(new CardEffect(investigator.PlayCard, () => ChoosingInvestigator(investigator), EffectType.Choose, name: Enemy.Info.Name + " perseguirá a " + investigator.InvestigatorCardComponent.Info.Name, investigatorImageCardInfoOwner: investigator));
                    yield return new ActiveInvestigatorAction(GameControl.LeadInvestigator).RunNow();
                    yield return new ChooseCardAction(investigators, isOptionalChoice: false).RunNow();
                    yield return new ActiveInvestigatorAction(null).RunNow();
                }
                else yield break;
            }
            yield return new AnimationCardAction(Enemy, withReturn: false, audioClip: Enemy.Effect8).RunNow();
            yield return new MoveCardAction(Enemy, LocationToGoNearestInvestigator, withPreview: false).RunNow();

            IEnumerator ChoosingInvestigator(InvestigatorComponent investigator)
            {
                LocationToGoNearestInvestigator = investigatorsTarget[investigator].MyOwnZone;
                yield return null;
            }
        }

        public (CardComponent path, int distance) InitializerFindPath(List<CardComponent> listLocation, CardComponent moveToLocation)
        {
            currentPath = new CardComponent[12];
            locationsCheck = new List<CardComponent>();
            distance = 0;
            return FindPath(listLocation, moveToLocation);
        }

        (CardComponent path, int distance) FindPath(List<CardComponent> listLocation, CardComponent moveToLocation)
        {
            List<CardComponent> listToCheck = new List<CardComponent>();
            foreach (CardComponent location in listLocation)
            {
                currentPath[distance] = location;
                if (location == moveToLocation) return (currentPath[1] ?? currentPath[0], distance);
                locationsCheck.Add(location);
                listToCheck.AddRange(GetMovePosibilities(location).FindAll(c => !locationsCheck.Contains(c) && !listToCheck.Contains(c)));
            }
            distance++;
            if (listToCheck.Count > 0) return FindPath(listToCheck, moveToLocation);
            return (currentPath[0], 99);
        }

        List<CardComponent> GetMovePosibilities(CardComponent location)
        {
            CardLocation logicLocation = (CardLocation)location.CardLogic;
            return AllLocationsAllowed.FindAll(c => logicLocation.MovePosibilities.HasFlag(((CardLocation)c.CardLogic).MySymbol));
        }
    }
}