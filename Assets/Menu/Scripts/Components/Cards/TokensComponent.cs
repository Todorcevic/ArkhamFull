using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TokensComponent : MonoBehaviour
{
    [SerializeField] Image physicalImage;
    [SerializeField] Image mentalImage;
    [SerializeField] Image xpImage;
    [SerializeField] TextMeshProUGUI physicalText;
    [SerializeField] TextMeshProUGUI mentalText;
    [SerializeField] TextMeshProUGUI xpText;

    public void SetPhysical(int quantity)
    {
        physicalImage.gameObject.SetActive(quantity > 0);
        physicalText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }

    public void SetMental(int quantity)
    {
        mentalImage.gameObject.SetActive(quantity > 0);
        mentalText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }

    public void SetXp(int quantity)
    {
        xpImage.gameObject.SetActive(quantity > 0);
        xpText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }
}
