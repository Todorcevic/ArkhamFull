using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01118 : CardEnemy
    {
        CardComponent Attic => GameControl.GetCard("01113");
        public override List<CardComponent> SpawnLocation => new List<CardComponent>() { Attic };
    }
}