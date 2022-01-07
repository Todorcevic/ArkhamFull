using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class CardTools : MonoBehaviour
    {
        [SerializeField] CardComponent cardComponent;
        [SerializeField] Transform rotationTransformCard;
        [SerializeField] Renderer rendFront;
        [SerializeField] Renderer rendBack;
        [SerializeField] BuffSprite buffSpritePrefab;
        [SerializeField] GameObject buffBox;
        [SerializeField] Canvas cardCanvas;
        [SerializeField] GameObject infoBox;
        [SerializeField] GameObject infoFront;
        [SerializeField] GameObject infoBack;
        [SerializeField] Image visualOwner;
        [SerializeField] Image visualRealOwner;
        [SerializeField] Image locationClues;
        [SerializeField] Image giveClues;
        [SerializeField] Image enemyLife;
        [SerializeField] Image shroud;
        [SerializeField] Image resourceCost;
        [SerializeField] Image investigatorStats;
        [SerializeField] TextMeshProUGUI locationCluesValue;
        [SerializeField] TextMeshProUGUI giveCluesValue;
        [SerializeField] TextMeshProUGUI enemyLifeValue;
        [SerializeField] TextMeshProUGUI shroudValue;
        [SerializeField] TextMeshProUGUI resourceCostValue;
        [SerializeField] TextMeshProUGUI investigatorWillpowerValue;
        [SerializeField] TextMeshProUGUI investigatorIntellectValue;
        [SerializeField] TextMeshProUGUI investigatorCombatValue;
        [SerializeField] TextMeshProUGUI investigatorAgilityValue;
        [SerializeField] TextMeshProUGUI infoEffect;
        [SerializeField] GameObject tokensBox;
        [SerializeField] MeshFilter cardMesh;
        [SerializeField] TokenComponent healthToken;
        [SerializeField] TokenComponent sanityToken;
        [SerializeField] Image glowImage;
        [SerializeField] Material active;
        [SerializeField] Material inactive;
        [SerializeField] Material selected;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioSource secundaryAudioSource;
        [SerializeField] AudioClip EnterCardClip;
        [SerializeField] AudioClip ExitCardClip;
        [SerializeField] AudioClip SelectCardClip;
        [SerializeField] AudioClip DeselectCardClip;
        [SerializeField] AudioClip PlayCardClip;
        [SerializeField] AudioClip ShowBuffClip;
        public AudioScriptableObject ClipsForType { get; set; }
        public AudioScriptableObject ClipsForCard { get; set; }
        public Renderer RendFront => rendFront;
        public Renderer RendBack => rendBack;
        public Material PreviewMaterial => cardComponent.IsBack ? rendBack.sharedMaterial : rendFront.sharedMaterial;
        public Transform RotationTransformCard => rotationTransformCard;
        public Canvas CardCanvas => cardCanvas;
        public GameObject TokensBox => tokensBox;
        public MeshFilter CardMesh => cardMesh;
        public GameObject InfoBox { get => infoBox; set => infoBox = value; }
        public Image GlowImage { get => glowImage; set => glowImage = value; }
        public GameObject BuffBox { get => buffBox; set => buffBox = value; }
        public GameObject InfoFront { get => infoFront; set => infoFront = value; }
        public GameObject InfoBack { get => infoBack; set => infoBack = value; }
        public Sprite SpriteOwner { get; set; }
        public AudioSource AudioSource => audioSource;
        public AudioSource SecundaryAudioSource => secundaryAudioSource;

        /*****************************************************************************************/
        public void HideCard(bool desactive) => gameObject.SetActive(!desactive);

        public void HideFrontCard(bool desactive) => rendFront.enabled = !desactive;

        public void ChangeGlowImage(Sprite sprite, Vector3 scale)
        {
            glowImage.sprite = sprite;
            glowImage.transform.localScale = scale;
        }

        public void ChangeColor(Color color, bool? isBack = null)
        {
            float timeAnimation = GameData.ANIMATION_TIME_DEFAULT / 2;
            if (isBack ?? false) rendBack.sharedMaterial.DOColor(color, timeAnimation);
            else if (!isBack ?? false) rendFront.sharedMaterial.DOColor(color, timeAnimation);
            else
            {
                rendFront.sharedMaterial.DOColor(color, timeAnimation);
                rendBack.sharedMaterial.DOColor(color, timeAnimation);
            }
        }

        public void ShowCardsMyOwnZone(bool show)
        {
            DOTween.Complete("ShowInMyOwn" + cardComponent.UniqueId);
            if (show)
                cardComponent.MyOwnZone?.ZoneBehaviour.transform.DOScale(0.5f, GameData.ANIMATION_TIME_DEFAULT * 2).SetEase(Ease.OutElastic, 1.1f).SetDelay(GameData.ANIMATION_TIME_DEFAULT).SetId("ShowInMyOwn" + cardComponent.UniqueId);
            else
                cardComponent.MyOwnZone?.ZoneBehaviour.transform.DOScale(0, GameData.ANIMATION_TIME_DEFAULT).SetId("ShowInMyOwn" + cardComponent.UniqueId);
        }

        public void ShowBuff(CardComponent card)
        {
            audioSource.PlayOneShot(ShowBuffClip);
            BuffSprite buffS = Instantiate(buffSpritePrefab, buffBox.transform);
            buffS.spriteBuff.sprite = AllComponents.CardBuilder.GetSprite(card.ID);
            buffS.id = card.UniqueId.ToString();
            buffS.selfCard = card;
            buffS.parentCard = cardComponent;
            DOTween.Sequence().Append(buffS.transform.DOScale(1.6f, GameData.ANIMATION_TIME_DEFAULT).SetEase(Ease.OutBounce))
                .Append(buffS.transform.DOScale(1f, GameData.ANIMATION_TIME_DEFAULT)).SetId("ShowBuff");
        }

        public void HideBuff(string id)
        {
            DOTween.Kill("ShowBuff");
            if (buffBox.GetComponentsInChildren<BuffSprite>().ToList().Exists(c => c.id == id))
                Destroy(buffBox.GetComponentsInChildren<BuffSprite>().ToList().Find(c => c.id == id).gameObject);
        }

        public void PrintCardActions(List<CardEffect> cardEffects)
        {
            string fullText = string.Empty;
            foreach (CardEffect cardEffect in cardEffects?.FindAll(c => c.Card == cardComponent))
                fullText += (cardEffect.TakeEffectTypeIcon() + cardEffect.Name + Environment.NewLine);
            CardEffect cardEf = cardEffects?.Find(c => c.Card == cardComponent);
            visualOwner.sprite = cardEf?.PlayOwner.PlayCard.CardTools.SpriteOwner;
            visualRealOwner.sprite = cardEf?.RealVisualOwner?.PlayCard.CardTools.SpriteOwner;
            visualRealOwner.gameObject.SetActive(visualRealOwner.sprite != null);
            ShowInfoBox(fullText);
        }


        public void ShowInfoBox(string text)
        {
            infoEffect.text = text;
            InfoBox.SetActive(text != string.Empty);
        }

        public CardComponent Clone() => Instantiate(gameObject, AllComponents.CardBuilder.transform).GetComponent<CardComponent>();

        public void Destroy()
        {
            cardComponent.CurrentZone?.ListCards.Remove(cardComponent);
            GameControl.AllCardComponents.Remove(cardComponent);
            DOTween.Kill(transform);
            Destroy(gameObject);
        }

        public int GetModifierValue(Skill skillType)
        {
            switch (skillType)
            {
                case Skill.Agility: return cardComponent.Agility + cardComponent.Wild;
                case Skill.Combat: return cardComponent.Combat + cardComponent.Wild;
                case Skill.Intellect: return cardComponent.Intellect + cardComponent.Wild;
                case Skill.Willpower: return cardComponent.Willpower + cardComponent.Wild;
                default: throw new ArgumentException("Wrong Skill name", skillType.ToString());
            }
        }

        public void ActiveInfo()
        {
            if (cardComponent.CardLogic is CardLocation cardLocation)
            {
                locationClues.gameObject.SetActive(true);
                locationCluesValue.text = cardLocation.Clues.ToString();
                shroud.gameObject.SetActive(true);
                UpdateShroud(cardLocation.Shroud);
            }

            if (cardComponent.CardLogic is CardAct cardAct)
            {
                giveClues.gameObject.SetActive(true);
                giveCluesValue.text = cardAct.CluesNeeded.ToString();
            }

            if (cardComponent.CardLogic is CardEnemy cardenemy)
            {
                enemyLife.gameObject.SetActive(true);
                enemyLifeValue.text = cardenemy.Health.ToString();
            }

            if (cardComponent.CardLogic is CardInvestigator && cardComponent.CardType != CardType.PlayCard)
            {
                investigatorStats.gameObject.SetActive(true);
                investigatorWillpowerValue.text = cardComponent.Info.Skill_willpower.ToString();
                investigatorIntellectValue.text = cardComponent.Info.Skill_intellect.ToString();
                investigatorCombatValue.text = cardComponent.Info.Skill_combat.ToString();
                investigatorAgilityValue.text = cardComponent.Info.Skill_agility.ToString();
            }
        }

        public void UpdateShroud(int value)
        {
            shroudValue.text = value.ToString();
            shroudValue.color = Color.white;
        }

        public void UpdateResourceCost(int value)
        {
            resourceCost.gameObject.SetActive(true);
            resourceCostValue.text = value.ToString();
        }

        public void HideResourceCost() => resourceCost.gameObject.SetActive(false);

        public void UpdateInvestigatorStatsInfo()
        {
            investigatorWillpowerValue.text = cardComponent.Owner.Willpower.ToString();
            investigatorWillpowerValue.color = Color.black;
            if (cardComponent.Owner.Willpower > cardComponent.Info.Skill_willpower) investigatorWillpowerValue.color = Color.green;
            if (cardComponent.Owner.Willpower < cardComponent.Info.Skill_willpower) investigatorWillpowerValue.color = Color.red;

            investigatorIntellectValue.text = cardComponent.Owner.Intellect.ToString();
            investigatorIntellectValue.color = Color.black;
            if (cardComponent.Owner.Intellect > cardComponent.Info.Skill_intellect) investigatorIntellectValue.color = Color.green;
            if (cardComponent.Owner.Intellect < cardComponent.Info.Skill_intellect) investigatorIntellectValue.color = Color.red;

            investigatorCombatValue.text = cardComponent.Owner.Combat.ToString();
            investigatorCombatValue.color = Color.black;
            if (cardComponent.Owner.Combat > cardComponent.Info.Skill_combat) investigatorCombatValue.color = Color.green;
            if (cardComponent.Owner.Combat < cardComponent.Info.Skill_combat) investigatorCombatValue.color = Color.red;

            investigatorAgilityValue.text = cardComponent.Owner.Agility.ToString();
            investigatorAgilityValue.color = Color.black;
            if (cardComponent.Owner.Agility > cardComponent.Info.Skill_agility) investigatorAgilityValue.color = Color.green;
            if (cardComponent.Owner.Agility < cardComponent.Info.Skill_agility) investigatorAgilityValue.color = Color.red;
        }

        public void CurrentGlow(CardState glowState)
        {
            switch (glowState)
            {
                case CardState.Inactive: { glowImage.material = inactive; break; }
                case CardState.Active:
                    {
                        glowImage.transform.DOScale(1.25f, 0);
                        glowImage.transform.DOScale(1.15f, GameData.ANIMATION_TIME_DEFAULT);
                        glowImage.material = active; break;
                    }
                case CardState.Selected:
                    {
                        glowImage.transform.DOScale(1f, 0);
                        glowImage.transform.DOScale(1.15f, GameData.ANIMATION_TIME_DEFAULT);
                        glowImage.material = selected; break;
                    }
            }
        }

        public void PlaySound(AudioClip audio, float volumne = 1, bool withLoop = false)
        {
            audioSource.volume = volumne;
            audioSource.loop = withLoop;
            audioSource.clip = audio;
            audioSource.Play();
        }

        public void PlaySoundEnterCard() => audioSource.PlayOneShot(EnterCardClip);
        public void PlaySoundExitCard() => audioSource.PlayOneShot(ExitCardClip);
        public void PlaySoundSelectCard() => audioSource.PlayOneShot(SelectCardClip);
        public void PlaySoundDeselectCard() => audioSource.PlayOneShot(DeselectCardClip);
        public void PlaySoundCard() => audioSource.PlayOneShot(PlayCardClip);
        public void PlayOneShotSound(AudioClip audio)
        {
            if (audio != null) audioSource.PlayOneShot(audio);
        }
        public void StopSound() => audioSource.Stop();
    }
}