using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class TestAction : PhaseAction
    {
        //List<string> chaosTokenList = JsonDataManager.CreateListFromJson<List<string>>(GameFiles.ChaosBagPath + GameData.Instance.Difficulty);
        //List<string> chaosTokenList = new List<string>() { "Chaos+1", "Chaos-0", "Chaos-1", "Chaos-2", "Chaos-3", "Chaos-4", "Chaos-5", "ChaosSkull", "ChaosTablet", "ChaosCultist", "ChaosWin", "ChaosFail" }; 
        List<string> act = new List<string>() { "01108"/*, "01109", "01110"*/ };
        List<string> agenda = new List<string>() { /*"01105", "01106",*/ "01107" };
        string scenario = "01104";
        List<string> encounterDeck = new List<string>() { "01167", "01168", "01160", "01160", "01118", "01116" };
        //List<string> encounterDeck = GameControl.Deck[DeckType.Encounter];
        List<string> encounterDiscard = new List<string>() { "01166", "01159", "01119" };

        List<string> player1Hand = new List<string>() { "01031", "01016", "01022", "01022", "01053" };
        List<string> player1Deck = new List<string>() { "01089", "01088", "01093" };
        List<string> player1Discard = new List<string>() { "01086", "01087" };

        List<string> player2Hand = new List<string>() { "01048", "01050", "01046", "01045", "01045" };
        List<string> player2Deck = new List<string>() { "01086", "01092" };

        protected override IEnumerator PhaseLogic()
        {
            InvestigatorComponent investigator1 = GameControl.AllInvestigatorsInGame[0];
            InvestigatorComponent investigator2 = GameControl.AllInvestigatorsInGame[1];
            //InvestigatorComponent investigator3 = GameControl.AllInvestigators[2];
            //InvestigatorComponent investigator4 = GameControl.AllInvestigators[3];
            CardComponent Estudio = GetCard("01111");
            CardComponent Pasillo = GetCard("01112");
            CardComponent Atico = GetCard("01113");
            CardComponent Sotano = GetCard("01114");
            CardComponent Salita = GetCard("01115");
            CardComponent Investigador1Card = investigator1.InvestigatorCardComponent;
            CardComponent Investigador2Card = investigator2.InvestigatorCardComponent;
            //CardComponent Investigador3Card = investigator3.InvestigatorCardComponent;
            //CardComponent Investigador4Card = investigator4.InvestigatorCardComponent;
            CardComponent PlayCard1 = investigator1.PlayCard;
            CardComponent PlayCard2 = investigator2.PlayCard;
            //CardComponent PlayCard3 = investigator3.PlayCard;
            //CardComponent PlayCard4 = investigator4.PlayCard;
            CardComponent GulGelido = GetCard("01119");
            CardComponent SacerdoteGul = GetCard("01116");
            CardComponent OtroGul = GetCard("01161");
            CardComponent Rata = GetCard("01159");

            /*****************************************************************************************/
            //yield return new SettingGame().RunNow();
            //yield return new CoreScenario1().RunNow();
            GameControl.NoResolution = new CoreScenario1().NoResolution;

            yield return new SetChaosBagAction(testMode: true).RunNow();
            //GameControl.GameIsStarted = true;
            yield return new ActiveInvestigatorAction(investigator1).RunNow();

            /*****************************************************************************************/

            /*Scenario*/

            yield return MoveListCards(act, AllComponents.Table.Act);
            yield return MoveListCards(agenda, AllComponents.Table.Agenda);
            yield return new MoveCardAction(GetCard(scenario), AllComponents.Table.Scenario, isBack: true, isFast: true).RunNow();
            yield return MoveListCards(encounterDeck, AllComponents.Table.EncounterDeck, back: true);
            yield return MoveListCards(encounterDiscard, AllComponents.Table.EncounterDiscard);

            /*Locations*/
            yield return new MoveCardAction(Estudio, AllComponents.Table.LocationZones[1], isFast: true).RunNow();
            //yield return new MoveCardAction(Salita, AllComponents.Table.LocationZones[7], isFast: true).RunNow();
            //yield return new MoveCardAction(Atico, AllComponents.Table.LocationZones[12], isFast: true).RunNow();
            //yield return new MoveCardAction(Sotano, AllComponents.Table.LocationZones[2], isFast: true).RunNow();
            //yield return new MoveCardAction(Pasillo, AllComponents.Table.LocationZones[6], isFast: true).RunNow();

            /*Player1*/
            yield return new MoveCardAction(Investigador1Card, investigator1.InvestigatorZone, withPreview: true).RunNow();
            yield return new MoveCardAction(PlayCard1, Estudio.MyOwnZone).RunNow();
            yield return new UpdateActionsLeft(investigator1, investigator1.InitialActions).RunNow();
            yield return MoveListCards(player1Hand, investigator1.Hand);
            yield return MoveListCards(player1Deck, investigator1.InvestigatorDeck, back: true);
            yield return MoveListCards(player1Discard, investigator1.InvestigatorDiscard);

            yield return new SelectInvestigatorAction(investigator2).RunNow();

            /*Player2*/
            yield return new MoveCardAction(Investigador2Card, investigator2.InvestigatorZone, withPreview: true).RunNow();
            yield return new MoveCardAction(PlayCard2, Estudio.MyOwnZone).RunNow();
            yield return new UpdateActionsLeft(investigator2, investigator2.InitialActions).RunNow();
            yield return MoveListCards(player2Hand, investigator2.Hand);
            yield return MoveListCards(player2Deck, investigator2.InvestigatorDeck, back: true);
            //yield return new MoveCardAction(GetCard("01010"), investigator2.InvestigatorDiscard, fast: true).RunNow();

            ///*Player3*/
            //yield return new MoveCardAction(Investigador3Card, investigator3.InvestigatorZone, withPreview: true).RunNow();
            //yield return new MoveCardAction(PlayCard3, Estudio.MyOwnZone).RunNow();
            //yield return new SetAmountActions(investigator3, GameData.InitialInvestigationActions).RunNow();

            ///*Player4*/
            //yield return new MoveCardAction(Investigador4Card, investigator4.InvestigatorZone, withPreview: true).RunNow();
            //yield return new MoveCardAction(PlayCard4, Estudio.MyOwnZone).RunNow();
            //yield return new SetAmountActions(investigator3, GameData.InitialInvestigationActions).RunNow();

            /*Enemies*/
            //yield return new MoveCardAction(GetCard("01102"), investigator1.Threat, isFast: true, isBack: false).RunNow();
            //yield return new MoveCardAction(Rata, investigator1.Threat, isFast: true).RunNow();
            //yield return new MoveCardAction(SacerdoteGul, investigator1.Threat, isFast: true, isBack: false).RunNow();
            //yield return new ExaustCardAction(Rata, withPause: false).RunNow();
            //yield return new ExaustCardAction(SacerdoteGul, withPause: false).RunNow();
            //yield return new MoveCardAction(GetCard("01159"), investigator1.Threat, isFast: true).RunNow();
            //yield return new MoveCardAction(GetCard("01161"), Atico.MyOwnZone, isFast: true, isBack: false).RunNow();
            //yield return new MoveCardAction(GulGelido, Salita.MyOwnZone, isFast: true).RunNow();


            /*Assets*/
            //yield return new MoveCardAction(GetCard("01011"), investigator1.Threat, isFast: true).RunNow();
            //yield return new MoveCardAction(GetCard("01117"), investigator2.Assets, isFast: true).RunNow();
            //yield return new MoveCardAction(GetCard("01071"), investigator1.Assets, isFast: true).RunNow();
            //yield return new MoveCardAction(GetCard("01082"), investigator1.Assets, isFast: true).RunNow();


            /*Treatery*/
            //yield return new MoveCardAction(GetCard("01009"), investigator1.Threat, isFast: true).RunNow();
            //yield return new MoveCardAction(GetCard("01164"), investigator1.Threat, isFast: true).RunNow();

            /*Tokens*/
            //yield return new AddTokenAction(SacerdoteGul.HealthToken, 9).RunNow();
            yield return new AddTokenAction(Investigador1Card.CluesToken, 2).RunNow();
            //yield return new AddTokenAction(Investigador1Card.CluesToken, 12).RunNow();
            //yield return new AddTokenAction(Investigador1Card.HealthToken, 8).RunNow();
            yield return new AddTokenAction(Investigador2Card.CluesToken, 1).RunNow();
            //yield return new AddTokenAction(Investigador1Card.SanityToken, 7).RunNow();
            //yield return new AddTokenAction(SacerdoteGul.HealthToken, 9).RunNow();
            //yield return new AddTokenAction(Investigador2Card.CluesToken, 2).RunNow();
            //yield return new AddTokenAction(SacerdoteGul.ResourcesToken, 4).RunNow();
            //yield return new AddTokenAction(GameControl.CurrentAgenda.ThisCard.DoomToken, 12).RunNow();

            /*****************************************************************************************/
            /*Start*/
            yield return new SelectInvestigatorAction(investigator1).RunNow();

            //yield return new EnemyPhase().RunNow();
            //yield return new AddPhases().RunNow();
            //yield return new InvestigationPhase().RunNow();
            //yield return new EnemyPhase().RunNow();
            //yield return new UpkeepPhase().RunNow();
            yield return new AddPhases().RunNow();
            //yield return new MythosPhase().RunNow();
            //yield return new InvestigationPhase().RunNow();
        }

        IEnumerator MoveListCards(List<string> listCards, Zone zone, bool back = false)
        {

            listCards.Reverse();
            foreach (string cardId in listCards)
            {
                Debug.Log("Moving " + cardId);
                yield return new MoveCardAction(GetCard(cardId), zone, isFast: true, isBack: back).RunNow();
            }
        }

        List<CardComponent> cardsPlaying = new List<CardComponent>();

        CardComponent GetCard(string id)
        {
            CardComponent card = GameControl.AllCardComponents.Except(cardsPlaying).ToList().Find(c => c.ID == id) ?? GameControl.AllCardComponents.Find(c => c.ID == id);
            cardsPlaying.Add(card);
            return card;
        }
    }
}