using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace ArkhamShared
{
    public enum Language { EN, ES }

    [Flags]
    public enum Difficulty
    {
        None = 0,
        Easy = 1,
        Standard = 2,
        Hard = 4,
        Expert = 8
    }

    public class GameData
    {
        static GameData instance;
        public static GameData Instance
        {
            get => instance ?? (instance = new GameData());
            set => instance = value;
        }

        public const int DRAW_INITIAL_HAND = 5;
        public const int MAX_SIZE_HAND = 8;
        public const int MAX_INVESTIGATORS = 4;
        public const int MAX_UNIQUE_CARDS_SELECTED = 2;
        public const int INITIAL_RESOURCES = 5;
        public const float ANIMATION_TIME_DEFAULT = 0.4f;
        public const float INTERACTIVE_TIME_DEFAULT = 0.25f;
        public const float DELAY_TIME_DEFAULT = 0.05f;
        public const float CARD_THICK = 0.01f;
        public bool IsAFinishGame { get; set; }
        public bool IsNotFirstScenario => Scenario != "Scenario1";
        public string Chapter { get; set; } = "Core";
        public string Scenario { get; set; } = "Scenario2";
        public Chapter1Data ChapterData { get; set; } = new Chapter1Data();
        [JsonIgnore] public string FullScenarioName => Chapter + Scenario;
        public Language Language { get; set; } = Language.ES;
        public Difficulty Difficulty { get; set; } = Difficulty.Standard;
        public static Color ExaustedColor => new Color(1f, 0.5f, 0.5f);
    }
}