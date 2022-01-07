using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01087 : CardAsset
    {
        CardEffect investigateWithFlash;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckInvestigate(investigatorTurn))
                investigatorTurn.CardEffects.Add(investigateWithFlash = new CardEffect(
                    card: ThisCard,
                    effect: InvestigateWithFlash,
                    animationEffect: InvestigateWithFlashAnimation,
                    payEffect: PayCostEffect,
                    cancelEffect: CancelEffect,
                    type: EffectType.Activate | EffectType.Investigate,
                    name: "Investigar con " + ThisCard.Info.Name,
                    actionCost: 1,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
        }

        bool CheckInvestigate(InvestigatorTurn investigatorTurn)
        {
            if (ThisCard.CurrentZone != investigatorTurn.ActiveInvestigator.Assets) return false;
            if (investigatorTurn.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (ThisCard.ResourcesToken.Amount < 1) return false;
            return true;
        }

        IEnumerator InvestigateWithFlashAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator InvestigateWithFlash()
        {
            InvestigateLocation investigateLocation = new InvestigateLocation(ThisCard.VisualOwner.CurrentLocation, investigateWithFlash.IsCancelable);
            investigateLocation.SkillTest.TestValue -= 2;
            investigateLocation.SkillTest.ExtraCard = ThisCard;
            yield return investigateLocation.RunNow();
            investigateWithFlash.IsCancel = !investigateLocation.SkillTest.IsComplete ?? false;
        }

        IEnumerator PayCostEffect() => new AddTokenAction(ThisCard.ResourcesToken, -1).RunNow();

        IEnumerator CancelEffect() => new AddTokenAction(ThisCard.ResourcesToken, 1).RunNow();

        protected override IEnumerator PlayCardFromHand()
        {
            yield return base.PlayCardFromHand();
            yield return new AddTokenAction(ThisCard.ResourcesToken, 3).RunNow();
        }
    }
}