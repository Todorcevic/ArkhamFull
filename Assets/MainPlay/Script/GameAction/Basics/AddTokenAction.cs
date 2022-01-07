using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class AddTokenAction : GameAction
    {
        TokenStack TokenStack => AllComponents.TokenStacks;
        public override GameActionType GameActionType => GameActionType.Basic;
        public int Amount { get; set; }
        public TokenComponent ToToken { get; set; }
        public TokenComponent FromToken { get; set; }
        public CardComponent CardAffected { get; set; }
        public TokenComponent SecundaryToToken { get; set; }
        public int SecundaryAmount { get; set; }

        /*****************************************************************************************/
        public AddTokenAction(TokenComponent toToken, int amount, TokenComponent from = null, TokenComponent secundaryToToken = null, int secundaryAmount = 0)
        {
            CardAffected = toToken.TokenInThisCard;
            Amount = amount;
            ToToken = toToken;
            FromToken = from ?? TokenStack.AllTokens.Find(t => t.TokenType == ToToken.TokenType);
            if (amount < 0) Reverse(); //RemoveTokenAction ej: AddTokenAction(investigator.ResourcesToken, -5).RunNow();
            SecundaryToToken = secundaryToToken; //Used to assign damage and horror at once
            SecundaryAmount = secundaryAmount;
        }

        /*****************************************************************************************/
        void Reverse()
        {
            FromToken = ToToken;
            ToToken = TokenStack.AllTokens.Find(t => t.TokenType == ToToken.TokenType);
            Amount = Mathf.Abs(Amount);
        }

        protected override IEnumerator ActionLogic()
        {
            if (CardAffected.IsOutGame || !CardAffected.IsInPlay) yield break;
            yield return ToToken.AddAmountFrom(Amount, FromToken);
            if (SecundaryToToken != null) yield return SecundaryToToken.AddAmountFrom(SecundaryAmount, FromToken);
        }
    }
}