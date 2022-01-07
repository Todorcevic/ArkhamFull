using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01103 : CardEnemy, ICleanCard, IBuffable
    {
        InvestigatorComponent investigatorSelected;
        readonly Dictionary<InvestigatorComponent, CardLogic> investigatorsAfected = new Dictionary<InvestigatorComponent, CardLogic>();
        public override List<InvestigatorComponent> Prey(List<InvestigatorComponent> investigatorList) =>
            investigatorList.Contains(ThisCard.Owner) ? new List<InvestigatorComponent> { ThisCard.Owner } : investigatorList;

        public override bool IsHunter => true;

        /*****************************************************************************************/
        void ICleanCard.CleanCard()
        {
            foreach (InvestigatorComponent investigator in investigatorsAfected.Keys.ToArray())
            {
                investigatorSelected = investigator;
                ((IBuffable)this).DeBuffEffect();
            }
        }

        bool IBuffable.ActiveBuffCondition(GameAction gameAction)
        {
            foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame)
            {
                investigatorSelected = investigator;
                if (CurrentLocation != null && investigator.CurrentLocation == CurrentLocation && !investigatorsAfected.ContainsKey(investigator))
                    ((IBuffable)this).BuffEffect();
                else if (investigatorsAfected.ContainsKey(investigator) && investigator.CurrentLocation != CurrentLocation)
                    ((IBuffable)this).DeBuffEffect();
            }
            return false;
        }

        void IBuffable.BuffEffect()
        {
            investigatorsAfected.Add(investigatorSelected, investigatorSelected.InvestigatorCardComponent.CardLogic);
            investigatorSelected.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);
            investigatorSelected.InvestigatorCardComponent.CardLogic = new CardInvestigator().WithThisCard(investigatorSelected.InvestigatorCardComponent);
        }

        void IBuffable.DeBuffEffect()
        {
            investigatorSelected.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            investigatorSelected.InvestigatorCardComponent.CardLogic = investigatorsAfected[investigatorSelected];
            investigatorsAfected.Remove(investigatorSelected);
        }
    }
}