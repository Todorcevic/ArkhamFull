using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

namespace ArkhamGamePlay
{
    public abstract class InteractableCenterShow : InteractableAction
    {
        readonly protected Dictionary<Zone, ZoneBehaviour> realZone = new Dictionary<Zone, ZoneBehaviour>();
        public bool CardsInPreview { get; set; } = true;
        public List<CardEffect> ChosenCardEffects { get; set; } = new List<CardEffect>();

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            AllComponents.ShowHideChooseCard.ICenterShowableAction = this;
            yield return base.ActionLogic();
            AllComponents.ShowHideChooseCard.ICenterShowableAction = null;
        }

        public IEnumerator ShowPreviewCards()
        {
            GameControl.AllCardComponents.FindAll(c => c.IsInCenterPreview).ForEach(c => c.MoveFast(c.CurrentZone));
            yield return new WaitWhile(() => DOTween.IsTweening("MoveTo") || DOTween.IsTweening("MoveFast"));
            AllComponents.ShowHideChooseCard.SetSprite(false);
            realZone.Clear();
            ChosenCardEffects.ForEach(c => c.Card.Position = c.Card.transform.GetSiblingIndex());
            foreach (CardComponent card in ChosenCardEffects.FindAll(c => !c.Card.IsEliminated).Select(c => c.Card))
            {
                DOTween.Kill(card.transform);
                if (!realZone.ContainsKey(card.CurrentZone)) realZone.Add(card.CurrentZone, card.CurrentZone.ZoneBehaviour);
                PreparingCard(card, true);
                MoveToCenter(card);
                yield return null;
            }
            CardsInPreview = true;
            yield return new WaitWhile(() => DOTween.IsTweening("MoveFast"));
            AllComponents.Table.CenterPreview.ZoneBehaviour.PostCardMove();
        }

        public IEnumerator ShowTable()
        {
            yield return new WaitWhile(() => DOTween.IsTweening("MoveTo") || DOTween.IsTweening("MoveFast"));
            AllComponents.ShowHideChooseCard.SetSprite(true);
            foreach (CardComponent card in ChosenCardEffects.FindAll(c => !c.Card.IsEliminated).Select(c => c.Card).OrderBy(c => c.Position))
            {
                DOTween.Kill(card.transform);
                PreparingCard(card, false);
                MoveToOriginalzone(card);
                yield return null;
            }
            CardsInPreview = false;
            AllComponents.Table.CenterPreview.ZoneBehaviour.PreCardMove();
            yield return new WaitWhile(() => DOTween.IsTweening("MoveFast"));
            FinishingShowTable();
        }

        protected virtual void FinishingShowTable()
        {
            ChosenCardEffects.Select(c => c.Card).ToList().ForEach(c => c.CardTools.ShowCardsMyOwnZone(true));
            ChosenCardEffects.Select(c => c.Card.CurrentZone.ZoneBehaviour).Distinct().ToList().ForEach(c => c.PostCardMove());
        }

        protected virtual void MoveToCenter(CardComponent card)
        {
            card.CurrentZone.ZoneBehaviour = AllComponents.Table.CenterPreview.ZoneBehaviour;
            card.MoveFast(card.CurrentZone);
            AllComponents.Table.CenterPreview.AddCard(card);
        }

        protected void PreparingCard(CardComponent card, bool isPrepared)
        {
            card.CardTools.ShowCardsMyOwnZone(!isPrepared);
            if (isPrepared && !AnyIsClicked) card.CardTools.PrintCardActions(ChosenCardEffects);
            else card.CardTools.ShowInfoBox(string.Empty);
            card.CardTools.CardCanvas.sortingLayerName = isPrepared ? "Super" : "Card";
        }

        protected virtual void MoveToOriginalzone(CardComponent card)
        {
            card.CurrentZone.ZoneBehaviour = realZone[card.CurrentZone];
            card.MoveFast(card.CurrentZone, indexPosition: card.Position);
            card.Position = null;
        }
    }
}