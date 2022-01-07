using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArkhamGamePlay
{
    public class WindowsDrag : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        [SerializeField] RectTransform panelBody;
        Vector3 pointerDisplacement;

        public void OnBeginDrag(PointerEventData eventData) =>
            pointerDisplacement = Input.mousePosition - panelBody.position;

        public void OnDrag(PointerEventData eventData) =>
            panelBody.position = Input.mousePosition - pointerDisplacement;
    }
}