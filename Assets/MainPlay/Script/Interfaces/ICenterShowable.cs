using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public interface ICenterShowable
    {
        bool CardsInPreview { get; set; }
        IEnumerator ShowPreviewCards();
        IEnumerator ShowTable();
    }
}