using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArkhamGamePlay
{
    public class InvestigatorTraumasComponent : MonoBehaviour
    {
        InvestigatorComponent investigator;
        [SerializeField] Image investigatorImage;
        [SerializeField] List<GameObject> physicalTraumas;
        [SerializeField] List<GameObject> mentalTraumas;
        [SerializeField] List<GameObject> experience;
        [SerializeField] TextMeshProUGUI isKilledText;
        [SerializeField] TextMeshProUGUI isInsaneText;

        /*****************************************************************************************/
        public void SetInvestigatorTraumas(InvestigatorComponent investigator)
        {
            isKilledText.gameObject.SetActive(investigator.IsKilled);
            isInsaneText.gameObject.SetActive(investigator.IsInsane);
            this.investigator = investigator;
            investigatorImage.sprite = AllComponents.CardBuilder.GetSprite(investigator.IdInvestigator);
            ShowPhysicalTrauma();
            ShowMentalTrauma();
            ShowExperience();
        }

        void ShowPhysicalTrauma()
        {
            physicalTraumas.ForEach(p => p.gameObject.SetActive(false));
            for (int i = 0; i < investigator.PhysicalTraumas; i++)
                if (physicalTraumas.Count > i) physicalTraumas[i].SetActive(true);
        }

        void ShowMentalTrauma()
        {
            mentalTraumas.ForEach(m => m.gameObject.SetActive(false));
            for (int i = 0; i < investigator.MentalTraumas; i++)
                if (mentalTraumas.Count > i) mentalTraumas[i].SetActive(true);
        }

        void ShowExperience()
        {
            experience.ForEach(x => x.gameObject.SetActive(false));
            for (int i = 0; i < investigator.Xp; i++)
                if (experience.Count > i) experience[i].SetActive(true);
        }
    }
}