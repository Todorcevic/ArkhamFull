using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class PanelCampaignComponent : ControlPanelComponent, IButtonClickable
    {
        public List<CardComponent> VictoryCards { get; } = new List<CardComponent>();
        [SerializeField] List<InvestigatorTraumasComponent> investigatorTraumas;
        [SerializeField] List<Image> victoryCards;
        [SerializeField] TextMeshProUGUI campaignRegister;
        [SerializeField] AudioSource atmosAudioSource;

        /*****************************************************************************************/
        public void SetPanelCampaign()
        {
            PlayAudio();
            SetInvestigators();
        }

        void SetInvestigators()
        {
            investigatorTraumas.ForEach(i => i.gameObject.SetActive(false));
            int n = 0;
            foreach (InvestigatorComponent investigator in GameControl.AllInvestigators)
            {
                investigatorTraumas[n].gameObject.SetActive(true);
                investigatorTraumas[n].SetInvestigatorTraumas(investigator);
                n++;
            }
        }

        public void AddVictoryCard(CardComponent card, string text = null, int xp = 0)
        {
            VictoryCards.Add(card);
            Image victoryCard = victoryCards.Find(v => !v.gameObject.activeInHierarchy);
            victoryCard.gameObject.SetActive(true);
            victoryCard.sprite = AllComponents.CardBuilder.GetSprite(card.ID);
            if (text != null) RegisterText(text);
            if (xp > 0) GainXpForAll(xp);
        }

        public void RegisterText(string text) => campaignRegister.text += "* " + text + Environment.NewLine;

        public void GainXpForAll(int xp) => GameControl.AllInvestigators.ForEach(i => i.Xp += xp);

        public void SetButton() => SetReadyButton(this, state: ButtonState.Ready);

        public void ReadyClicked(ReadyButton button)
        {
            StopAudio();
            StartCoroutine(HideThisPanel());
        }
        void PlayAudio()
        {
            atmosAudioSource.volume = 0;
            atmosAudioSource.DOFade(1, GameData.ANIMATION_TIME_DEFAULT * 4);
            atmosAudioSource.Play();
        }

        void StopAudio()
        {
            atmosAudioSource.DOFade(0, GameData.ANIMATION_TIME_DEFAULT * 4).OnComplete(atmosAudioSource.Stop);
        }
    }
}