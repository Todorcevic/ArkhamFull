using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ArkhamMenu
{
    public class InvestigatorImageComponent : MonoBehaviour
    {
        [SerializeField] Image image;

        Sprite currentImage;
        public Sprite CurrentImage
        {
            get => currentImage;
            set
            {
                currentImage = value;
                if (value == null)
                {
                    image.sprite = null;
                    image.enabled = false;
                }
                else
                {
                    image.enabled = true;
                    image.sprite = value;
                }
            }
        }
    }
}
