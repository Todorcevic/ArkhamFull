using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class OportunityAttack : GameAction
    {
        readonly InvestigatorComponent investigator;
        bool CheckOportunityAttack() => investigator.IsEngangedAndReady
                && (EffectType.Activate | EffectType.Play | EffectType.Investigate | EffectType.Move | EffectType.Resource | EffectType.Draw | EffectType.Engange)
                .HasFlag(CardEffect.Type) && !CardEffect.IsCancel;
        public override GameActionType GameActionType => GameActionType.Compound;
        public CardEffect CardEffect { get; }
        public bool CanCancel { get; private set; }

        /*****************************************************************************************/
        public OportunityAttack(CardEffect cardEffect, InvestigatorComponent investigator)
        {
            CardEffect = cardEffect;
            this.investigator = investigator;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (investigator.IsDefeat) yield break;
            if (CheckOportunityAttack())
            {
                CanCancel = true;
                CardEffect.PayCostEffect = new ActionsTools().JoinEffects(CardEffect.PayCostEffect, PrePayCostEffect);
                yield return null;
            }
        }

        IEnumerator PrePayCostEffect()
        {
            CardEffect.IsCancelable = false;
            yield return new EnemiesAttack(investigator, isOportunityAttack: true).RunNow();
        }
    }
}