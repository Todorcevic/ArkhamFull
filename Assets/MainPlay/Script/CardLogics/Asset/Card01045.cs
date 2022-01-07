using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01045 : CardAsset
    {
        CardEffect mainCardEffect;

        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckActiveEffect(investigatorTurn))
                investigatorTurn.CardEffects.Add(mainCardEffect = new CardEffect(
                    card: ThisCard,
                    effect: Investigate,
                    animationEffect: InvestigateAnimation,
                    type: EffectType.Activate | EffectType.Investigate,
                    name: "Activar " + ThisCard.Info.Name,
                    actionCost: 1,
                    needExhaust: true,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
        }

        bool CheckActiveEffect(InvestigatorTurn investigatorTurn)
        {
            if (ThisCard.CurrentZone != investigatorTurn.ActiveInvestigator.Assets) return false;
            return true;
        }

        IEnumerator InvestigateAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator Investigate()
        {
            InvestigateLocation investigateLocation = new InvestigateLocation(ThisCard.VisualOwner.CurrentLocation, mainCardEffect.IsCancelable);
            investigateLocation.WinEffect = () => new AddTokenAction(ThisCard.VisualOwner.InvestigatorCardComponent.ResourcesToken, 3).RunNow(); ;
            yield return investigateLocation.RunNow();
            mainCardEffect.IsCancel = !investigateLocation.SkillTest.IsComplete ?? false;
        }
    }
}