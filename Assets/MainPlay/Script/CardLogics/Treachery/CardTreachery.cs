using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public abstract class CardTreachery : CardLogic, IRevelation
    {
        public virtual bool IsPeril => false;
        public virtual bool IsSurge => false;
        public virtual bool IsDiscarted => true;

        /*****************************************************************************************/

        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is CardEffectsFilterAction filterAction && CheckPerilRevelation())
                RemoveCardEffects(filterAction);
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is RevelationAction revelatioAction && CheckSurgeRevelation(revelatioAction))
                new DrawAction(GameControl.ActiveInvestigator, isEncounter: true).AddActionTo();
        }

        bool CheckPerilRevelation()
        {
            if (!IsPeril) return false;
            if (!ThisCard.IsInFullPlay) return false;
            return true;
        }

        bool CheckSurgeRevelation(RevelationAction revelatioAction)
        {
            if (!IsSurge) return false;
            if (revelatioAction.RevelationCard != ThisCard) return false;
            return true;
        }

        void RemoveCardEffects(CardEffectsFilterAction filterAction)
        {
            filterAction.CardEffects = filterAction.CardEffects.FindAll(c => c.PlayOwner == GameControl.ActiveInvestigator);
        }

        public abstract IEnumerator Revelation();
    }
}