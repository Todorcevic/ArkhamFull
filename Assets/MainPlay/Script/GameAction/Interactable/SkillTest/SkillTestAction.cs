using System.Collections;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class SkillTestAction : InteractableAction
    {
        public SkillTest SkillTest { get; }
        public override bool CanPlayFastAction => true;
        public override string GameActionInfo => "Prueba de habilidad para " + ActiveInvestigator.Name;

        /*****************************************************************************************/
        public SkillTestAction(SkillTest skillTest) => SkillTest = skillTest;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogicBegin()
        {
            GameControl.CurrentSkillTestAction = this;
            if (!SkillTest.CardToTest.IsInFullPlay || GameControl.ActiveInvestigator.IsDefeat)
            {
                SkillTest.IsComplete = null;
                new SkillTestActionComplete(SkillTest).AddActionTo();
                AnyIsClicked = true;
            }
            else
            {
                AllComponents.PanelSkillTest.SetPanel(SkillTest);
                yield return base.ActionLogicBegin();
            }
        }

        public override void SetButton()
        {
            AllComponents.PanelSkillTest.SetReadyButton(this, state: ButtonState.Ready);
            if (SkillTest.IsOptional)
            {
                AllComponents.ReadyButton.SetReadyButton(this, state: ButtonState.StandBy);
                AllComponents.ReadyButton.ChangeButtonText("Volver");
            }
        }

        public override void ReadyClicked(ReadyButton button)
        {
            if (button == AllComponents.PanelSkillTest.ReadyButton)
            {
                SkillTest.IsComplete = true;
                new SkillTestActionResolution(SkillTest).AddActionTo();
                new SkillTestActionComplete(SkillTest).AddActionTo();
            }
            else if (button == AllComponents.ReadyButton)
            {
                if (cardSelected != null)
                    CardPlay(cardSelected);
                else new SkillTestActionComplete(SkillTest).AddActionTo();
            }
            AnyIsClicked = true;
        }

        protected override void PlayableCards()
        {
            foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame.FindAll(i => i.CurrentLocation == GameControl.ActiveInvestigator.CurrentLocation))
                foreach (CardComponent card in investigator.Hand.ListCards.FindAll(c => (c.CardTools.GetModifierValue(SkillTest.SkillType) + c.Wild) > 0))
                {
                    CardEffects.Add(new CardEffect(
                        card: card,
                        effect: () => AllComponents.PanelSkillTest.AddModifier(card),
                        type: EffectType.Modifier,
                        name: "Agregar modificador",
                        investigatorImageCardInfoOwner: card.VisualOwner,
                        investigatorRealOwner: ActiveInvestigator)
                        );
                }
            foreach (CardComponent card in SkillTest.Modifiers)
                CardEffects.Add(new CardEffect(
                    card: card,
                    effect: () => AllComponents.PanelSkillTest.RemoveModifier(card),
                    type: EffectType.Modifier,
                    name: "Quitar modificador",
                    investigatorImageCardInfoOwner: card.VisualOwner,
                    investigatorRealOwner: ActiveInvestigator));
        }

        protected override void CheckCardEffectsAmount() { }
    }
}