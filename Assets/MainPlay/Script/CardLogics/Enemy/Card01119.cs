using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01119 : CardEnemy
    {
        CardComponent Cellar => GameControl.GetCard("01114");
        public override List<CardComponent> SpawnLocation => new List<CardComponent>() { Cellar };
    }
}