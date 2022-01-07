using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01081 : CardSkill
    {
        bool desenganged;
        CardComponent locationToMove;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is SkillTestActionComplete skillTestComplete && CheckSkillTestWin(skillTestComplete))
                new EffectAction(ThisCardEffect, ThisCardAnimation).AddActionTo();
        }

        protected override bool CheckSkillTestWin(SkillTestActionComplete skillTestComplete)
        {
            if (skillTestComplete.SkillTest.SkillTestType != SkillTestType.Evade) return false;
            return base.CheckSkillTestWin(skillTestComplete);
        }

        protected override IEnumerator ThisCardAnimation() =>
            new AnimationCardAction(ThisCard, withReturn: false, audioClip: ThisCard.Effect1).RunNow();


        protected override IEnumerator ThisCardEffect()
        {
            Zone zoneToStayEnemies = GameControl.ActiveInvestigator.CurrentLocation.MyOwnZone;
            CardEffect thisCardEffect = new CardEffect(
                card: ThisCard,
                effect: ConfirmDesenganged,
                type: EffectType.Choose,
                name: "Dejar de estar enfrentado a todos los enemigos");
            yield return new ChooseCardAction(thisCardEffect, isOptionalChoice: true).RunNow();

            LocationSymbol thisLocationSymbol = ((CardLocation)GameControl.ActiveInvestigator.CurrentLocation.CardLogic).MySymbol;
            List<CardEffect> chooseToMove = new List<CardEffect>();
            foreach (CardComponent location in GameControl.AllCardComponents.FindAll(c => c.CardType == CardType.Location && c.IsInPlay && ((CardLocation)c.CardLogic).MovePosibilities.HasFlag(thisLocationSymbol)))
                chooseToMove.Add(new CardEffect(
                    card: location,
                    effect: () => ConfirmMove(location),
                    type: EffectType.Choose | EffectType.Move,
                    name: "Moverse aquí"));
            yield return new ChooseCardAction(chooseToMove, isOptionalChoice: true).RunNow();

            if (desenganged)
                foreach (CardComponent enemy in ThisCard.VisualOwner.AllEnemiesEnganged)
                    enemy.MoveTo(zoneToStayEnemies);
            if (locationToMove != null)
                yield return new MoveCardAction(GameControl.ActiveInvestigator.PlayCard, locationToMove.MyOwnZone).RunNow();
            if (desenganged)
                foreach (CardComponent enemy in ThisCard.VisualOwner.AllEnemiesEnganged)
                    yield return new MoveCardAction(enemy, enemy.CurrentZone, withPreview: false).RunNow();
            desenganged = false;
            locationToMove = null;

            IEnumerator ConfirmDesenganged()
            {
                desenganged = true;
                yield return null;
            }

            IEnumerator ConfirmMove(CardComponent location)
            {
                locationToMove = location;
                yield return null;
            }
        }
    }
}