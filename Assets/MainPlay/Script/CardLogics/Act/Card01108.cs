using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class Card01108 : CardAct
    {
        CardComponent Studio => GameControl.GetCard("01111");
        CardComponent Hallway => GameControl.GetCard("01112");
        CardComponent Attic => GameControl.GetCard("01113");
        CardComponent Cellar => GameControl.GetCard("01114");
        CardComponent Parlor => GameControl.GetCard("01115");

        /*****************************************************************************************/
        protected override IEnumerator BackFace()
        {
            yield return SetLocations();
            yield return RemoveEnemies();
            yield return MoveInvestigators();
            yield return RemoveStudio();
        }

        IEnumerator SetLocations()
        {
            yield return new ShowCardAction(Hallway).RunNow();
            yield return new MoveCardAction(Hallway, AllComponents.Table.LocationZones[6], withPreview: false).RunNow();
            yield return new ShowCardAction(Attic).RunNow();
            yield return new MoveCardAction(Attic, AllComponents.Table.LocationZones[12], withPreview: false).RunNow();
            yield return new ShowCardAction(Cellar).RunNow();
            yield return new MoveCardAction(Cellar, AllComponents.Table.LocationZones[2], withPreview: false).RunNow();
            yield return new ShowCardAction(Parlor).RunNow();
            yield return new MoveCardAction(Parlor, AllComponents.Table.LocationZones[7], withPreview: false).RunNow();
        }

        IEnumerator RemoveEnemies()
        {
            foreach (CardComponent enemy in GameControl.AllCardComponents.FindAll(c => c.CardType == CardType.Enemy && ((CardEnemy)c.CardLogic).CurrentLocation == Studio))
                yield return new DiscardAction(enemy).RunNow();
        }

        IEnumerator MoveInvestigators()
        {
            foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame)
                yield return new MoveCardAction(investigator.PlayCard, Hallway.MyOwnZone, isFast: true).RunNow();
            yield return new WaitWhile(() => DOTween.IsTweening("MoveCard"));
        }

        IEnumerator RemoveStudio() => new DiscardAction(Studio, outGame: true).RunNow();
    }
}