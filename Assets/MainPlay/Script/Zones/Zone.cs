using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class Zone
    {
        public Zones ZoneType { get; }
        public List<CardComponent> ListCards { get; } = new List<CardComponent>();
        public ZoneBehaviour ZoneBehaviour { get; set; }
        public CardComponent ThisCard { get; set; }
        public InvestigatorComponent InvestigatorOwer => GameControl.AllInvestigatorsInGame.Find(i => i.InvestigatorZones.Contains(this));

        /*****************************************************************************************/
        public Zone(Zones typeZone) => ZoneType = typeZone;

        /*****************************************************************************************/
        public void AddCard(CardComponent card) => ListCards.Add(card);
        public void InsertCard(CardComponent card, int indexPosition) => ListCards.Insert(indexPosition, card);
        public void RemoveCard(CardComponent card) => ListCards.Remove(card);

        public IEnumerator Shuffle(float timeAnimation = GameData.ANIMATION_TIME_DEFAULT)
        {
            ZoneBehaviour.AudioSource.clip = AllComponents.AudioComponent.LoadClips("MoveDeck").clip2;
            ZoneBehaviour.AudioSource.loop = true;
            ZoneBehaviour.AudioSource.Play();
            ListCards.Shuffle();
            for (int i = 0; i < ListCards.Count; i++)
            {
                ListCards[i].transform.SetSiblingIndex(i);
                yield return ListCards[i].transform.DOShakeRotation(timeAnimation).SetId("Shuffle");
                yield return DOTween.Sequence().Append(ListCards[i].transform.DOLocalMoveX((UnityEngine.Random.value - 0.5f) * 0.5f, timeAnimation / 2))
                    .Append(ListCards[i].transform.DOLocalMoveX(0, timeAnimation / 2));
                yield return ListCards[i].transform.DOLocalMoveY(GameData.CARD_THICK * i, timeAnimation).SetId("Shuffle");
            }
            yield return new WaitWhile(() => DOTween.IsTweening("Shuffle"));
            ZoneBehaviour.AudioSource.Stop();
            ZoneBehaviour.AudioSource.loop = false;
            ZoneBehaviour.PreCardMove();
            ZoneBehaviour.PostCardMove();
        }

        public IEnumerator MoveToPosition(CardComponent card, int position)
        {
            ListCards.Remove(card);
            ListCards.Insert(position, card);
            card.transform.SetSiblingIndex(position);
            float originalX = card.transform.localPosition.x;
            yield return card.transform.DOLocalMoveX(-1, GameData.ANIMATION_TIME_DEFAULT).SetEase(Ease.InOutCubic);
            for (int i = 0; i < ListCards.Count; i++)
                yield return ListCards[i].transform.DOLocalMoveY(GameData.CARD_THICK * i, GameData.ANIMATION_TIME_DEFAULT / 4);
            yield return new WaitWhile(() => DOTween.IsTweening(card.transform));
            yield return card.transform.DOLocalMoveX(originalX, GameData.ANIMATION_TIME_DEFAULT).SetEase(Ease.InOutCubic).WaitForCompletion();
            ZoneBehaviour.PreCardMove();
            ZoneBehaviour.PostCardMove();
        }
    }
}