using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01035 : CardAsset
    {
        CardEffect mainCardEffect;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckActiveEffect(investigatorTurn))
                investigatorTurn.CardEffects.Add(mainCardEffect = new CardEffect(
                    card: ThisCard,
                    effect: Heal,
                    animationEffect: HealAnimation,
                    type: EffectType.Activate,
                    name: "Activar " + ThisCard.Info.Name,
                    actionCost: 1,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
        }

        bool CheckActiveEffect(InvestigatorTurn investigatorTurn)
        {
            if (ThisCard.CurrentZone != investigatorTurn.ActiveInvestigator.Assets) return false;
            return true;
        }

        IEnumerator HealAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator Heal()
        {
            List<CardEffect> cardEffects = new List<CardEffect>();
            foreach (CardComponent investigator in GameControl.AllInvestigatorsInGame.FindAll(i => i.CurrentLocation == ThisCard.VisualOwner.CurrentLocation).Select(i => i.InvestigatorCardComponent))
                cardEffects.Add(new CardEffect(
                    card: investigator,
                    effect: () => SkillTestHeal(investigator),
                    animationEffect: ((CardInvestigator)investigator.CardLogic).ThankYouAnimation,
                    type: EffectType.Choose,
                    name: "Intentar curar a " + investigator.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            yield return new ChooseCardAction(cardEffects, cancelableCardEffect: mainCardEffect).RunNow();

            IEnumerator SkillTestHeal(CardComponent investigator)
            {
                SkillTest skillTest = new SkillTest()
                {
                    Title = "Curando a " + investigator.Info.Name,
                    SkillType = Skill.Intellect,
                    CardToTest = ThisCard,
                    TestValue = 2,
                    IsOptional = mainCardEffect.IsCancelable
                };

                skillTest.WinEffect.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => Healing(investigator),
                    type: EffectType.Choose,
                    name: "Curar a " + investigator.Info.Name + " con " + ThisCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner,
                    investigatorRealOwner: investigator.Owner));

                skillTest.LoseEffect.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => new AssignDamageHorror(investigator.Owner, damageAmount: 1).RunNow(),
                    type: EffectType.Choose,
                    name: "Dañar a " + investigator.Info.Name + " con " + ThisCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner,
                    investigatorRealOwner: investigator.Owner));

                SkillTestAction skillTestAction = new SkillTestAction(skillTest);
                yield return skillTestAction.RunNow();
                mainCardEffect.IsCancel = !skillTestAction.SkillTest.IsComplete ?? false;

                IEnumerator Healing(CardComponent cardToHeal)
                {
                    if (cardToHeal.HealthToken.Amount > 0)
                        yield return new AddTokenAction(investigator.HealthToken, -1).RunNow();
                }
            }
        }
    }
}