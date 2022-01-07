using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01174 : CardTreachery, IBuffable
    {
        CardComponent LocationAffected;
        CardEffect breakCardEffect;
        CardEffect pickCardEffect;
        SkillTest skillTest;
        enum typeDiscard { breaking, picking }
        public override bool IsDiscarted => false;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is CardEffectsFilterAction filterAction && ActiveBuffCondition(filterAction))
                filterAction.CardEffects.RemoveAll(c => c.PlayOwner?.CurrentLocation == LocationAffected && c.Type.HasFlag(EffectType.Investigate));
            if (gameAction is InvestigatorTurn investigatorTurn && CheckCanDiscard(investigatorTurn))
            {
                investigatorTurn.CardEffects.Add(breakCardEffect = new CardEffect(
                    card: ThisCard,
                    effect: () => SkillTestToDiscard(typeDiscard.breaking),
                    type: EffectType.Activate,
                    name: "Romper " + ThisCard.Info.Name,
                    actionCost: 1));
                investigatorTurn.CardEffects.Add(pickCardEffect = new CardEffect(
                    card: ThisCard,
                    effect: () => SkillTestToDiscard(typeDiscard.picking),
                    type: EffectType.Activate,
                    name: "Forzar " + ThisCard.Info.Name,
                    actionCost: 1));
            }
        }

        bool CheckCanDiscard(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.IsInPlay) return false;
            if (investigatorTurn.ActiveInvestigator.PlayCard.CurrentZone != ThisCard.CurrentZone) return false;
            return true;
        }

        IEnumerator SkillTestToDiscard(typeDiscard type)
        {
            skillTest = new SkillTest
            {
                Title = (type == typeDiscard.breaking ? "Rompiendo " : "Forzando ") + ThisCard.Info.Name,
                SkillType = type == typeDiscard.breaking ? Skill.Combat : Skill.Agility,
                CardToTest = ThisCard,
                TestValue = 4,
                IsOptional = true
            };
            skillTest.WinEffect.Add(new CardEffect(
                card: ThisCard,
                effect: () => new DiscardAction(ThisCard, withTopUp: false).RunNow(),
                animationEffect: () => new AnimationCardAction(ThisCard, withReturn: false, audioClip: type == typeDiscard.breaking ? ThisCard.Effect1 : ThisCard.Effect2).RunNow(),
                type: EffectType.Choose,
                name: "Descartar " + ThisCard.Info.Name));
            SkillTestAction skillTestAction = new SkillTestAction(skillTest);
            yield return skillTestAction.RunNow();
            breakCardEffect.IsCancel = pickCardEffect.IsCancel = !skillTestAction.SkillTest.IsComplete ?? false;
        }

        public override IEnumerator Revelation()
        {
            List<CardComponent> AllLocation = GameControl.AllCardComponents.FindAll(c => c.CardLogic is CardLocation && c.IsInPlay && !c.MyOwnZone.ListCards.Exists(p => p.ID == ThisCard.ID));
            int? maxClues = AllLocation.Select(c => c.CluesToken.Amount)?.Max();
            List<CardComponent> MostCluesLocation = AllLocation.FindAll(c => c.CluesToken.Amount == maxClues);
            List<CardEffect> cardEffects = new List<CardEffect>();
            foreach (CardComponent location in MostCluesLocation)
            {
                cardEffects.Add(new CardEffect(
                    card: location,
                    effect: () => new MoveCardAction(ThisCard, location.MyOwnZone).RunNow(),
                    type: EffectType.Choose,
                    name: "Seleccionar " + location.Info.Name
                    ));
            }

            yield return new ChooseCardAction(cardEffects, isOptionalChoice: false, withPreview: cardEffects.Count > 1).RunNow();
        }

        public bool ActiveBuffCondition(GameAction gameAction) => ThisCard.IsInPlay;

        public void BuffEffect()
        {
            LocationAffected = ThisCard.CurrentZone.ThisCard;
            LocationAffected.CardTools.ShowBuff(ThisCard);
        }

        public void DeBuffEffect()
        {
            LocationAffected.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            LocationAffected = null;
        }
    }
}
