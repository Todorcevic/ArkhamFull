using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ArkhamMenu;
using Michsky.UI.Dark;

public class RetireInvestigatorModal : MonoBehaviour
{
    InvNCardComponent investigator;
    [SerializeField] ModalWindowManager modalWindows;
    [SerializeField] InvestigatorSelectorComponent investigatorSelector;
    [SerializeField] ViewCardManager viewCardManager;
    [SerializeField] TextMeshProUGUI description;

    public void SetInvestigator(InvNCardComponent investigator)
    {
        this.investigator = investigator;
        description.text = "¿Deseas retirar a " + investigator.Info.Name + "?";
        modalWindows.ModalWindowIn();
    }

    public void YeapButton()
    {
        investigatorSelector.RetireInvestigator(investigator);
    }
}
