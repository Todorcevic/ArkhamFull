using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArkhamGamePlay
{
    public class TableComponent : MonoBehaviour
    {
        [SerializeField] ZoneBehaviour[] investigatorZonesBehaviour;
        [SerializeField] ZoneBehaviour[] playZonesBehaviour;
        [SerializeField] ZoneBehaviour[] locationsZoneBehaviours;

        public Zone[] LocationZones { get; set; } = new Zone[21];
        public ZoneBehaviour[] InvestigatorZonesBehaviour { get => investigatorZonesBehaviour; set => investigatorZonesBehaviour = value; }
        public ZoneBehaviour[] PlayZonesBehaviour { get => playZonesBehaviour; set => playZonesBehaviour = value; }
        public ZoneBehaviour[] LocationsZoneBehaviours { get => locationsZoneBehaviours; set => locationsZoneBehaviours = value; }
        public Zone EncounterDeck { get; set; } = new Zone(Zones.EncounterDeck);
        public Zone EncounterDiscard { get; set; } = new Zone(Zones.EncounterDiscard);
        public Zone Scenario { get; set; } = new Zone(Zones.Scenario);
        public Zone Agenda { get; set; } = new Zone(Zones.Agenda);
        public Zone Act { get; set; } = new Zone(Zones.Act);
        public Zone CenterPreview { get; set; } = new Zone(Zones.Center);
        public Zone TokenStack { get; set; } = new Zone(Zones.TokenStack);
        public Zone PlayCardZone { get; set; } = new Zone(Zones.PlayCardZone);
        public Zone VictoryZone { get; set; } = new Zone(Zones.Victory);

        public void SettingTableZones()
        {
            EncounterDeck.ZoneBehaviour = PlayZonesBehaviour[0];
            EncounterDiscard.ZoneBehaviour = PlayZonesBehaviour[1];
            Scenario.ZoneBehaviour = PlayZonesBehaviour[2];
            Agenda.ZoneBehaviour = PlayZonesBehaviour[3];
            Act.ZoneBehaviour = PlayZonesBehaviour[4];
            PlayCardZone.ZoneBehaviour = PlayZonesBehaviour[5];
            CenterPreview.ZoneBehaviour = PlayZonesBehaviour[6];
            TokenStack.ZoneBehaviour = PlayZonesBehaviour[7];
            VictoryZone.ZoneBehaviour = AllComponents.CardBuilder.ZoneBehaviour;

            int i = 0;
            foreach (ZoneBehaviour zoneBehaviour in LocationsZoneBehaviours)
            {
                LocationZones[i] = new Zone(Zones.Location);
                LocationZones[i++].ZoneBehaviour = zoneBehaviour;
            }
        }
    }
}