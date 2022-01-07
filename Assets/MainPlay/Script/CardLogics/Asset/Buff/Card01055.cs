using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01055 : CardAsset, IBuffable
    {
        CardEffect evadeAll;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckPlayCardInGame(investigatorTurn))
                investigatorTurn.CardEffects.Add(evadeAll = new CardEffect(
                    card: ThisCard,
                    effect: EvadeAll,
                    animationEffect: EvadeAllAnimation,
                    type: EffectType.Activate,
                    name: "Activar " + ThisCard.Info.Name,
                    actionCost: 1,
                    needExhaust: true,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
            if (gameAction is OportunityAttack oportunityAttack && CheckOportunityAttack(oportunityAttack))
                oportunityAttack.IsActionCanceled = true;
        }

        bool CheckPlayCardInGame(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.VisualOwner != GameControl.ActiveInvestigator) return false;
            return true;
        }

        bool CheckOportunityAttack(OportunityAttack oportunityAttack)
        {
            if (oportunityAttack.CardEffect != evadeAll) return false;
            return true;
        }

        IEnumerator EvadeAllAnimation() =>
                new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator EvadeAll()
        {
            Zone zoneToStayEnemies = ThisCard.VisualOwner.CurrentLocation.MyOwnZone;
            LocationSymbol thisLocationSymbol = ((CardLocation)GameControl.ActiveInvestigator.CurrentLocation.CardLogic).MySymbol;
            List<CardEffect> chooseToMove = new List<CardEffect>();
            foreach (CardComponent location in GameControl.AllCardComponents.FindAll(c => c.CardType == CardType.Location && c.IsInPlay && ((CardLocation)c.CardLogic).CanMoveToThis(ThisCard.VisualOwner)))
                chooseToMove.Add(new CardEffect(
                    card: location,
                    effect: () => Moving(location),
                    type: EffectType.Choose,
                    name: "Moverse aquí",
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            yield return new ChooseCardAction(chooseToMove, cancelableCardEffect: evadeAll).RunNow();

            IEnumerator Moving(CardComponent location)
            {
                foreach (CardComponent enemy in ThisCard.VisualOwner.AllEnemiesInMyLocation)
                    enemy.MoveTo(zoneToStayEnemies);
                yield return new MoveCardAction(ThisCard.VisualOwner.PlayCard, location.MyOwnZone).RunNow();
                foreach (CardComponent enemy in ThisCard.VisualOwner.AllEnemiesInMyLocation)
                    yield return new MoveCardAction(enemy, zoneToStayEnemies, withPreview: false).RunNow();
            }
        }

        bool IBuffable.ActiveBuffCondition(GameAction gameAction) => ThisCard.IsInPlay;

        void IBuffable.BuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);
            ThisCard.VisualOwner.AgilityBonus++;
        }

        void IBuffable.DeBuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            ThisCard.VisualOwner.AgilityBonus--;
        }
    }
}