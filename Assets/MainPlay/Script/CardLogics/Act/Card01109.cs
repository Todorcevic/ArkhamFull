using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01109 : CardAct
    {
        CardComponent Parlor => GameControl.GetCard("01115");
        CardComponent Hallway => GameControl.GetCard("01112");
        CardComponent LitaChantler => GameControl.GetCard("01117");
        CardComponent GhoulPriest => GameControl.GetCard("01116");
        List<InvestigatorComponent> ThisInvestigatorsCanGiveClues => GameControl.AllInvestigatorsInGame
                .Where(i => i.InvestigatorCardComponent.CluesToken.Amount > 0 && i.CurrentLocation == Hallway)
                .ToList();

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction) { }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is UpkeepPhase && CanGiveClues())
                new AssignClues(ThisInvestigatorsCanGiveClues).AddActionTo();
        }

        bool CanGiveClues()
        {
            if (GameControl.CurrentAct != this) return false;
            if (ThisInvestigatorsCanGiveClues.Count < 1) return false;
            return true;
        }

        protected override IEnumerator BackFace()
        {
            yield return new RevealLocationAction(Parlor).RunNow();
            yield return new ShowCardAction(LitaChantler).RunNow();
            yield return new MoveCardAction(LitaChantler, Parlor.MyOwnZone, withPreview: false).RunNow();
            yield return new ShowCardAction(GhoulPriest).RunNow();
            yield return new MoveCardAction(GhoulPriest, Hallway.MyOwnZone, withPreview: false).RunNow();
        }
    }
}