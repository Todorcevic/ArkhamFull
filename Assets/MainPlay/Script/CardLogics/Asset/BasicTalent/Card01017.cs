using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01017 : BasicTalent
    {
        protected override Skill SkillToTest => (Skill.Combat | Skill.Willpower);
    }
}