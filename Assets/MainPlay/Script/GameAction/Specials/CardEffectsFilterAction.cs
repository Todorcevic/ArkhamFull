using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class CardEffectsFilterAction : GameAction
    {
        public List<CardEffect> CardEffects { get; set; }

        /*****************************************************************************************/
        public CardEffectsFilterAction(List<CardEffect> listCardEffects) => CardEffects = listCardEffects;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            CardEffects.RemoveAll(c => c.ActionCost > c.PlayOwner.ActionsLeft
            || c.ResourceCost > c.PlayOwner.Resources
            || (c.NeedExhaust && c.Card.IsExausted)
            || (c.CheckFilterToCancel?.Invoke() ?? false));
            yield return null;
        }
    }
}