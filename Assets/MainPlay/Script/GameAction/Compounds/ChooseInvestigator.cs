using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class ChooseInvestigator : GameAction
    {
        public override string GameActionInfo => "Selección de Investigador.";
        List<CardComponent> PlayCards => GameControl.AllInvestigatorsInGame.Where(i => !i.IsExausted).Select(c => c.PlayCard).ToList();
        readonly List<CardEffect> cardEffects = new List<CardEffect>();

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            foreach (CardComponent playCard in PlayCards)
                cardEffects.Add(new CardEffect(
                    card: playCard,
                    effect: () => ChoosingInvestigator(playCard),
                    animationEffect: () => Animation(playCard),
                    type: EffectType.Choose,
                    name: "Jugar el turno de " + playCard.Info.Name,
                    investigatorImageCardInfoOwner: playCard.Owner));
            yield return new ChooseCardAction(cardEffects, isOptionalChoice: false, isFastAction: true).RunNow();
        }

        IEnumerator ChoosingInvestigator(CardComponent playCard) =>
            new ActiveInvestigatorAction(playCard.Owner).RunNow();

        IEnumerator Animation(CardComponent playCard) =>
          new AnimationCardAction(playCard, audioClip: playCard.Owner.InvestigatorCardComponent.Effect1).RunNow();
    }
}