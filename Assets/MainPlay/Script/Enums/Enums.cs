using System;

namespace ArkhamGamePlay
{
    [Flags]
    public enum CardType
    {
        None = 0,
        Act = 1,
        Agenda = 2,
        Asset = 4,
        Enemy = 8,
        Event = 16,
        Investigator = 32,
        Location = 64,
        Scenario = 128,
        Skill = 256,
        Treachery = 512,
        Resource = 1024,
        PlayCard = 2048
    }

    [Flags]
    public enum DeckType
    {
        None = 0,
        Act = 1,
        Agenda = 2,
        Scenario = 4,
        Encounter = 8,
        Location = 16,
        Special = 32
    }

    [Flags]
    public enum CardTokenType
    {
        None = 0,
        Health = 1,
        Sanity = 2,
        Resources = 4,
        Clues = 8,
        Doom = 16
    }

    [Flags]
    public enum ChaosTokenType
    {
        None = 0,
        Basic = 1,
        Cultist = 2,
        Fail = 4,
        Win = 8,
        Tablet = 16,
        Skull = 32,
        Kthulu = 64
    }

    [Flags]
    public enum GameActionType
    {
        None = 0,
        Basic = 1,
        Setting = 2,
        Phase = 4,
        Interactable = 8,
        Card = 16,
        History = 32,
        SkillTest = 64,
        Compound = 128
    }

    [Flags]
    public enum Zones
    {
        None = 0,
        Investigator = 1,
        InvestigatorDeck = 2,
        InvestigatorDiscard = 4,
        Hand = 8,
        Assets = 16,
        Threat = 32,
        EncounterDeck = 64,
        EncounterDiscard = 128,
        Scenario = 256,
        Agenda = 512,
        Act = 1024,
        Location = 2048,
        IntoCard = 4096,
        CardBuilder = 8192,
        PlayCardZone = 16384,
        Center = 32768,
        TokenStack = 65536,
        Victory = 131072
    }

    [Flags]
    public enum ButtonState
    {
        None = 0,
        Ready = 1,
        StandBy = 2,
        Off = 4
    }

    [Flags]
    public enum Skill
    {
        None = 0,
        Combat = 1,
        Agility = 2,
        Intellect = 4,
        Willpower = 8
    }

    [Flags]
    public enum SkillTestType
    {
        None = 0,
        Attack = 1,
        Evade = 2,
        Investigate = 4
    }

    [Flags]
    public enum LocationSymbol
    {
        None = 0,
        Circle = 1,
        Square = 2,
        Diamond = 4,
        Plus = 8,
        Triangle = 16,
        Bar = 32,
        Moon = 64,
        X = 128,
        T = 256
    }

    [Flags]
    public enum EffectType
    {
        None = 0,
        Instead = 1,
        Activate = 2,
        Reaction = 4,
        Fight = 8,
        Engange = 16,
        Evade = 32,
        Investigate = 64,
        Move = 128,
        Resource = 256,
        Draw = 512,
        Play = 1024,
        Resign = 2048,
        Parley = 4096,
        Fast = 8192,
        Choose = 16384,
        Modifier = 32768
    }

    [Flags]
    public enum CardState
    {
        None = 0,
        Inactive = 1,
        Active = 2,
        Selected = 4,
        SelectedRed = 8
    }
}