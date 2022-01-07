using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01135 : CardTreachery
    {
        public override bool IsPeril => true;

        /*****************************************************************************************/
        public override IEnumerator Revelation()
        {
            CardEffect spendClue = new CardEffect(
                card: ThisCard,
                effect: SpendClue,
                type: EffectType.Choose,
                name: "Gastar pista",
                needSpendClues: true
                );

            CardEffect takeDamage = new CardEffect(
                card: ThisCard,
                effect: TakeDamage,
                type: EffectType.Choose,
                name: "Recibir daño"
                );
            List<CardEffect> cardEffects = new List<CardEffect>() { spendClue, takeDamage };

            if (GameControl.ActiveInvestigator.Clues < 1) yield return new EffectAction(TakeDamage).RunNow();
            else yield return new MultiCastAction(cardEffects, isOptionalChoice: false).RunNow();
        }

        IEnumerator SpendClue() => new AddTokenAction(GameControl.ActiveInvestigator.InvestigatorCardComponent.CluesToken, -1).RunNow();

        IEnumerator TakeDamage() => new AssignDamageHorror(GameControl.ActiveInvestigator, damageAmount: 2).RunNow();
    }
}
