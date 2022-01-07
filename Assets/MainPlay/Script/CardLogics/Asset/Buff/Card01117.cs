using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01117 : CardAsset, IBuffable
    {
        InvestigatorComponent investigatorSelected;
        readonly List<InvestigatorComponent> investigatorAfected = new List<InvestigatorComponent>();

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is SettingInvestigatorAttack attackAction && AllyAttack(attackAction))
                attackAction.WinEffect = new ActionsTools().JoinEffects(() => LitaHelp(attackAction), attackAction.WinEffect);
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is AddTokenAction addToken && CheckDie(addToken))
                new EffectAction(Discard, DiscardAnimation).AddActionTo();

            IEnumerator DiscardAnimation()
            {
                ThisCard.CardTools.PlayOneShotSound(ThisCard.Effect6);
                yield return new WaitWhile(() => ThisCard.CardTools.AudioSource.isPlaying);
            }

            IEnumerator Discard() => new DiscardAction(ThisCard, outGame: true).RunNow();
        }

        bool AllyAttack(SettingInvestigatorAttack attackAction)
        {
            if (!ThisCard.IsInPlay) return false;
            if (GameControl.ActiveInvestigator.CurrentLocation != ThisCard.VisualOwner?.CurrentLocation) return false;
            if (!attackAction.Enemy.KeyWords.Contains("Monster")) return false;
            return true;
        }

        IEnumerator LitaHelp(SettingInvestigatorAttack attackAction)
        {
            yield return new ChooseCardAction(
                new CardEffect(card: ThisCard,
                effect: () => BonusDamage(attackAction),
                animationEffect: BonusDamageAnimation,
                type: EffectType.Reaction,
                name: "Ayuda de " + ThisCard.Info.Name + ": +1 de daño"), isOptionalChoice: true).RunNow();
            yield return null;
        }

        IEnumerator BonusDamageAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator BonusDamage(SettingInvestigatorAttack attackAction)
        {
            attackAction.Damage++;
            yield return null;
        }

        bool IBuffable.ActiveBuffCondition(GameAction gameAction)
        {
            if (!ThisCard.Owner) return false;
            foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame)
            {
                investigatorSelected = investigator;
                if (ThisCard.IsInPlay && investigator.CurrentLocation == ThisCard.Owner.CurrentLocation && !investigatorAfected.Contains(investigator))
                    ((IBuffable)this).BuffEffect();
                else if ((!ThisCard.IsInPlay || investigator.CurrentLocation != ThisCard.Owner?.CurrentLocation) && investigatorAfected.Contains(investigator))
                    ((IBuffable)this).DeBuffEffect();
            }
            return false;
        }

        void IBuffable.BuffEffect()
        {
            investigatorSelected.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);
            investigatorAfected.Add(investigatorSelected);
            investigatorSelected.CombatBonus++;
        }

        void IBuffable.DeBuffEffect()
        {
            investigatorSelected.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            investigatorAfected.Remove(investigatorSelected);
            investigatorSelected.CombatBonus--;
        }
    }
}