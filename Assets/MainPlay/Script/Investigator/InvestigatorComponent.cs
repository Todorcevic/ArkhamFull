using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using System;
using TMPro;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class InvestigatorComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        bool isExausted;
        int intellectBonus;
        int combatBonus;
        int agilityBonus;
        int willpowerBonus;

        [Header("Settings")]
        [SerializeField] Image imageCard;
        [SerializeField] Image selectGlow;
        [SerializeField] Image selectableGlow;
        [SerializeField] TextMeshProUGUI textInfo;

        [SerializeField] Zone investigatorZone;
        [SerializeField] Zone hand;
        [SerializeField] Zone assets;
        [SerializeField] Zone threat;
        [SerializeField] Zone investigatorDeck;
        [SerializeField] Zone investigatorDiscard;
        [SerializeField] Zone[] investigatorZones;

        public InvestigatorData InvestigatorData => InvestigatorData.AllInvestigatorsData.Find(i => i.Id == IdInvestigator);
        public List<string> FullListCards => InvestigatorData.FullListCards;
        public List<string> ListCardsID => InvestigatorData.ListCardsID;
        public List<string> CardRequeriments => InvestigatorData.CardRequeriments;
        public string IdInvestigator { get; set; }
        public bool IsExausted
        {
            get => isExausted;
            set
            {
                if (isExausted == value) return;
                isExausted = value;
                imageCard.color = isExausted ? GameData.ExaustedColor : Color.white;
            }
        }
        public List<CardComponent> AllEnemiesInMyLocation => GameControl.AllCardComponents.FindAll(c => c.CardLogic is CardEnemy cardEnemy && cardEnemy.CurrentLocation != null && cardEnemy.CurrentLocation == CurrentLocation);
        public List<CardComponent> AllEnemiesEnganged => Threat.ListCards.FindAll(c => c.CardType == CardType.Enemy);
        public List<CardComponent> ListCardComponent { get; set; } = new List<CardComponent>();
        public CardComponent PlayCard { get; set; }
        public CardComponent InvestigatorCardComponent { get; set; }
        public CardComponent CurrentLocation { get => PlayCard.CurrentZone.ThisCard; }
        public TextMeshProUGUI TextInfo { get => textInfo; set => textInfo = value; }
        public bool IsEnganged => Threat.ListCards.Exists(c => c.CardType == CardType.Enemy);
        public bool IsEngangedAndReady => Threat.ListCards.Exists(c => c.CardType == CardType.Enemy && !c.IsExausted);
        public bool IsDefeat => !GameControl.AllInvestigatorsInGame.Contains(this) && GameControl.GameIsStarted;
        public bool IsResign { get; set; }
        public bool IsBeingEliminated { get; set; }
        public bool IsKilled => PhysicalTraumas >= InvestigatorCardComponent.Info.Health;
        public bool IsInsane => MentalTraumas >= InvestigatorCardComponent.Info.Sanity;
        public bool IsDie => IsKilled || IsInsane;
        public int ActionsLeft { get; set; }
        public int InitialActions { get; set; } = 3;
        public int Damage => InvestigatorCardComponent.HealthToken.Amount;
        public int Horror => InvestigatorCardComponent.SanityToken.Amount;
        public int Health => (int)InvestigatorCardComponent.Info.Health;
        public int Sanity => (int)InvestigatorCardComponent.Info.Sanity;
        public int Resources => InvestigatorCardComponent.ResourcesToken.Amount;
        public int Clues => InvestigatorCardComponent.CluesToken.Amount;
        public int Doom => InvestigatorCardComponent.DoomToken.Amount;
        public int Intellect => ((int)InvestigatorCardComponent.Info.Skill_intellect + intellectBonus);
        public int Combat => (int)InvestigatorCardComponent.Info.Skill_combat + combatBonus;
        public int Willpower => (int)InvestigatorCardComponent.Info.Skill_willpower + willpowerBonus;
        public int Agility => (int)InvestigatorCardComponent.Info.Skill_agility + agilityBonus;
        public int AttackDamage { get; set; } = 1;
        public int PhysicalTraumas
        {
            get => InvestigatorData.PhysicalTrauma;
            set => InvestigatorData.PhysicalTrauma = value;
        }
        public int MentalTraumas
        {
            get => InvestigatorData.MentalTrauma;
            set => InvestigatorData.MentalTrauma = value;
        }
        public int Xp
        {
            get => InvestigatorData.Xp;
            set => InvestigatorData.Xp = value;
        }
        public string Name => InvestigatorCardComponent.Info.Name;
        public Zone InvestigatorZone { get => investigatorZone; set => investigatorZone = value; }
        public Zone Hand { get => hand; set => hand = value; }
        public Zone Assets { get => assets; set => assets = value; }
        public Zone Threat { get => threat; set => threat = value; }
        public Zone InvestigatorDeck { get => investigatorDeck; set => investigatorDeck = value; }
        public Zone InvestigatorDiscard { get => investigatorDiscard; set => investigatorDiscard = value; }
        public Zone[] InvestigatorZones { get => investigatorZones; set => investigatorZones = value; }
        public Image SelectGlow => selectGlow;
        public Image SelectableGlow => selectableGlow;
        public int IntellectBonus
        {
            get => intellectBonus;
            set
            {
                intellectBonus = value;
                InvestigatorCardComponent.CardTools.UpdateInvestigatorStatsInfo();
                UpdateSkillPanel();
            }
        }
        public int CombatBonus
        {
            get => combatBonus;
            set
            {
                combatBonus = value;
                InvestigatorCardComponent.CardTools.UpdateInvestigatorStatsInfo();
                UpdateSkillPanel();
            }
        }
        public int AgilityBonus
        {
            get => agilityBonus;
            set
            {
                agilityBonus = value;
                InvestigatorCardComponent.CardTools.UpdateInvestigatorStatsInfo();
                UpdateSkillPanel();
            }
        }
        public int WillpowerBonus
        {
            get => willpowerBonus;
            set
            {
                willpowerBonus = value;
                InvestigatorCardComponent.CardTools.UpdateInvestigatorStatsInfo();
                UpdateSkillPanel();
            }
        }

        public InvestigatorSlots Slots { get; private set; }

        /************************************************************************************************/
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!GameControl.FlowIsRunning && !DOTween.IsTweening("SelectInvestigator") && GameControl.GameIsStarted)
            {
                transform.DOScale(1.1f, GameData.INTERACTIVE_TIME_DEFAULT);
                InvestigatorCardComponent.CardSensor.CurrentBehaviourZone.PointEnterCardAnimation(InvestigatorCardComponent);
                GameControl.CurrentInvestigator.InvestigatorCardComponent.CardSensor.CurrentBehaviourZone.PointEnterPreview(InvestigatorCardComponent);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!GameControl.FlowIsRunning && !DOTween.IsTweening("SelectInvestigator") && GameControl.GameIsStarted)
            {
                transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
                InvestigatorCardComponent.CardSensor.CurrentBehaviourZone.PointExitCardAnimation(InvestigatorCardComponent);
                GameControl.CurrentInvestigator.InvestigatorCardComponent.CardSensor.CurrentBehaviourZone.PointExitPreview(InvestigatorCardComponent);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!GameControl.FlowIsRunning && !DOTween.IsTweening("SelectInvestigator"))
            {
                if (GameControl.GameIsStarted)
                    GameControl.CurrentInvestigator.InvestigatorCardComponent.CardSensor.CurrentBehaviourZone.PointExitPreview(InvestigatorCardComponent);
                StartCoroutine(AllComponents.InvestigatorManagerComponent.SelectInvestigator(this));
            }
        }

        public void BuildingCardsInvestigator()
        {
            imageCard.enabled = true;
            imageCard.sprite = AllComponents.CardBuilder.GetSprite(IdInvestigator);
            Slots = new InvestigatorSlots(this);
            BuildingListZones();
            BuildingCardComponents();
        }

        void BuildingListZones()
        {
            InvestigatorZones = new Zone[]
            {
            InvestigatorZone = new Zone (Zones.Investigator),
            Hand = new Zone (Zones.Hand),
            Assets = new Zone (Zones.Assets),
            Threat = new Zone (Zones.Threat),
            InvestigatorDeck = new Zone (Zones.InvestigatorDeck),
            InvestigatorDiscard = new Zone (Zones.InvestigatorDiscard)
            };
        }

        void BuildingCardComponents()
        {
            PlayCard = AllComponents.CardBuilder.BuildPlayCard(IdInvestigator);
            InvestigatorCardComponent = AllComponents.CardBuilder.BuildCard(IdInvestigator, IdInvestigator + "b");
            InvestigatorCardComponent.Owner = PlayCard.Owner = this;
            foreach (string cardId in FullListCards)
            {
                CardComponent card = AllComponents.CardBuilder.BuildCard(cardId, GameFiles.INVESTIGATOR_BACK_IMAGE);
                card.Owner = this;
                ListCardComponent.Add(card);
            }
        }

        public List<CardComponent> ListingCardsInMyZone()
        {
            List<CardComponent> cardsInMyZones = new List<CardComponent>();
            foreach (Zone zone in InvestigatorZones)
                cardsInMyZones.AddRange(zone.ListCards);
            return cardsInMyZones;
        }

        public bool CheckIsSelectable() =>
            SelectableGlow.enabled = ListingCardsInMyZone().Exists(card => card.CanBePlayedNow);

        public void ShowInfo(string info = null)
        {
            TextInfo.text = info;
            TextInfo.enabled = info != null;
        }

        void UpdateSkillPanel()
        {
            if (GameControl.CurrentSkillTestAction != null) AllComponents.PanelSkillTest.UpdatePanel();
        }

        public void AddCard(CardComponent card)
        {
            ListCardComponent.Add(card);
            InvestigatorData.CardRequeriments.Add(card.ID);
        }

        public void RemoveCard(CardComponent card)
        {
            ListCardComponent.Remove(card);
            InvestigatorData.CardRequeriments.Remove(card.ID);
        }
    }
}