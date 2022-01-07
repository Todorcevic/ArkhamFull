using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;

namespace ArkhamGamePlay
{
    public class CardLogic
    {
        bool buffActive;
        public CardComponent ThisCard { get; set; }
        protected bool BuffActive
        {
            get => buffActive;
            set
            {
                if (value == buffActive) return;
                buffActive = value;
                if (buffActive) ((IBuffable)this).BuffEffect();
                else ((IBuffable)this).DeBuffEffect();
            }
        }

        /*****************************************************************************************/
        public void BeginGameActionEvents(GameAction gameAction)
        {
            BeginGameAction(gameAction);
            if (this is IBuffable buffableCard) BuffActive = buffableCard.ActiveBuffCondition(gameAction);
        }

        public void EndGameActionEvents(GameAction gameAction)
        {
            EndGameAction(gameAction);
            if (this is IBuffable buffableCard) BuffActive = buffableCard.ActiveBuffCondition(gameAction);
        }

        protected virtual void BeginGameAction(GameAction gameAction) { }

        protected virtual void EndGameAction(GameAction gameAction) { }

        public CardLogic WithThisCard(CardComponent card)
        {
            ThisCard = card;
            return this;
        }
    }
}