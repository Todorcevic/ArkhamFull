using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01077 : BasicTalent
    {
        protected override Skill SkillToTest => (Skill.Agility | Skill.Willpower);
    }
}