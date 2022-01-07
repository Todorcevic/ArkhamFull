using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArkhamShared;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class CoreScenario2 : PhaseAction
    {
        public static List<CardComponent> CultistDeck = GameControl.Deck[DeckType.Special];
        CardComponent ghoulPriest = GameControl.GetCard("01116");
        CardComponent center = Random.Range(0, 1) == 0 ? GameControl.Deck[DeckType.Location][4] : GameControl.Deck[DeckType.Location][3];
        CardComponent south = Random.Range(0, 1) == 0 ? GameControl.Deck[DeckType.Location][8] : GameControl.Deck[DeckType.Location][7];
        CardComponent cementery = GameControl.GetCard("01133");
        CardComponent Acolyte => AllComponents.Table.EncounterDeck.ListCards.Find(c => c.ID == "01169");
        public string NextScenario => "Scenario3";


        /*****************************************************************************************/
        protected override IEnumerator PhaseLogic()
        {
            yield return PanelHistory();
            yield return SetEncounterDeck();
            yield return SetScenario();
            yield return SetAgenda();
            yield return SetAct();
            yield return SetLocations();
            yield return SetAcolyte();
            yield return SetPlayCards();
            GameControl.NoResolution = R1;
        }

        IEnumerator PanelHistory()
        {
            if (GameData.Instance.ChapterData.LitaLeave)
                yield return new PanelHistoryAction("scenario2_intro1").RunNow();
            else
                yield return new PanelHistoryAction("scenario2_intro2").RunNow();
            yield return new PanelHistoryAction("scenario2_intro3").RunNow();
        }

        IEnumerator SetEncounterDeck()
        {
            if (!GameData.Instance.ChapterData.GhoulPriestAlive)
                GameControl.Deck[DeckType.Encounter].Remove(ghoulPriest);
            yield return new MoveDeck(GameControl.Deck[DeckType.Encounter], AllComponents.Table.EncounterDeck, isBack: true).RunNow();
            yield return new ShuffleAction(AllComponents.Table.EncounterDeck).RunNow();
        }

        IEnumerator SetScenario()
        {
            bool isBack = (Difficulty.Expert | Difficulty.Hard).HasFlag(GameData.Instance.Difficulty);
            yield return new ShowCardAction(GameControl.Deck[DeckType.Scenario][0], isBack: isBack).RunNow();
            yield return new MoveCardAction(GameControl.Deck[DeckType.Scenario][0], AllComponents.Table.Scenario, withPreview: false).RunNow();
        }

        IEnumerator SetAgenda()
        {
            yield return new MoveDeck(GameControl.Deck[DeckType.Agenda], AllComponents.Table.Agenda).RunNow();
            yield return new ShowCardAction(GameControl.CurrentAgenda.ThisCard).RunNow();
            yield return new MoveCardAction(GameControl.CurrentAgenda.ThisCard, AllComponents.Table.Agenda, withPreview: false).RunNow();
        }

        IEnumerator SetAct()
        {
            yield return new MoveDeck(GameControl.Deck[DeckType.Act], AllComponents.Table.Act).RunNow();
            yield return new ShowCardAction(GameControl.CurrentAct.ThisCard).RunNow();
            yield return new MoveCardAction(GameControl.CurrentAct.ThisCard, AllComponents.Table.Act, withPreview: false).RunNow();
        }

        IEnumerator SetLocations()
        {
            if (GameData.Instance.ChapterData.LeadHomeUp)
                yield return MoveToZoneLocation(GameControl.Deck[DeckType.Location][10], AllComponents.Table.LocationZones[2]);
            yield return MoveToZoneLocation(GameControl.Deck[DeckType.Location][9], AllComponents.Table.LocationZones[6]);
            yield return MoveToZoneLocation(south, AllComponents.Table.LocationZones[1]);
            yield return MoveToZoneLocation(GameControl.Deck[DeckType.Location][6], AllComponents.Table.LocationZones[0]);
            yield return MoveToZoneLocation(GameControl.Deck[DeckType.Location][5], AllComponents.Table.LocationZones[5]);
            yield return MoveToZoneLocation(center, AllComponents.Table.LocationZones[11]);
            yield return MoveToZoneLocation(GameControl.Deck[DeckType.Location][2], AllComponents.Table.LocationZones[12]);
            yield return MoveToZoneLocation(GameControl.Deck[DeckType.Location][1], AllComponents.Table.LocationZones[7]);
            yield return MoveToZoneLocation(GameControl.Deck[DeckType.Location][0], AllComponents.Table.LocationZones[10]);
        }

        IEnumerator MoveToZoneLocation(CardComponent location, Zone zone)
        {
            yield return new ShowCardAction(location).RunNow();
            yield return new MoveCardAction(location, zone, withPreview: false).RunNow();
        }

        IEnumerator SetAcolyte()
        {
            if (GameControl.AllInvestigatorsInGame.Count > 1)
                yield return SpawnAcolyte(Acolyte, south);
            if (GameControl.AllInvestigatorsInGame.Count > 2)
                yield return SpawnAcolyte(Acolyte, center);
            if (GameControl.AllInvestigatorsInGame.Count > 3)
                yield return SpawnAcolyte(Acolyte, cementery);
        }

        IEnumerator SpawnAcolyte(CardComponent acolyte, CardComponent location)
        {
            yield return new ShowCardAction(acolyte).RunNow();
            yield return new SpawnEnemyAction(acolyte, spawnSite: location).RunNow();
        }

        IEnumerator SetPlayCards()
        {
            CardComponent startLocation = GameData.Instance.ChapterData.LeadHomeUp ?
                GameControl.Deck[DeckType.Location][10] :
                GameControl.Deck[DeckType.Location][9];
            foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame)
                yield return new MoveCardAction(investigator.PlayCard, startLocation.MyOwnZone, isFast: true).RunNow();
            yield return new WaitWhile(() => DOTween.IsTweening("MoveCard"));
        }

        public IEnumerator R1()
        {
            yield return new PanelHistoryAction("scenario2_resolution1").RunNow();
            foreach (CardComponent cultist in CultistDeck)
                Interrogatory(cultist);
            GameData.Instance.ChapterData.GhoulPriestAlive = !AllComponents.PanelCampaign.VictoryCards.Contains(ghoulPriest);
            if (GameData.Instance.ChapterData.GhoulPriestAlive) AllComponents.PanelCampaign.RegisterText(ghoulPriest.Info.Name + " sigue vivo.");
            GameData.Instance.Scenario = NextScenario;
        }

        void Interrogatory(CardComponent cultist)
        {
            bool isVictory = AllComponents.PanelCampaign.VictoryCards.Contains(cultist);
            AllComponents.PanelCampaign.RegisterText(cultist.Info.Name + (isVictory ? " ha sido interrogado." : " ha escapado."));
            GameData.Instance.ChapterData.Cultists.Add(cultist.ID, isVictory);
        }

        public IEnumerator R2()
        {
            yield return new PanelHistoryAction("scenario2_resolution2").RunNow();
            AllComponents.PanelCampaign.RegisterText("Es más de medianoche.");
            GameData.Instance.ChapterData.IsMidNight = true;
            yield return R1();
        }
    }
}
