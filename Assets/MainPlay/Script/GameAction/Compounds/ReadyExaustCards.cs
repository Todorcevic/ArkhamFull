using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class ReadyExaustCards : GameAction
    {
        public override string GameActionInfo => "Reiniciando acciones y preparando las cartas.";
        public override GameActionType GameActionType => GameActionType.Compound;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            foreach (CardComponent card in GameControl.AllCardComponents.FindAll(c => c.IsExausted))
                yield return new ExaustCardAction(card, false, withPause: false).RunNow();
            foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame)
                yield return new UpdateActionsLeft(investigator, investigator.InitialActions).RunNow();
            yield return new WaitWhile(() => DOTween.IsTweening("Exhausting"));
        }
    }
}