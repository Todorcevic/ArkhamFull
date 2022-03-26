using System.Collections;
using System.Linq;
using UnityEngine;

namespace ArkhamGamePlay
{
    public abstract class CardAct : CardLogic
    {
        int Multiplier => (bool)ThisCard.Info.Clues_fixed ? 1 : GameControl.InvestigatorsStartingAmount;
        public int? CluesNeeded => ThisCard.Info.Clues * Multiplier;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InteractableAction interactableAction && GiveClues(interactableAction))
                interactableAction.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => new AssignClues(GameControl.AllInvestigatorsInGame.FindAll(c => c.InvestigatorCardComponent.CluesToken.Amount > 0)).RunNow(),
                    type: EffectType.Instead,
                    name: "Entregar pistas al Plan"
                    ));
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is AddTokenAction addToken && CheckActAdvance(addToken))
                new EffectAction(ActAdvance, ActAdvanceAnimation).AddActionTo();
        }

        bool GiveClues(InteractableAction interactableAction)
        {
            if (!interactableAction.CanPlayFastAction) return false;
            if (GameControl.AllInvestigatorsInGame.Sum(c => c.InvestigatorCardComponent.CluesToken.Amount) < CluesNeeded) return false;
            return true;
        }

        bool CheckActAdvance(AddTokenAction addTokenAction)
        {
            if (addTokenAction.ToToken != ThisCard.CluesToken) return false;
            if (ThisCard.CluesToken.Amount < ThisCard.Info.Clues * Multiplier) return false;
            return true;
        }

        protected IEnumerator ActAdvanceAnimation()
        {
            ThisCard.CardTools.PlayOneShotSound(ThisCard.ClipType1);
            yield return new WaitWhile(() => ThisCard.CardTools.AudioSource.isPlaying);
        }

        protected IEnumerator ActAdvance()
        {
            yield return new AddTokenAction(ThisCard.CluesToken, -ThisCard.CluesToken.Amount).RunNow();
            yield return new ShowCardAction(ThisCard, isBack: true, withReturn: true).RunNow();
            yield return BackFace();
            yield return new DiscardAction(ThisCard, outGame: true, withTopUp: true, withPreview: false).RunNow();
            yield return new ShowCardAction(GameControl.CurrentAct.ThisCard, withReturn: true).RunNow();
        }

        protected abstract IEnumerator BackFace();
    }
}