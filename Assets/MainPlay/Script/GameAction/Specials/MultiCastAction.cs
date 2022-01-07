using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class MultiCastAction : GameAction
    {
        readonly bool isOptionalChoice;
        ChooseMultiCast chooseMultiCast;
        List<CardComponent> cardsToDestroy = new List<CardComponent>();
        public override GameActionType GameActionType => GameActionType.Compound;
        public List<CardEffect> ListCardsEffect { get; set; }
        public bool IsCancel { get; set; }

        /*****************************************************************************************/
        public MultiCastAction(List<CardEffect> cardEffects, bool isOptionalChoice = true)
        {
            ListCardsEffect = cardEffects;
            this.isOptionalChoice = isOptionalChoice;
            foreach (CardEffect effectStruct in ListCardsEffect)
            {
                CardComponent cloneCard = effectStruct.Card.CardTools.Clone();
                cloneCard.ID = effectStruct.Card.ID;
                cloneCard.CardLogic = new CardLogic().WithThisCard(cloneCard);
                cloneCard.CurrentZone = effectStruct.Card.MyOwnZone;
                cloneCard.transform.position = effectStruct.Card.transform.position;
                cloneCard.transform.rotation = effectStruct.Card.transform.rotation;
                cloneCard.CardSensor.StackerZone.transform.DOScale(0, 0);
                effectStruct.Card = cloneCard;
                effectStruct.Effect = new ActionsTools().JoinEffects(Destroying, effectStruct.Effect);
                GameControl.AllCardComponents.Add(cloneCard);
                cardsToDestroy.Add(cloneCard);
            }
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            chooseMultiCast = new ChooseMultiCast(ListCardsEffect, isOptionalChoice: isOptionalChoice);
            yield return chooseMultiCast.RunNow();
            yield return new WaitWhile(() => DOTween.IsTweening("MoveFast"));
            IsCancel = chooseMultiCast.IsCancel;
            yield return Destroying();
        }

        IEnumerator Destroying()
        {
            if (chooseMultiCast.CardPlayed != null)
                yield return chooseMultiCast.CardPlayed.MoveTo(AllComponents.CardBuilder.Zone, timeAnimation: 0f).WaitForCompletion();
            yield return new WaitWhile(() => DOTween.IsTweening("HorizontalOrder"));
            foreach (CardComponent card in cardsToDestroy)
                card.CardTools.Destroy();
            cardsToDestroy.Clear();
        }
    }
}