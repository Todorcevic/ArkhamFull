using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01115 : CardLocation
    {
        CardEffect parlayCardEffect;
        public override LocationSymbol MySymbol => LocationSymbol.Diamond;
        public override LocationSymbol MovePosibilities => LocationSymbol.Square;
        CardComponent Lita => GameControl.GetCard("01117");

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn)
            {
                if (CanResign(investigatorTurn))
                    investigatorTurn.CardEffects.Add(new CardEffect(
                        card: ThisCard,
                        effect: () => Resign(investigatorTurn.ActiveInvestigator),
                        animationEffect: () => ResignAnimation(investigatorTurn.ActiveInvestigator),
                        type: EffectType.Activate | EffectType.Resign,
                        name: "Huir por la puerta delantera",
                        actionCost: 1));
                if (CanParlayWithLita(investigatorTurn))
                    investigatorTurn.CardEffects.Add(parlayCardEffect = new CardEffect(
                        card: Lita,
                        effect: () => TakeControlLita(investigatorTurn.ActiveInvestigator),
                        animationEffect: TakeControlLitaAnimation,
                        type: EffectType.Activate | EffectType.Parley,
                        name: "Convencer a " + Lita.Info.Name,
                        actionCost: 1));
            }
        }

        bool CanResign(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.MyOwnZone.ListCards.Contains(investigatorTurn.ActiveInvestigator.PlayCard)) return false;
            return true;
        }

        bool CanParlayWithLita(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.IsInPlay) return false;
            if (!IsRevealed) return false;
            if (Lita.Owner != null) return false;
            if (Lita.CurrentZone != investigatorTurn.ActiveInvestigator.PlayCard.CurrentZone) return false;
            return true;
        }

        public override bool CanMoveToThis(InvestigatorComponent investigator)
        {
            if (!IsRevealed) return false;
            return base.CanMoveToThis(investigator);
        }

        IEnumerator ResignAnimation(InvestigatorComponent investigator) =>
            new AnimationCardAction(investigator.InvestigatorCardComponent, audioClip: investigator.InvestigatorCardComponent.Effect8).RunNow();

        IEnumerator Resign(InvestigatorComponent investigator)
        {
            investigator.IsResign = true;
            yield return new InvestigatorEliminatedAction(investigator).RunNow();
        }

        IEnumerator TakeControlLitaAnimation() => new AnimationCardAction(Lita, audioClip: Lita.Effect2).RunNow();

        IEnumerator TakeControlLita(InvestigatorComponent investigator)
        {
            SkillTest skillTest = new SkillTest
            {
                Title = "Convenciendo a " + ThisCard.Info.Name,
                SkillType = Skill.Intellect,
                CardToTest = Lita,
                TestValue = 4,
                IsOptional = parlayCardEffect.IsCancelable
            };
            skillTest.WinEffect.Add(new CardEffect(
                card: Lita,
                effect: WinEffect,
                animationEffect: WinEffectAnimation,
                type: EffectType.Choose,
                name: "Controlar a Lita"));

            SkillTestAction skillTestAction = new SkillTestAction(skillTest);
            yield return skillTestAction.RunNow();
            parlayCardEffect.IsCancel = !skillTestAction.SkillTest.IsComplete ?? false;

            IEnumerator WinEffectAnimation() => new AnimationCardAction(Lita, audioClip: Lita.Effect2).RunNow();

            IEnumerator WinEffect()
            {
                Lita.Owner = investigator;
                yield return new MoveCardAction(Lita, investigator.Assets, withPreview: false).RunNow();
            }
        }
    }
}