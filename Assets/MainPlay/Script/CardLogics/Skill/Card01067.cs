using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01067 : CardSkill
    {
        protected override IEnumerator ThisCardEffect()
        {
            if (ThisCard.Owner.InvestigatorCardComponent.SanityToken.Amount > 0)
                yield return new AddTokenAction(ThisCard.Owner.InvestigatorCardComponent.SanityToken, -1).RunNow();
        }
    }
}