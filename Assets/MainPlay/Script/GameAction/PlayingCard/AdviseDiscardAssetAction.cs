using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ArkhamGamePlay
{
    public class AdviseDiscardAssetAction : GameAction
    {
        readonly CardEffect cardEffect;
        readonly InvestigatorComponent investigator;
        bool CheckConditionsToDiscard => cardEffect.Card.CardLogic is CardAsset && (cardEffect.Type.HasFlag(EffectType.Play) || cardEffect.Type.HasFlag(EffectType.Fast)) && !cardEffect.IsCancel;
        public bool CanCancel { get; private set; }

        /*****************************************************************************************/
        public AdviseDiscardAssetAction(CardEffect cardEffect, InvestigatorComponent investigator)
        {
            this.cardEffect = cardEffect;
            this.investigator = investigator;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (investigator.IsDefeat) yield break;
            CheckDiscardAssetsAction chooseDiscardAsset = new CheckDiscardAssetsAction(investigator, cardEffect.Card, isConsult: true);
            yield return chooseDiscardAsset.RunNow();
            CanCancel = CheckConditionsToDiscard && chooseDiscardAsset.NoFreeSlot;
        }
    }
}