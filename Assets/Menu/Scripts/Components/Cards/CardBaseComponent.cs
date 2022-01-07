using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Zenject;
using System.Linq;
using ArkhamShared;

namespace ArkhamMenu
{
    public class CardBaseComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] protected Image auraComponent;
        [SerializeField] Image imageComponent;
        [SerializeField] protected TextMeshProUGUI quantityText;
        [SerializeField] TokensComponent tokensComponent;
        [SerializeField] TextMeshProUGUI killed;
        [SerializeField] TextMeshProUGUI insane;
        [SerializeField] TextMeshProUGUI retired;
        public string NameCard { get; set; }
        public bool IsInvestigator { get; set; }
        [Inject] public DeckBuildManager DeckBuildManager { get; set; }
        public InvestigatorData InvestigatorData => InvestigatorData.AllInvestigatorsData.Find(i => i.Id == ID);
        public InvestigatorSelectorComponent InvestigatorSelector { get; set; }
        public Sprite ImageBack { get; set; }
        public Image ImageComponent => imageComponent;
        public Card Info => Card.DataCardDictionary[ID];

        string id;
        public virtual string ID
        {
            get => id;
            set
            {
                id = value;
                SetNameCard(value);
                SetImageCard(value);

                void SetNameCard(string idCard)
                {
                    gameObject.name = idCard;
                    NameCard = Card.DataCardDictionary[idCard].Name;
                }

                void SetImageCard(string idCard)
                {
                    ImageComponent.sprite = ImageLoader.cardsImage.Find(img => img.name == idCard);
                    ImageBack = ImageLoader.cardsImage.Find(img => img.name == idCard + "b");
                }
            }
        }

        public virtual int Quantity
        {
            get
            {
                int nCards = 0;
                foreach (InvestigatorData invD in InvestigatorData.AllInvestigatorsData)
                    nCards += invD.ListCardsID.FindAll(s => s.Contains(ID)).Count;
                nCards += InvestigatorSelector.ListActiveInvCard.FindAll(i => i.ID == ID).Count;
                return (Info.Quantity ?? 0) - nCards;
            }
        }

        protected bool isSelected;
        public virtual bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value) return;
                isSelected = auraComponent.enabled = value ? true : false;
            }
        }

        protected bool isDisable;
        public virtual bool IsDisable
        {
            get => isDisable;
            set
            {
                if (isDisable == value) return;
                ImageComponent.color = value ? new Color(0.789f, 0.789f, 0.789f, 0.5f) : Color.white;
                isDisable = value;
            }
        }

        protected bool isVisible;
        public virtual bool IsVisible
        {
            get => isVisible;
            set
            {
                if (isVisible == value) return;
                gameObject.SetActive(value);
                isVisible = value;
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            IsSelected = true;
            DeckBuildManager.ShowingPreviewCard(this);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            IsSelected = false;
            DeckBuildManager.HidePreviewCard(this);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!IsDisable && DoubleClickCheck.DoubleClicked(eventData.clickTime, eventData.pointerPress))
                DoubleClickCard(this);
        }

        public virtual void DoubleClickCard(CardBaseComponent cardBase)
        {
            if (IsInvestigator)
                DeckBuildManager.ChooseInvestigatorCard(this);
            else
                DeckBuildManager.ChooseCard(this);
        }

        public void PrintQuantity() => quantityText.text = Quantity < 2 ? "" : "x" + Quantity;

        public void SetTokens(bool isRemove = false)
        {
            tokensComponent.SetPhysical(isRemove ? 0 : InvestigatorData?.PhysicalTrauma ?? 0);
            tokensComponent.SetMental(isRemove ? 0 : InvestigatorData?.MentalTrauma ?? 0);
            tokensComponent.SetXp(isRemove ? 0 : InvestigatorData?.Xp ?? 0);
        }

        public void CheckCanPlay()
        {
            killed.gameObject.SetActive(InvestigatorData?.IsKilled ?? false);
            insane.gameObject.SetActive(InvestigatorData?.IsInsane ?? false);
            retired.gameObject.SetActive(InvestigatorData?.IsRetired ?? false);
        }
    }
}
