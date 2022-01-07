using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    [CreateAssetMenu]
    public class SpritesCards : ScriptableObject
    {
        public List<Sprite> cardsSprite;
        public List<Texture> cardsTexture;
    }
}