using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class PlayCardAction : GameAction
    {
        CardEffect cardEffect;
        public InvestigatorComponent Investigator { get; }
        public CardEffect CardEffect { get => cardEffect; set => cardEffect = value; }
        public CardEffect CancelCardEffect { get; }

        /*****************************************************************************************/
        public PlayCardAction(CardEffect cardEffect)
        {
            this.cardEffect = cardEffect;
            Investigator = cardEffect.PlayOwner;
            CancelCardEffect = new CardEffect(
                card: cardEffect.Card,
                effect: () => null,
                type: EffectType.Choose,
                name: string.Empty);
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (Investigator.IsDefeat) yield break;
            yield return CheckFilters();
            if (CancelCardEffect.Name != string.Empty)
                yield return new ChooseCardAction(CancelCardEffect, cancelableCardEffect: cardEffect).RunNow();
        }

        IEnumerator CheckFilters()
        {
            AdviseDiscardAssetAction adviseDiscard = new AdviseDiscardAssetAction(cardEffect, Investigator);
            yield return adviseDiscard.RunNow();
            if (adviseDiscard.CanCancel) CancelCardEffect.Name += "<color=#FF2B2B>Deberás descartar algun apoyo para jugar " + CancelCardEffect.Card.Info.Name + ". </color>";

            OportunityAttack oportunityAttack = new OportunityAttack(cardEffect, Investigator);
            yield return oportunityAttack.RunNow();
            if (oportunityAttack.CanCancel) CancelCardEffect.Name += cardEffect.Name + "<color=#FF2B2B> provocará un ataque de oportunidad. </color>";

            CheckCancelSkillTest checkCancelSkillTest = new CheckCancelSkillTest(cardEffect);
            yield return checkCancelSkillTest.RunNow();
            if (checkCancelSkillTest.CanCancel) CancelCardEffect.Name += cardEffect.Name + "<color=#FF2B2B> hará que no se pueda cancelar la prueba de habilidad. </color>";
        }
    }
}