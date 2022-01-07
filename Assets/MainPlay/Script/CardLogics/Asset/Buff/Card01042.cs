using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01042 : CardAsset, IBuffable
    {
        CardEffect mainCardEffect;
        InvestigatorComponent investigatorAfected;
        Skill skillBonuses;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckActiveEffect(investigatorTurn))
                investigatorTurn.CardEffects.Add(mainCardEffect = new CardEffect(
                    card: ThisCard,
                    effect: Bonus,
                    animationEffect: BonusAnimation,
                    type: EffectType.Activate,
                    name: "Activar " + ThisCard.Info.Name,
                    actionCost: 1,
                    needExhaust: true,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is PhaseAction) BuffActive = false;
        }

        bool CheckActiveEffect(InvestigatorTurn investigatorTurn)
        {
            if (ThisCard.CurrentZone != investigatorTurn.ActiveInvestigator.Assets) return false;
            return true;
        }

        IEnumerator BonusAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator Bonus()
        {
            List<CardEffect> cardEffects = new List<CardEffect>();
            foreach (CardComponent investigator in GameControl.AllInvestigatorsInGame.FindAll(i => i.CurrentLocation == ThisCard.VisualOwner.CurrentLocation).Select(i => i.InvestigatorCardComponent))
                cardEffects.Add(new CardEffect(
                    card: investigator,
                    effect: () => ChooseSkill(investigator),
                    animationEffect: ((CardInvestigator)investigator.CardLogic).ThankYouAnimation,
                    type: EffectType.Choose,
                    name: "Obtener bonificación para " + investigator.Info.Name,
                    investigatorImageCardInfoOwner: investigator.Owner));
            yield return new ChooseCardAction(cardEffects, cancelableCardEffect: mainCardEffect).RunNow();

            IEnumerator ChooseSkill(CardComponent investigator)
            {
                investigatorAfected = investigator.Owner;
                List<CardEffect> cardEffects2 = new List<CardEffect>();
                foreach (Skill skill in ((Skill[])Enum.GetValues(typeof(Skill))).Where(v => v != Skill.None))
                    cardEffects2.Add(new CardEffect(
                        card: investigator,
                        effect: () => AddSkill(skill),
                        type: EffectType.Choose,
                        name: "+2 <sprite name=" + skill.ToString() + ">",
                        investigatorImageCardInfoOwner: investigator.Owner)
                        );
                MultiCastAction multiCast = new MultiCastAction(cardEffects2, isOptionalChoice: mainCardEffect.IsCancelable);
                yield return multiCast.RunNow();
                mainCardEffect.IsCancel = multiCast.IsCancel;


                IEnumerator AddSkill(Skill skill)
                {
                    skillBonuses = skill;
                    BuffActive = true;
                    yield return null;
                }
            }
        }

        bool IBuffable.ActiveBuffCondition(GameAction gameAction) => BuffActive;

        void IBuffable.BuffEffect()
        {
            investigatorAfected.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);
            switch (skillBonuses)
            {
                case Skill.Agility: investigatorAfected.AgilityBonus += 2; break;
                case Skill.Combat: investigatorAfected.CombatBonus += 2; break;
                case Skill.Intellect: investigatorAfected.IntellectBonus += 2; break;
                case Skill.Willpower: investigatorAfected.WillpowerBonus += 2; break;
            }
        }

        void IBuffable.DeBuffEffect()
        {
            investigatorAfected.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            switch (skillBonuses)
            {
                case Skill.Agility: investigatorAfected.AgilityBonus -= 2; break;
                case Skill.Combat: investigatorAfected.CombatBonus -= 2; break;
                case Skill.Intellect: investigatorAfected.IntellectBonus -= 2; break;
                case Skill.Willpower: investigatorAfected.WillpowerBonus -= 2; break;
            }
        }
    }
}