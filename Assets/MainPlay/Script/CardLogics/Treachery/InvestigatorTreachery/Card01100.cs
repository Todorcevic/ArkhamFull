using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01100 : InvestigatorTreachery
    {
        public override bool IsDiscarted => false;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InvestigatorTurn investigatorTurn && Discard(investigatorTurn))
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => new DiscardAction(ThisCard).RunNow(),
                    type: EffectType.Activate,
                    name: "Descartar " + ThisCard.Info.Name,
                    actionCost: 2));
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is AddTokenAction addTokenAction && TakeExtraDamage(addTokenAction))
                new EffectAction(ExtraHorror, ExtraHorrorAnimation).AddActionTo();
        }

        bool Discard(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.IsInPlay) return false;
            if (Investigator.CurrentLocation != investigatorTurn.ActiveInvestigator.CurrentLocation) return false;
            return true;
        }

        bool TakeExtraDamage(AddTokenAction addTokenAction)
        {
            if (!ThisCard.IsInPlay) return false;
            if (addTokenAction.ToToken.TokenInThisCard != Investigator.InvestigatorCardComponent) return false;
            if (addTokenAction.ToToken.TokenType != CardTokenType.Health) return false;
            if (addTokenAction.Amount < 1) return false;
            return true;
        }

        IEnumerator ExtraHorrorAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator ExtraHorror() => new AddTokenAction(Investigator.InvestigatorCardComponent.SanityToken, 1).RunNow();


        public override IEnumerator Revelation() =>
           new MoveCardAction(ThisCard, Investigator.Threat, withPreview: false).RunNow();
    }
}