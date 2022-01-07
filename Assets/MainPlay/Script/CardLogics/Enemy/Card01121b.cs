using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01121b : CardEnemy, IBuffable
    {
        InvestigatorComponent investigatorEnganged;
        public override List<InvestigatorComponent> Prey(List<InvestigatorComponent> investigatorList) =>
            investigatorList.FindAll(i => i.Clues == investigatorList.Max(e => e.Clues));
        public override List<CardComponent> SpawnLocation => Prey(GameControl.AllInvestigatorsInGame).ConvertAll(i => i.PlayCard);
        public override bool IsHunter => true;
        public override int Health => (int)ThisCard.Info.Health + (GameControl.InvestigatorsStartingAmount * 2);

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is DiscoverCluesAction discoverCluesAction && CheckDiscoverClues(discoverCluesAction))
                discoverCluesAction.IsActionCanceled = true;
            if (gameAction is CardEffectsFilterAction filterAction && IsEnganged)
                filterAction.CardEffects.RemoveAll(c => c.NeedSpendClues && c.PlayOwner == investigatorEnganged);
            // No he conseguido averiguar porque CardEffectsFilter no modifica los CardEffects del MultiCastAction
            // Espero averiguarlo y poder eliminar la condicion de abajo
            if (gameAction is MultiCastAction chooseCardAction && IsEnganged)
                chooseCardAction.ListCardsEffect.RemoveAll(c => c.NeedSpendClues && c.PlayOwner == investigatorEnganged);
        }

        bool CheckDiscoverClues(DiscoverCluesAction discoverCluesAction)
        {
            if (!IsEnganged) return false;
            if (discoverCluesAction.Investigator != investigatorEnganged) return false;
            return true;
        }

        public bool ActiveBuffCondition(GameAction gameAction) => IsEnganged;

        public void BuffEffect()
        {
            investigatorEnganged = InvestigatorEnganged;
            InvestigatorEnganged.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);
        }

        public void DeBuffEffect()
        {
            investigatorEnganged.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            investigatorEnganged = null;
        }
    }
}

