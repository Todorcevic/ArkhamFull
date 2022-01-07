using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamShared
{
    public static class GameFiles
    {
        public const string RESOURCE_PATH = "JsonFiles/";
        const string HISTORIES_PATH = "History/Histories";
        const string SCENARIO_DECKS_PATH = "Decks/";
        const string CHAOS_BAG_DIRECTORY = "ChaosBag/";

        public const string INVESTIGATOR_BACK_IMAGE = "InvestigatorBack";
        public const string ENCOUNTER_BACK_IMAGE = "EncounterBack";

        public const string ALL_CARD_DATA_JSON = "AllCardData";
        public const string NEW_INVESTIGATORS_FILE = "InvestigatorsDataDefault";
        public const string TEST_INVESTIGATORS_FILE = "InvestigatorsDataTest";
        public const string CURRENT_INVESTIGATORS_FILE = "InvestigatorsDataSave";
        public const string GAMEDATA_FILE = "GameData";

        public const string ENCOUNTER_JSON = "Encounter";
        public const string ACT_JSON = "Act";
        public const string AGENDA_JSON = "Agenda";
        public const string LOCATION_JSON = "Location";
        public const string SCENARIO_JSON = "Scenario";
        public const string SPECIAL_JSON = "Special";

        public static string ScenarioPath => RESOURCE_PATH + GameData.Instance.Chapter + "/" + GameData.Instance.Scenario + "/" + SCENARIO_DECKS_PATH;
        public static string HistoriesPath => RESOURCE_PATH + GameData.Instance.Chapter + "/" + GameData.Instance.Scenario + "/" + HISTORIES_PATH;
        public static string ChaosBagPath => RESOURCE_PATH + GameData.Instance.Chapter + "/" + CHAOS_BAG_DIRECTORY;
        public static string StartedGameFilePath => Application.persistentDataPath + "/" + CURRENT_INVESTIGATORS_FILE + ".json";
        public static string GameDataFilePath => Application.persistentDataPath + "/" + GAMEDATA_FILE + ".json";
    }
}