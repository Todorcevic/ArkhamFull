using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01097 : InvestigatorTreachery
    {
        public override IEnumerator Revelation() =>
          new AddTokenAction(Investigator.InvestigatorCardComponent.ResourcesToken, -Investigator.Resources).RunNow();
    }
}