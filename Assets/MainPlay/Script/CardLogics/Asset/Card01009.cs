using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01009 : CardAsset, IRevelation
    {
        Effect revelation;
        CardComponent Daisy => GameControl.GetCard("01002").Owner.InvestigatorCardComponent;
        bool IRevelation.IsDiscarted => false;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CanTakeHorror(investigatorTurn))
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: TakeHorror,
                    animationEffect: TakeHorrorAnimation,
                    type: EffectType.Activate,
                    name: "Mover 1 token de horror a " + Daisy.Info.Name,
                    actionCost: 1)
                    );
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is AddTokenAction) ThisCard.CanBeDiscard = ThisCard.SanityToken.Amount < 1;
            if (gameAction is RevealChaosTokenAction revealChaosT && ChangeToken(revealChaosT))
                new EffectAction(() => ChangingTokenToFail(revealChaosT), ChangingTokenToFailAnimation).AddActionTo();
        }

        bool CanTakeHorror(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.VisualOwner.CurrentLocation != investigatorTurn.ActiveInvestigator.CurrentLocation) return false;
            if (ThisCard.SanityToken.Amount < 1) return false;
            return true;
        }

        bool ChangeToken(RevealChaosTokenAction revealChaosToken)
        {
            if (!ThisCard.IsInPlay) return false;
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (revealChaosToken.Token.Type != ChaosTokenType.Win) return false;
            return true;
        }

        IEnumerator TakeHorrorAnimation() =>
            new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator TakeHorror()
        {
            yield return new AddTokenAction(Daisy.SanityToken, 1, ThisCard.SanityToken).RunNow();
            if (ThisCard.SanityToken.Amount < 1) yield return new DiscardAction(ThisCard).RunNow();
        }

        IEnumerator ChangingTokenToFailAnimation() =>
            new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect2).RunNow();

        IEnumerator ChangingTokenToFail(RevealChaosTokenAction revealChaosToken)
        {
            ChaosTokenComponent oldToken = revealChaosToken.Token;
            revealChaosToken.Token = AllComponents.ChaosBag.tokenList.Find(t => t.Type == ChaosTokenType.Fail);
            yield return new ReturnChaosTokenAction(oldToken).RunNow();
            SkillTest skillTest = revealChaosToken.SkillTest;
            yield return new RevealChaosTokenAction(ref skillTest, revealChaosToken.Token).RunNow();
        }

        public IEnumerator Revelation()
        {
            yield return new MoveCardAction(ThisCard, ThisCard.VisualOwner.Threat, withPreview: false).RunNow();
            yield return new AddTokenAction(ThisCard.SanityToken, 3).RunNow();
        }
    }
}