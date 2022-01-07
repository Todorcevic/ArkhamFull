using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class PanelHistoryComponent : ControlPanelComponent
    {
        History[] historyList;
        [SerializeField] TextMeshProUGUI titleHistory;
        [SerializeField] TextMeshProUGUI textHistory;
        [SerializeField] Image imageHistory;
        [SerializeField] Sprite[] spritesHistory;
        [SerializeField] AudioSource atmosAudioSource;
        [SerializeField] List<AudioClip> audioClips;

        /*****************************************************************************************/
        public void LoadHistories() =>
            historyList = JsonDataManager.CreateListFromJson<History[]>(GameFiles.HistoriesPath);

        public void SetPanel(string idHistory)
        {
            History thisHistory = Array.Find(historyList, c => c.id == idHistory);
            titleHistory.text = thisHistory.title;
            textHistory.text = thisHistory.text;
            imageHistory.sprite = Array.Find(spritesHistory, c => c.name == thisHistory.image);
        }

        public void PlayAtmos(string idHistory)
        {
            DOTween.Kill("StopAtmos");
            atmosAudioSource.clip = audioClips.Find(a => a.name == idHistory);
            atmosAudioSource.loop = true;
            atmosAudioSource.volume = 0;
            atmosAudioSource.DOFade(1, GameData.ANIMATION_TIME_DEFAULT * 4);
            atmosAudioSource.Play();
        }

        public void StopAtmos()
        {
            atmosAudioSource.DOFade(0, GameData.ANIMATION_TIME_DEFAULT * 4).OnComplete(atmosAudioSource.Stop).SetId("StopAtmos");
            atmosAudioSource.loop = false;
        }
    }
}