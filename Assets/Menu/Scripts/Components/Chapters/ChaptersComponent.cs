using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Michsky.UI.Dark;

namespace ArkhamMenu
{
    public class ChaptersComponent : MonoBehaviour
    {
        public List<GameObject> chapters;
        public ScrollRect scrollRect;

        void UnlockChapter(GameObject chapter)
        {
            chapter.GetComponent<Button>().interactable = true;
            chapter.GetComponent<UIElementSound>().enabled = true;
            chapter.transform.Find("LockBackground").gameObject.SetActive(false);
        }

        public void ScrollMove(float amountMove)
        {
            scrollRect.DOHorizontalNormalizedPos(scrollRect.horizontalNormalizedPosition + amountMove, 0.5f);
        }
    }
}
