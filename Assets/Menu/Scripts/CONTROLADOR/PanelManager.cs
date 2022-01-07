using UnityEngine;
using Michsky.UI.Dark;
using UnityEngine.UI;
using Zenject;
using System.Linq;
using ArkhamShared;

namespace ArkhamMenu
{
    public class PanelManager : MonoBehaviour
    {
        ViewCardManager viewCardManager;
        InvestigatorSelectorComponent investigatorSelector;
        [SerializeField] CheckReady checkReady;
        [SerializeField] Button investigatorButton;
        public static bool IsInInvestigatorPanel { get; set; }

        /********************************************************************************/
        [Inject]
        public void Constructor(ViewCardManager viewCardManager, InvestigatorSelectorComponent investigatorSelector)
        {
            this.viewCardManager = viewCardManager;
            this.investigatorSelector = investigatorSelector;
        }

        /********************************************************************************/
        public void ContinueGame()
        {
            JsonDataManager.LoadInvestigatorsData();
            RefreshDatas();
            GoToInvestigatorPanel();
        }

        void RefreshDatas()
        {
            foreach (InvestigatorData inv in InvestigatorData.AllInvestigatorsData.FindAll(i => i.DeckPosition > 0 && i.CanPlay).OrderBy(i => i.DeckPosition))
                investigatorSelector.AddInvestigator(inv.Id);
            investigatorSelector.CurrentInvestigator = investigatorSelector.ListActiveInvCard[0];
            viewCardManager.ShowListSelectedCards(investigatorSelector.CurrentInvestigator);
        }

        public void RestartingInvestigator()
        {
            viewCardManager.CleanSelectedCards();
            investigatorSelector.CleanAllInvCard();
        }

        public void GoToInvestigatorPanel()
        {
            IsInInvestigatorPanel = true;
            GetComponent<PanelTabManager>().PanelAnim(0);
            viewCardManager.ShowInvestigatorCards();
            SettingButtons(false);
        }

        public void GoToDeckPanel()
        {
            IsInInvestigatorPanel = false;
            investigatorSelector.CurrentInvestigator = investigatorSelector.GetComponentInChildren<InvNCardComponent>(); //Select the first InvCard
            GetComponent<PanelTabManager>().PanelAnim(1);
            viewCardManager.ShowDeckCards(investigatorSelector.CurrentInvestigator);
            SettingButtons(true);
        }

        public void SettingButtons(bool state)
        {
            checkReady.CheckingAllReady();
            investigatorButton.SwitchON(state);
        }
    }
}