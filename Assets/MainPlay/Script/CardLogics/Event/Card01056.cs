using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01056 : CardEvent
    {
        ChaosTokenComponent chaosToken;
        RevealChaosTokenAction revealChaos;
        int? oldValue;
        protected override bool IsFast => true;
        protected override string nameCardEffect => "Cambiar resultado del token";

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction) { }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is RevealChaosTokenAction revealChaos && ChangeChaosToken(revealChaos))
            {
                this.revealChaos = revealChaos;
                revealChaos.ChooseCardOptionalAction.ChosenCardEffects.Add(playFromHand = PlayFromHand);
            }

            if (gameAction is SkillTestActionComplete skillTestComplete && CheckResetChaosToken(skillTestComplete))
            {
                chaosToken.Value = chaosToken.Type == ChaosTokenType.Basic ? oldValue : null; //Autorefresh
                chaosToken = null;
            }
        }

        bool ChangeChaosToken(RevealChaosTokenAction revealChaos)
        {
            if (!CheckPlayedFromHand()) return false;
            if (revealChaos.Token.Value >= 0) return false;
            if (ThisCard.VisualOwner != GameControl.ActiveInvestigator) return false;
            return true;
        }

        bool CheckResetChaosToken(SkillTestActionComplete skillTestComplete)
        {
            if (chaosToken == null) return false;
            if (skillTestComplete.SkillTest.TokenThrow != chaosToken) return false;
            return true;
        }

        protected override IEnumerator LogicEffect()
        {
            chaosToken = revealChaos.Token;
            oldValue = chaosToken.Value;
            chaosToken.Value = Math.Abs((int)chaosToken.Value);
            AllComponents.PanelSkillTest.AddModifier(ThisCard, 0);
            yield return null;
        }

        protected override bool CheckFilterToCancel() => !ChangeChaosToken(revealChaos);

    }
}