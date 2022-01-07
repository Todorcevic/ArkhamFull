using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01110 : CardAct
    {
        CardEnemy GhoulPriest => ((CardEnemy)GameControl.GetCard("01116").CardLogic);

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction) { }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is DefeatCardAction die && CheckActAdvance(die))
                new EffectAction(ActAdvance, ActAdvanceAnimation).AddActionTo();
        }

        bool CheckActAdvance(DefeatCardAction die)
        {
            if (this != GameControl.CurrentAct) return false;
            if (die.CardToDefeat != GhoulPriest.ThisCard) return false;
            return true;
        }

        protected override IEnumerator BackFace()
        {
            List<CardEffect> cardEffects = new List<CardEffect>()
        {
        new CardEffect(
            card: ThisCard,
            effect: Resolution1,
            type: EffectType.Choose,
            name: "Quememoslo!",
            investigatorImageCardInfoOwner: GameControl.LeadInvestigator
            ),
        new CardEffect(
            card: ThisCard,
            effect: Resolution2,
            type: EffectType.Choose,
            name: "Ni hablar de quemarlo!",
            investigatorImageCardInfoOwner: GameControl.LeadInvestigator
            )
        };
            yield return new ActiveInvestigatorAction(GameControl.LeadInvestigator).RunNow();
            yield return new TurnCardAction(ThisCard, isBack: true, withPause: true).RunNow();
            yield return new MultiCastAction(cardEffects, isOptionalChoice: false).RunNow();
        }

        IEnumerator Resolution1() => new FinishGameAction(new CoreScenario1().R1).RunNow();
        IEnumerator Resolution2() => new FinishGameAction(new CoreScenario1().R2).RunNow();
    }
}