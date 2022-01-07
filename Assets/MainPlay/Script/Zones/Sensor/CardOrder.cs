using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public static class CardOrder
    {
        public static void StackOrder(this ZoneBehaviour zoneBehaviour, Transform card, float timeAnimation = GameData.INTERACTIVE_TIME_DEFAULT)
        {
            int nMaxSeparateCards = 0;
            int index = card.GetSiblingIndex();
            card.DOLocalMoveY(zoneBehaviour.yOffSet, timeAnimation).SetId("StackOrder");
            card.GetComponent<CardTools>().CardCanvas.sortingOrder = nMaxSeparateCards + 16;
            for (int n = index - 1; n >= 0; n--)
            {
                Transform child = zoneBehaviour.transform.GetChild(n);
                child.DOLocalMoveY(zoneBehaviour.yOffSet * nMaxSeparateCards--, timeAnimation).SetId("StackOrder");
                child.GetComponent<CardTools>().CardCanvas.sortingOrder = nMaxSeparateCards + 16;
            }
            nMaxSeparateCards = 0;
            for (int n = index + 1; n < zoneBehaviour.transform.childCount; n++)
            {
                Transform child = zoneBehaviour.transform.GetChild(n);
                child.DOLocalMoveY(zoneBehaviour.yOffSet * nMaxSeparateCards--, timeAnimation).SetId("StackOrder");
                child.GetComponent<CardTools>().CardCanvas.sortingOrder = nMaxSeparateCards + 16;
            }
        }

        public static IEnumerator HorizontalOrder(this ZoneBehaviour zoneBehaviour, Transform[] invisibleCards)
        {
            Array.ForEach(invisibleCards, invisibleCard => invisibleCard.gameObject.SetActive(false));
            for (int n = 0; n < zoneBehaviour.transform.childCount; n++)
                invisibleCards[n].gameObject.SetActive(true);
            yield return null;
            int i = 0;
            foreach (Transform card in zoneBehaviour.transform)
                yield return card.transform.DOMoveX(invisibleCards[i++].position.x, GameData.ANIMATION_TIME_DEFAULT * 2).SetId("HorizontalOrder");
            if (zoneBehaviour.transform.childCount > 0)
                zoneBehaviour.StayCard.localPosition = new Vector3(invisibleCards[zoneBehaviour.transform.childCount - 1].localPosition.x, zoneBehaviour.StayCard.localPosition.y, zoneBehaviour.StayCard.localPosition.z);
        }
    }
}