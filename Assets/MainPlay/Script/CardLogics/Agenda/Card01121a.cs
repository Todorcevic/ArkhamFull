using ArkhamGamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01121a : CardAgenda
    {
        CardComponent MaskedHunter => GameControl.GetCard("01121b");

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InvestigatorTurn investigatorTurn)
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => Resign(investigatorTurn.ActiveInvestigator),
                    animationEffect: () => ResignAnimation(investigatorTurn.ActiveInvestigator),
                    type: EffectType.Activate | EffectType.Resign,
                    name: "Desistir",
                    actionCost: 1));
        }

        IEnumerator ResignAnimation(InvestigatorComponent investigator) =>
            new AnimationCardAction(investigator.InvestigatorCardComponent, audioClip: investigator.InvestigatorCardComponent.Effect8).RunNow();

        IEnumerator Resign(InvestigatorComponent investigator)
        {
            investigator.IsResign = true;
            yield return new InvestigatorEliminatedAction(investigator).RunNow();
        }

        public override IEnumerator AgendaAdvance()
        {
            foreach (CardComponent card in GameControl.AllCardComponents.FindAll(c => c.DoomToken.Amount > 0))
                yield return new AddTokenAction(card.DoomToken, -card.DoomToken.Amount).RunNow();
            yield return new DiscardAction(ThisCard, outGame: true, withTopUp: true, withPreview: false).RunNow();
            yield return BackFace();
            yield return new ShowCardAction(GameControl.CurrentAgenda.ThisCard, withReturn: true).RunNow();
        }

        public override IEnumerator BackFace()
        {
            yield return new ShowCardAction(MaskedHunter).RunNow();
            yield return new SpawnEnemyAction(MaskedHunter).RunNow();
        }
    }
}
