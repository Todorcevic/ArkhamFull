using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;
using ArkhamMenu;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class InvestigatorManagerComponent : MonoBehaviour
    {
        [SerializeField] InvestigatorComponent investigatorPrefab;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip changingInvestigator;

        public void BuildInvestigators()
        {
            GameControl.AllInvestigatorsInGame = new List<InvestigatorComponent>();
            GameControl.InvestigatorsDefeat = new List<InvestigatorComponent>();
            JsonDataManager.LoadInvestigatorsData();
            List<InvestigatorData> playingInvestigators = InvestigatorData.AllInvestigatorsData.FindAll(i => i.DeckPosition > 0);
            GameControl.InvestigatorsStartingAmount = playingInvestigators.Count;
            foreach (InvestigatorData investigator in playingInvestigators.OrderBy(i => i.DeckPosition))
                BuildOneInvestigator(investigator.Id);
            StartCoroutine(SelectInvestigator(GameControl.LeadInvestigator));
        }

        public void SelectInvestigatorCoroutine(InvestigatorComponent investigator) => StartCoroutine(SelectInvestigator(investigator));

        void BuildOneInvestigator(string investigatorId)
        {
            InvestigatorComponent investigatorComponent = Instantiate(investigatorPrefab, transform);
            investigatorComponent.IdInvestigator = investigatorComponent.gameObject.name = investigatorId;
            investigatorComponent.BuildingCardsInvestigator();
            GameControl.AllInvestigatorsInGame.Add(investigatorComponent);
        }

        public IEnumerator SelectInvestigator(InvestigatorComponent investigator)
        {
            if (investigator != GameControl.CurrentInvestigator)
            {
                DOTween.Kill("SelectInvestigator");
                DOTween.Complete("StackOrder");
                if (GameControl.CurrentInvestigator != null)
                    yield return RemoveCardsInTable();
                GameControl.CurrentInvestigator = investigator;
                yield return MoveCardsToTable(investigator);
            }
        }

        IEnumerator MoveCardsToTable(InvestigatorComponent investigator)
        {
            audioSource.PlayOneShot(changingInvestigator);
            for (int i = 0; i < investigator.InvestigatorZones.Length; i++)
            {
                investigator.InvestigatorZones[i].ZoneBehaviour = AllComponents.Table.InvestigatorZonesBehaviour[i];
                foreach (CardComponent card in investigator.InvestigatorZones[i].ListCards.ToArray())
                    if (!card.IsInCenterPreview)
                        card.MoveFast(investigator.InvestigatorZones[i], withAudio: null).SetId("SelectInvestigator");
            }
            yield return new WaitWhile(() => DOTween.IsTweening("SelectInvestigator"));
            foreach (Zone zone in investigator.InvestigatorZones)
                zone.ZoneBehaviour.PostCardMove();
            yield return new WaitWhile(() => DOTween.IsTweening("HorizontalOrder"));
        }

        IEnumerator RemoveCardsInTable()
        {
            foreach (Zone zone in GameControl.CurrentInvestigator.InvestigatorZones)
            {
                zone.ZoneBehaviour = AllComponents.CardBuilder.ZoneBehaviour;
                foreach (CardComponent card in zone.ListCards.ToArray())
                    if (!card.IsInCenterPreview)
                        card.MoveFast(zone, withAudio: null).SetId("SelectInvestigator");
            }
            yield return null;
        }
    }
}