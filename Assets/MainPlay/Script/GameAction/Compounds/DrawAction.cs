using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class DrawAction : GameAction
    {
        bool withShow;
        readonly Zone deckToDraw;
        readonly Zone deckDiscardPile;
        readonly InvestigatorComponent investigator;
        public override GameActionType GameActionType => GameActionType.Compound;
        public CardComponent CardToDraw { get; set; }

        /*****************************************************************************************/
        public DrawAction(InvestigatorComponent investigator, CardComponent cardToDraw, bool withShow = false)
        {
            this.investigator = investigator;
            CardToDraw = cardToDraw;
            deckToDraw = cardToDraw.CurrentZone;
            this.withShow = withShow;
        }

        public DrawAction(InvestigatorComponent investigator, bool isEncounter = false, bool withShow = true)
        {
            this.investigator = investigator;
            this.withShow = withShow;
            if (isEncounter)
            {
                deckToDraw = AllComponents.Table.EncounterDeck;
                deckDiscardPile = AllComponents.Table.EncounterDiscard;
            }
            else
            {
                deckToDraw = investigator.InvestigatorDeck;
                deckDiscardPile = investigator.InvestigatorDiscard;
            }
            CardToDraw = deckToDraw.ListCards.LastOrDefault();
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (investigator.IsDefeat) yield break;
            yield return new SelectInvestigatorAction(investigator).RunNow();
            if (CardToDraw == null) yield return DeckVoid(deckDiscardPile);
            if (withShow || CardToDraw.CardLogic is CardEnemy || CardToDraw.CardLogic is IRevelation) yield return new ShowCardAction(CardToDraw).RunNow();
            else yield return new AnimationCardAction(CardToDraw, isBack: false, withReturn: false, audioClip: CardToDraw.ShowCardFront).RunNow();
            if (CardToDraw.CardLogic is CardEnemy) yield return new SpawnEnemyAction(CardToDraw, investigator).RunNow();
            else if (CardToDraw.CardLogic is IRevelation) yield return new RevelationAction(CardToDraw).RunNow();
            else if (deckToDraw.ZoneType == Zones.InvestigatorDeck)
                yield return new MoveCardAction(CardToDraw, investigator.Hand, withPreview: false, isBack: false, previewPause: 0).RunNow();
        }

        IEnumerator DeckVoid(Zone discardPile)
        {
            yield return new MoveDeck(discardPile.ListCards, deckToDraw, isBack: true, withMoveUp: true).RunNow();
            yield return new ShuffleAction(deckToDraw).RunNow();
            CardToDraw = deckToDraw.ListCards.LastOrDefault();
            if (discardPile.InvestigatorOwer != null)
                yield return new AssignDamageHorror(discardPile.InvestigatorOwer, horrorAmount: 1).RunNow();
        }
    }
}