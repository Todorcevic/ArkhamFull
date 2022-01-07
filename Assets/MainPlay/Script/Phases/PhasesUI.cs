using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class PhasesUI : MonoBehaviour
    {
        readonly float delay = GameData.ANIMATION_TIME_DEFAULT * 4;
        readonly float textDisplacement = 200;
        [SerializeField] Transform showCenter;
        [SerializeField] Transform big;
        [SerializeField] Transform mid;
        [SerializeField] Transform small;
        [SerializeField] Phase mythos;
        [SerializeField] Phase investigation;
        [SerializeField] Phase enemy;
        [SerializeField] Phase upkeep;
        [SerializeField] TextMeshProUGUI info;
        [SerializeField] List<Phase> corousel;
        [SerializeField] Image investigatorActive;
        [SerializeField] Sprite unknowInvestigator;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip ChangingPhase;
        [SerializeField] AudioClip Bong;
        [SerializeField] AudioClip Drop;
        [SerializeField] AudioClip Return;

        public Sequence PhaseSequence => DOTween.Sequence()
            .AppendCallback(() => audioSource.PlayOneShot(Drop))
            .Append(info.transform.DOLocalMoveY(0, GameData.ANIMATION_TIME_DEFAULT * 2.5f).SetEase(Ease.OutExpo))
            .AppendInterval(delay)
            .AppendCallback(() => audioSource.PlayOneShot(Return))
            .Append(info.transform.DOLocalMoveY(textDisplacement, GameData.ANIMATION_TIME_DEFAULT).SetEase(Ease.OutExpo))
            .SetId("ShowText");

        /*****************************************************************************************/
        public void SelectPhase(PhaseAction currentPhase)
        {
            if (currentPhase is MythosPhase) StartCoroutine(SelectingPhase(mythos));
            else if (currentPhase is InvestigationPhase) StartCoroutine(SelectingPhase(investigation));
            else if (currentPhase is EnemyPhase) StartCoroutine(SelectingPhase(enemy));
            else if (currentPhase is UpkeepPhase) StartCoroutine(SelectingPhase(upkeep));
        }

        public IEnumerator ShowInfo(string text)
        {
            if (text == info.text) yield break;
            investigatorActive.sprite = GameControl.ActiveInvestigator?.PlayCard.CardTools.SpriteOwner ?? unknowInvestigator;
            info.text = text;
            if (GameControl.CurrentAction is PhaseAction phaseAction)
            {
                investigatorActive.sprite = unknowInvestigator;
                yield return PhaseSequence.SetDelay(GameData.ANIMATION_TIME_DEFAULT * 2).Play().WaitForCompletion();
            }
        }

        public void SettingGamePhase()
        {
            corousel[0].Moving(big);
            corousel[1].Moving(mid);
            corousel[2].Moving(small);
        }

        void Animation()
        {
            audioSource.PlayOneShot(ChangingPhase);
            corousel[0].transform.DOScale(0, GameData.INTERACTIVE_TIME_DEFAULT);
            corousel[1].Moving(big);
            corousel[2].Moving(mid);
            corousel[3].Moving(small);
            MovingTransform();
        }

        void MovingTransform()
        {
            corousel.Add(corousel[0]);
            corousel.RemoveAt(0);
            corousel[0].transform.SetSiblingIndex(corousel.Count - 1);
        }

        IEnumerator SelectingPhase(Phase phase)
        {
            while (corousel[0] != phase)
            {
                Animation();
                yield return new WaitWhile(() => DOTween.IsTweening("corousel"));
            }

            DOTween.Complete("ChangingPhase");
            yield return DOTween.Sequence()
                    .Append(phase.transform.DORotate(new Vector3(0, 360, 0), GameData.ANIMATION_TIME_DEFAULT * 4, mode: RotateMode.FastBeyond360))
                    .AppendCallback(() => audioSource.PlayOneShot(Bong))
                    .Append(phase.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), GameData.ANIMATION_TIME_DEFAULT * 4, vibrato: 5, elasticity: 0.25f).SetEase(Ease.InOutCubic))
                    .SetId("ChangingPhase");
        }
    }
}