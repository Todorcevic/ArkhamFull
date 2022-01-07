using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01099 : InvestigatorTreachery
    {
        public override bool IsDiscarted => false;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InvestigatorTurn investigatorTurn && Discard(investigatorTurn))
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => new DiscardAction(ThisCard, withTopUp: false).RunNow(),
                    animationEffect: DiscardAnimation,
                    type: EffectType.Activate,
                    name: "Descartar " + ThisCard.Info.Name,
                    actionCost: 2));
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is AddTokenAction addTokenAction && CheckIfDamaged(addTokenAction))
                new EffectAction(ExtraDamage, ExtraDamageAnimation).AddActionTo();
        }

        bool Discard(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.IsInPlay) return false;
            if (Investigator.CurrentLocation != investigatorTurn.ActiveInvestigator.CurrentLocation) return false;
            return true;
        }

        bool CheckIfDamaged(AddTokenAction addTokenAction)
        {
            if (!ThisCard.IsInPlay) return false;
            if (addTokenAction.ToToken.TokenInThisCard != Investigator.InvestigatorCardComponent) return false;
            if (addTokenAction.ToToken.TokenType != CardTokenType.Sanity) return false;
            if (addTokenAction.Amount < 1) return false;
            return true;

        }

        IEnumerator DiscardAnimation() =>
            new AnimationCardAction(ThisCard, withReturn: false, audioClip: ThisCard.Effect2).RunNow();

        IEnumerator ExtraDamageAnimation() =>
            new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator ExtraDamage() =>
            new AddTokenAction(Investigator.InvestigatorCardComponent.HealthToken, 1).RunNow();

        public override IEnumerator Revelation() =>
          new MoveCardAction(ThisCard, Investigator.Threat, withPreview: false).RunNow();
    }
}