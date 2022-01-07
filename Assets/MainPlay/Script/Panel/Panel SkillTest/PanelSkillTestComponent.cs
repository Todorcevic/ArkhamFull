using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using System.Linq;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class PanelSkillTestComponent : ControlPanelComponent
    {
        [SerializeField] Sprite[] skillsImage;
        [SerializeField] TextMeshProUGUI[] listPercent;
        [SerializeField] TextMeshProUGUI titlePanel;
        [SerializeField] TextMeshProUGUI value;
        [SerializeField] TextMeshProUGUI testValue;
        [SerializeField] Image skill;
        [SerializeField] Image imageSkillTest;
        [SerializeField] SkillTestImageSensor imageSensor;
        [SerializeField] ModifierComponent modifierPrefab;
        [SerializeField] Transform modifierTransform;
        [SerializeField] ReadyButton button;
        [SerializeField] List<ModifierComponent> listModifiers;
        [SerializeField] ModifierComponent investigatorTest;
        [SerializeField] ModifierComponent extraCard;
        [SerializeField] AudioClip winClip;
        [SerializeField] AudioClip loseClip;
        SkillTest SkillTest { get; set; }

        /*****************************************************************************************/
        public void SetPanel(SkillTest skillTest)
        {
            investigatorTest.Active(GameControl.ActiveInvestigator.InvestigatorCardComponent);
            SkillTest = skillTest;
            extraCard.Active(SkillTest.ExtraCard);
            titlePanel.text = GameControl.CurrentInteractableAction.ActiveInvestigator.InvestigatorCardComponent.Info.Name + " " + skillTest.Title;
            skill.sprite = CheckSkillImage(skillTest.SkillType);
            imageSkillTest.sprite = AllComponents.CardBuilder.GetSprite(skillTest.CardToTest.IsBack ? skillTest.CardToTest.ID + "b" : skillTest.CardToTest.ID);
            imageSensor.Card = skillTest.CardToTest;
            UpdatePanel();
            StartCoroutine(SelectThisPanel());
        }

        public void AddModifier(CardComponent card, int mod)
        {
            SkillTest.UpdateModifier(mod);
            listModifiers.Find(m => !m.isActiveAndEnabled).Active(card);
        }

        public IEnumerator AddModifier(CardComponent card)
        {
            SkillTest.Modifiers.Add(card);
            listModifiers.Find(m => !m.isActiveAndEnabled).Active(card);
            SkillTest.UpdateModifier(card.CardTools.GetModifierValue(SkillTest.SkillType));
            yield return new MoveCardAction(card, AllComponents.Table.PlayCardZone).RunNow();
        }

        public IEnumerator RemoveModifier(CardComponent card)
        {
            SkillTest.Modifiers.Remove(card);
            listModifiers.Find(m => m.Card == card).Active(null);
            SkillTest.UpdateModifier(-card.CardTools.GetModifierValue(SkillTest.SkillType));
            yield return AllComponents.InvestigatorManagerComponent.SelectInvestigator(card.Owner);
            if (card.CurrentZone.ListCards.Last() != card)
                yield return card.transform.DOLocalMoveX(1, GameData.ANIMATION_TIME_DEFAULT).WaitForCompletion();
            yield return new MoveCardAction(card, card.Owner.Hand).RunNow();
        }

        public void UpdatePanel()
        {
            value.text = SkillTest.TotalInvestigatorValue.ToString();
            testValue.text = SkillTest.TotalTestValue.ToString();
            SetPercentList();

            void SetPercentList()
            {
                int mod = -3;
                foreach (TextMeshProUGUI percent in listPercent)
                {
                    double porcent = CalculatePosibilities(mod++);
                    percent.text = porcent.ToString() + "%";
                    if (mod == 1) ReadyButton.SkillTestButtonGlowColor((float)porcent / 100);
                }
            }

            double CalculatePosibilities(int mod)
            {
                int winner = 0;
                int modifierBackup = SkillTest.InitialModifier;
                foreach (ChaosTokenComponent token in AllComponents.ChaosBag.tokenList)
                {
                    SkillTest.InitialModifier += mod;
                    SkillTest.InitialModifier += (int)token.Value;
                    if (SkillTest.TotalInvestigatorValue >= SkillTest.TotalTestValue && token.Type != ChaosTokenType.Fail) winner++;
                    SkillTest.InitialModifier = modifierBackup;
                }
                return Math.Round(((float)winner / AllComponents.ChaosBag.tokenList.Count) * 100);
            }
        }

        public void ShowResult(SkillTest skillTest)
        {
            value.text = skillTest.TotalInvestigatorValue.ToString();
            testValue.text = SkillTest.TotalTestValue.ToString();
            imageSkillTest.sprite = skillTest.TokenThrow?.ImageToken ?? imageSkillTest.sprite;
            button.State = skillTest.IsWin ? ButtonState.Ready : ButtonState.StandBy;
            button.ChangeButtonText(skillTest.IsWin ? "You WIN" : "You LOSE");
            button.AudioSource.PlayOneShot(skillTest.IsWin ? winClip : loseClip);
        }

        Sprite CheckSkillImage(Skill skill)
        {
            switch (skill)
            {
                case Skill.Willpower: return skillsImage[0];
                case Skill.Intellect: return skillsImage[1];
                case Skill.Combat: return skillsImage[2];
                case Skill.Agility: return skillsImage[3];
                default: throw new ArgumentException("Wrong skill type", skill.ToString());
            }
        }

        public void CleanPanel() => listModifiers.FindAll(m => m.isActiveAndEnabled).ForEach(m => m.Active(null));
    }
}