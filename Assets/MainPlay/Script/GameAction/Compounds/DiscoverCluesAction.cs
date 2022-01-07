using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class DiscoverCluesAction : GameAction
    {
        public InvestigatorComponent Investigator { get; set; }
        public int Amount { get; set; }
        public CardComponent Location { get; set; }
        public DiscoverCluesAction(InvestigatorComponent investigator, int amount, CardComponent location = null)
        {
            Investigator = investigator;
            Amount = amount;
            Location = location ?? investigator.CurrentLocation;
        }
        protected override IEnumerator ActionLogic()
        {
            if (Location.CluesToken.Amount > 0)
                yield return new AddTokenAction(Investigator.InvestigatorCardComponent.CluesToken, Location.CluesToken.Amount < Amount ? Location.CluesToken.Amount : Amount, Location.CluesToken).RunNow();
        }

        protected override IEnumerator Animation() =>
            new AnimationCardAction(Location, audioClip: Location.ClipType3).RunNow();
    }
}
