using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class Card01071 : CardAsset
    {
        float timeAnimation = GameData.ANIMATION_TIME_DEFAULT;
        readonly Vector3[] originalTokenPosition = new Vector3[2];

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is RevealChaosTokenAction revealToken && ChooseToken())
                revealToken.ChooseCardOptionalAction.ChosenCardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => RevealOtherToken(revealToken),
                    animationEffect: RevealOtherTokenAnimation,
                    payEffect: PayCostEffect,
                    checkFilterToCancel: () => CheckFilterToCancel(revealToken),
                    type: EffectType.Reaction,
                    name: "Activar " + ThisCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
        }
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is AddTokenAction && CheckDiscard())
                new EffectAction(DiscardThisCard, DiscardThisCardAnimation).AddActionTo();
        }

        bool ChooseToken()
        {
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.ResourcesToken.Amount < 1) return false;
            if (ThisCard.VisualOwner != GameControl.ActiveInvestigator) return false;
            return true;
        }

        bool CheckDiscard()
        {
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.ResourcesToken.Amount > 0) return false;
            if (ThisCard.IsDiscarting) return false;
            return true;
        }

        IEnumerator RevealOtherTokenAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator RevealOtherToken(RevealChaosTokenAction revealToken)
        {
            revealToken.IsActionCanceled = true;
            List<ChaosTokenComponent> tokens = new List<ChaosTokenComponent>();
            List<CardEffect> allTokens = new List<CardEffect>();
            for (int i = 0; i < 2; i++)
            {
                ChaosTokenComponent token = AllComponents.ChaosBag.RandomChaosToken();
                tokens.Add(token);
                allTokens.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => ChoosingToken(token),
                    type: EffectType.Choose,
                    name: "Elegir este token",
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
                yield return AllComponents.ChaosBag.DropToken(token);
                yield return new WaitUntil(token.RigidBody.IsSleeping);
            }
            MultiCastAction multiCast = new MultiCastAction(allTokens, isOptionalChoice: false);

            for (int i = 0; i < 2; i++)
            {
                CardComponent card = multiCast.ListCardsEffect[i].Card;
                card.CardTools.TokensBox.SetActive(false);
                card.CardTools.HideFrontCard(true);
                card.CardTools.ChangeGlowImage(AllComponents.CardBuilder.TokenGlow, Vector3.one);
                originalTokenPosition[i] = tokens[i].transform.position;
                tokens[i].transform.SetParent(card.transform);
                tokens[i].RigidBody.isKinematic = true;
                tokens[i].GetComponent<MeshCollider>().enabled = false;
                DOTween.Sequence().Append(tokens[i].transform.DOLocalMove(Vector3.zero, timeAnimation))
                    .Join(tokens[i].transform.DOScale(0.3f, timeAnimation))
                    .Join(tokens[i].transform.DOLocalRotate(new Vector3(180, 0, 0), timeAnimation));
                tokens[i].gameObject.layer = 8;
            }
            yield return multiCast.RunNow();

            IEnumerator ChoosingToken(ChaosTokenComponent tokenSelected)
            {
                for (int i = 0; i < 2; i++)
                {
                    tokens[i].AudioSource.enabled = true;
                    tokens[i].transform.SetParent(AllComponents.ChaosBag.transform);
                    DOTween.Sequence().Append(tokens[i].transform.DOMove(originalTokenPosition[i], timeAnimation))
                        .Join(tokens[i].transform.DOScale(1f, timeAnimation)).SetId("DualTokens");
                    tokens[i].RigidBody.isKinematic = false;
                    tokens[i].GetComponent<MeshCollider>().enabled = true;
                    tokens[i].gameObject.layer = 0;
                }
                yield return new WaitWhile(() => DOTween.IsTweening("DualTokens"));
                revealToken.SkillTest.TokenThrow = tokenSelected;
                yield return AllComponents.ChaosBag.ReturnToken(tokens.Find(t => t != tokenSelected));
            }
        }

        IEnumerator PayCostEffect() => new AddTokenAction(ThisCard.ResourcesToken, -1).RunNow();

        IEnumerator DiscardThisCardAnimation()
        {
            ThisCard.CardTools.PlayOneShotSound(ThisCard.Effect2);
            yield return new WaitWhile(() => ThisCard.CardTools.AudioSource.isPlaying);
        }

        IEnumerator DiscardThisCard() => new DiscardAction(ThisCard).RunNow();

        protected override IEnumerator PlayCardFromHand()
        {
            yield return base.PlayCardFromHand();
            yield return new AddTokenAction(ThisCard.ResourcesToken, 4).RunNow();
        }

        bool CheckFilterToCancel(RevealChaosTokenAction revealToken) => revealToken.IsActionCanceled;
    }
}