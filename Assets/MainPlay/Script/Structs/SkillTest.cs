using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Schema;

namespace ArkhamGamePlay
{
    public class SkillTest
    {
        bool isWin;
        public bool IsWin
        {
            get => AutoResultWinLose ?? isWin;
            set { isWin = value; }
        }
        public bool IsOptional { get; set; }
        public bool? IsComplete { get; set; } = false;
        public bool? AutoResultWinLose { get; set; }
        public string Title { get; set; }
        public int InitialValue
        {
            get
            {
                switch (SkillType)
                {
                    case Skill.Willpower: return GameControl.ActiveInvestigator.Willpower;
                    case Skill.Intellect: return GameControl.ActiveInvestigator.Intellect;
                    case Skill.Combat: return GameControl.ActiveInvestigator.Combat;
                    case Skill.Agility: return GameControl.ActiveInvestigator.Agility;
                }
                return 0;
            }
        }
        public int TestValue { get; set; }
        public int InitialModifier { get; set; }
        public int TotalInvestigatorValue
        {
            get
            {
                if (AutoResultWinLose == false) return 0;
                return (InitialValue + InitialModifier) < 0 ? 0 : (InitialValue + InitialModifier);
            }
        }
        public int TotalTestValue
        {
            get
            {
                if (AutoResultWinLose == true) return 0;
                return TestValue < 0 ? 0 : TestValue;
            }
        }
        public Skill SkillType { get; set; }
        public SkillTestType SkillTestType { get; set; }
        public CardComponent CardToTest { get; set; }
        public ChaosTokenComponent TokenThrow { get; set; }
        public List<CardEffect> WinEffect { get; } = new List<CardEffect>();
        public List<CardEffect> LoseEffect { get; } = new List<CardEffect>();
        public List<CardComponent> Modifiers { get; set; } = new List<CardComponent>();
        public CardComponent ExtraCard { get; set; }
        public void UpdateModifier(int value)
        {
            InitialModifier += value;
            AllComponents.PanelSkillTest.UpdatePanel();
        }
    }
}