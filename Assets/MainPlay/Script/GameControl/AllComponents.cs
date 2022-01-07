using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class AllComponents : MonoBehaviour
    {
        [SerializeField] Transform _centerPreview;
        public static Transform CenterPreview;

        [SerializeField] ReadyButton _readyButton;
        public static ReadyButton ReadyButton;

        [SerializeField] PanelSkillTestComponent _panelSkillTest;
        public static PanelSkillTestComponent PanelSkillTest;

        [SerializeField] PanelHistoryComponent _panelHistory;
        public static PanelHistoryComponent PanelHistory;

        [SerializeField] PanelCampaignComponent _panelCampaign;
        public static PanelCampaignComponent PanelCampaign;

        [SerializeField] TableComponent _table;
        public static TableComponent Table;

        [SerializeField] CardBuilder _cardBuilder;
        public static CardBuilder CardBuilder;

        [SerializeField] TokenStack _tokenStacks;
        public static TokenStack TokenStacks;

        [SerializeField] ChaosBagComponent _chaosBag;
        public static ChaosBagComponent ChaosBag;

        [SerializeField] InvestigatorManagerComponent _investigatorManagerComponent;
        public static InvestigatorManagerComponent InvestigatorManagerComponent;

        [SerializeField] PhasesUI _phasesUI;
        public static PhasesUI PhasesUI;

        [SerializeField] ShowHideChooseCard _showHideChooseCard;
        public static ShowHideChooseCard ShowHideChooseCard;

        [SerializeField] AudioComponent _audioComponent;
        public static AudioComponent AudioComponent;

        public void BuildingComponents()
        {
            CenterPreview = _centerPreview;
            ReadyButton = _readyButton;
            PanelSkillTest = _panelSkillTest;
            PanelHistory = _panelHistory;
            PanelCampaign = _panelCampaign;
            Table = _table;
            CardBuilder = _cardBuilder;
            CardBuilder.Zone = new Zone(Zones.CardBuilder) { ZoneBehaviour = CardBuilder.ZoneBehaviour };
            TokenStacks = _tokenStacks;
            ChaosBag = _chaosBag;
            InvestigatorManagerComponent = _investigatorManagerComponent;
            PhasesUI = _phasesUI;
            ShowHideChooseCard = _showHideChooseCard;
            AudioComponent = _audioComponent;
        }
    }
}