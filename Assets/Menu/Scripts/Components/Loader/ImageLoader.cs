using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using ArkhamShared;

namespace ArkhamMenu
{
    public class ImageLoader : MonoBehaviour
    {
        [SerializeField] ScriptableSprite englishSprites;
        [SerializeField] ScriptableSprite spanishSprites;
        public static List<Sprite> cardsImage;

        public void CreateImageCards()
        {
            switch (GameData.Instance.Language)
            {
                default:
                    {
                        cardsImage = englishSprites.cardsSprite;
                        break;
                    }
                case Language.ES:
                    {
                        cardsImage = spanishSprites.cardsSprite;
                        break;
                    }
            }
        }
    }
}
