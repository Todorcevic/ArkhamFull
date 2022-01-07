using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class TestAction2 : PhaseAction
    {
        //List<string> chaosTokenList = JsonDataManager.CreateListFromJson<List<string>>(GameFiles.ChaosBagPath + GameData.Instance.Difficulty);
        //List<string> chaosTokenList = new List<string>() { "Chaos+1", "Chaos-0", "Chaos-1", "Chaos-2", "Chaos-3", "Chaos-4", "Chaos-5", "ChaosSkull", "ChaosTablet", "ChaosCultist", "ChaosWin", "ChaosFail" }; 
        List<string> act = new List<string>() { "01123" };
        List<string> agenda = new List<string>() { /*"01121a",*/ "01122" };
        string scenario = "01120";
        List<string> encounterDeck = new List<string>() { "01136", "01168", "01171", "01135", "01173", "01169" };
        //List<string> encounterDeck = GameControl.Deck[DeckType.Encounter];
        List<string> encounterDiscard = new List<string>() { "01169", "01170" };

        List<string> player1Hand = new List<string>() { "01040", "01042", "01042", "01040", "01040", "01040", "01068", "01008" };
        List<string> player1Deck = new List<string>() { "01096", "01089", "01088", "01093" };
        List<string> player1Discard = new List<string>() { "01086", "01087" };

        List<string> player2Hand = new List<string>() { "01048", "01050", "01046", "01045", "01010", "01055" };
        List<string> player2Deck = new List<string>() { "01086", "01092" };

        protected override IEnumerator PhaseLogic()
        {
            InvestigatorComponent investigator1 = GameControl.AllInvestigatorsInGame[0];
            InvestigatorComponent investigator2 = GameControl.AllInvestigatorsInGame[1];
            //InvestigatorComponent investigator3 = GameControl.AllInvestigators[2];
            //InvestigatorComponent investigator4 = GameControl.AllInvestigators[3];
            CardComponent TuCasa = GetCard("01124");
            CardComponent Fluvial = GetCard("01125");
            CardComponent Sur1 = GetCard("01126");
            CardComponent Sur2 = GetCard("01127");
            CardComponent Hospital = GetCard("01128");
            CardComponent Universidad = GetCard("01129");
            CardComponent Centro1 = GetCard("01130");
            CardComponent Centro2 = GetCard("01131");
            CardComponent Este = GetCard("01132");
            CardComponent Cementerio = GetCard("01133");
            CardComponent Norte = GetCard("01134");
            CardComponent SacerdoteGul = GetCard("01116");
            CardComponent Acolito = GetCard("01169");
            CardComponent Investigador1Card = investigator1.InvestigatorCardComponent;
            CardComponent Investigador2Card = investigator2.InvestigatorCardComponent;
            //CardComponent Investigador3Card = investigator3.InvestigatorCardComponent;
            //CardComponent Investigador4Card = investigator4.InvestigatorCardComponent;
            CardComponent PlayCard1 = investigator1.PlayCard;
            CardComponent PlayCard2 = investigator2.PlayCard;
            //CardComponent PlayCard3 = investigator3.PlayCard;
            //CardComponent PlayCard4 = investigator4.PlayCard;
            //CardComponent GulGelido = GetCard("01119");
            //CardComponent SacerdoteGul = GetCard("01116");
            //CardComponent OtroGul = GetCard("01161");
            //CardComponent Rata = GetCard("01159");
            CardComponent MaskedHunter = GetCard("01121b");

            /*****************************************************************************************/
            //yield return new SettingGame().RunNow();
            //yield return new CoreScenario1().RunNow();
            GameControl.NoResolution = new CoreScenario2().R1;

            yield return new SetChaosBagAction(testMode: true).RunNow();
            //GameControl.GameIsStarted = true;
            yield return new ActiveInvestigatorAction(investigator1).RunNow();

            /*****************************************************************************************/

            /*Scenario*/

            yield return MoveListCards(act, AllComponents.Table.Act);
            yield return MoveListCards(agenda, AllComponents.Table.Agenda);
            yield return new MoveCardAction(GetCard(scenario), AllComponents.Table.Scenario, isBack: false, isFast: true).RunNow();
            yield return MoveListCards(encounterDeck, AllComponents.Table.EncounterDeck, back: true);
            yield return MoveListCards(encounterDiscard, AllComponents.Table.EncounterDiscard);

            /*Locations*/
            //yield return new MoveCardAction(Estudio, AllComponents.Table.LocationZones[1], isFast: true).RunNow();
            yield return new MoveCardAction(TuCasa, AllComponents.Table.LocationZones[2], isFast: true).RunNow();
            yield return new MoveCardAction(Fluvial, AllComponents.Table.LocationZones[6], isFast: true).RunNow();
            yield return new MoveCardAction(Sur2, AllComponents.Table.LocationZones[1], isFast: true).RunNow();
            yield return new MoveCardAction(Hospital, AllComponents.Table.LocationZones[0], isFast: true).RunNow();
            yield return new MoveCardAction(Universidad, AllComponents.Table.LocationZones[5], isFast: true).RunNow();
            yield return new MoveCardAction(Centro2, AllComponents.Table.LocationZones[11], isFast: true).RunNow();
            yield return new MoveCardAction(Este, AllComponents.Table.LocationZones[12], isFast: true).RunNow();
            yield return new MoveCardAction(Cementerio, AllComponents.Table.LocationZones[7], isFast: true).RunNow();
            yield return new MoveCardAction(Norte, AllComponents.Table.LocationZones[10], isFast: true).RunNow();

            /*Player1*/
            yield return new MoveCardAction(Investigador1Card, investigator1.InvestigatorZone, withPreview: true).RunNow();
            yield return new MoveCardAction(PlayCard1, Hospital.MyOwnZone).RunNow();
            yield return new UpdateActionsLeft(investigator1, investigator1.InitialActions).RunNow();
            yield return MoveListCards(player1Hand, investigator1.Hand);
            yield return MoveListCards(player1Deck, investigator1.InvestigatorDeck, back: true);
            yield return MoveListCards(player1Discard, investigator1.InvestigatorDiscard);

            yield return new SelectInvestigatorAction(investigator2).RunNow();

            /*Player2*/
            yield return new MoveCardAction(Investigador2Card, investigator2.InvestigatorZone, withPreview: true).RunNow();
            yield return new MoveCardAction(PlayCard2, Hospital.MyOwnZone).RunNow();
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
            //yield return new MoveCardAction(MaskedHunter, investigator1.Threat, isFast: true, isBack: false).RunNow();
            //yield return new ExaustCardAction(Rata, withPause: false).RunNow();
            //yield return new ExaustCardAction(SacerdoteGul, withPause: false).RunNow();
            //yield return new MoveCardAction(GetCard("01139"), investigator1.Threat, isFast: true).RunNow();
            //yield return new MoveCardAction(GetCard("01161"), Atico.MyOwnZone, isFast: true, isBack: false).RunNow();
            //yield return new MoveCardAction(GetCard("01169"), investigator2.Threat, isFast: true, isBack: false).RunNow();
            //yield return new MoveCardAction(GetCard("01172"), Este.MyOwnZone, isFast: true, isBack: false).RunNow();


            /*Assets*/
            //yield return new MoveCardAction(GetCard("01011"), investigator1.Threat, isFast: true).RunNow();
            //yield return new MoveCardAction(GetCard("01117"), investigator2.Assets, isFast: true).RunNow();
            //yield return new MoveCardAction(GetCard("01071"), investigator1.Assets, isFast: true).RunNow();
            //yield return new MoveCardAction(GetCard("01082"), investigator1.Assets, isFast: true).RunNow();


            /*Treatery*/
            //yield return new MoveCardAction(GetCard("01011"), investigator2.Threat, isFast: true).RunNow();
            //yield return new MoveCardAction(GetCard("01174"), Hospital.MyOwnZone, isFast: true).RunNow();

            /*Tokens*/
            //yield return new AddTokenAction(SacerdoteGul.HealthToken, 9).RunNow();
            yield return new AddTokenAction(Investigador1Card.ResourcesToken, 10).RunNow();
            yield return new AddTokenAction(Investigador1Card.CluesToken, 4).RunNow();
            //yield return new AddTokenAction(Investigador2Card.HealthToken, 3).RunNow();
            //yield return new AddTokenAction(Investigador1Card.HealthToken, 7).RunNow();
            //yield return new AddTokenAction(Investigador2Card.SanityToken, 4).RunNow();
            //yield return new AddTokenAction(TuCasa.CluesToken, 4).RunNow();
            //yield return new AddTokenAction(MaskedHunter.HealthToken, 7).RunNow();
            //yield return new AddTokenAction(Investigador2Card.CluesToken, 2).RunNow();
            yield return new AddTokenAction(Investigador2Card.ResourcesToken, 4).RunNow();
            //yield return new AddTokenAction(GameControl.CurrentAgenda.ThisCard.DoomToken, 7).RunNow();

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
                yield return new MoveCardAction(GetCard(cardId), zone, isFast: true, isBack: back).RunNow();
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