using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01034 : BasicTalent
    {
        protected override Skill SkillToTest => (Skill.Agility | Skill.Intellect);
    }
}