using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01133 : CardLocation
    {
        InvestigatorComponent investigatorAffected;
        public override LocationSymbol MySymbol => LocationSymbol.X;
        public override LocationSymbol MovePosibilities => LocationSymbol.Circle;

        /*****************************************************************************************/
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is MoveCardAction moveCardAction && CheckMove(moveCardAction))
                new EffectAction(SkillTestGraveyard, SkillTestGraveyardAnimation).AddActionTo();
        }

        bool CheckMove(MoveCardAction moveCardAction)
        {
            if (moveCardAction.ThisCard.CardType != CardType.PlayCard) return false;
            if (moveCardAction.Zone != ThisCard.MyOwnZone) return false;
            investigatorAffected = moveCardAction.ThisCard.Owner;
            return true;
        }

        IEnumerator SkillTestGraveyardAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator SkillTestGraveyard()
        {
            SkillTest skillTest = new SkillTest
            {
                Title = "Entrando en " + ThisCard.Info.Name,
                SkillType = Skill.Willpower,
                CardToTest = ThisCard,
                TestValue = 3
            };

            skillTest.LoseEffect.Add(new CardEffect(
                card: ThisCard,
                effect: GraveyardEffect,
                animationEffect: SkillTestGraveyardAnimation,
                type: EffectType.Choose,
                name: "Efecto de " + ThisCard.Info.Name));

            yield return new SkillTestAction(skillTest).RunNow();
        }

        IEnumerator GraveyardEffect()
        {
            CardComponent riverTown = GameControl.GetCard("01125");
            List<CardEffect> cardEffects = new List<CardEffect>()
            {
                new CardEffect(
                    card: investigatorAffected.InvestigatorCardComponent,
                    effect: () => new AssignDamageHorror(investigatorAffected, horrorAmount: 2).RunNow(),
                    type: EffectType.Choose,
                    name: "Recibir horror."
                    ),
                new CardEffect(
                    card: riverTown,
                    effect: () => new MoveCardAction(investigatorAffected.PlayCard, riverTown.MyOwnZone, withPreview: false).RunNow(),
                    type: EffectType.Choose,
                    name: "Moverte a " + riverTown.Info.Name
                    )
            };
            yield return new ChooseCardAction(cardEffects, isOptionalChoice: false).RunNow();
        }
    }
}
