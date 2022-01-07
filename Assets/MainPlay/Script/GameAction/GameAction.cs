using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public abstract class GameAction
    {
        bool isRunNow;
        readonly Queue<GameAction> flow = new Queue<GameAction>();
        List<CardComponent> CardsEvent => GameControl.AllCardComponents.FindAll(c => c.CurrentZone != AllComponents.CardBuilder.Zone);
        public GameAction ActionParent { get; set; }
        public virtual string GameActionInfo { get; }
        public virtual GameActionType GameActionType { get; }
        public bool IsActionCanceled { get; set; }
        public ChooseCardAction ChooseCardOptionalAction { get; set; }

        /*****************************************************************************************/
        protected abstract IEnumerator ActionLogic();

        public IEnumerator RunNow()
        {
            isRunNow = true;
            ActionParent = GameControl.CurrentAction;
            yield return StartAction();
        }

        public void AddActionTo() => AddActionTo(GameControl.CurrentAction);

        public void AddActionTo(GameAction actionParent)
        {
            ActionParent = actionParent;
            ActionParent.flow.Enqueue(this);
        }

        IEnumerator StartAction()
        {
            GameControl.CurrentAction = this;
            if (GameActionInfo != null) yield return AllComponents.PhasesUI.ShowInfo(GameActionInfo);
            yield return Animation();
            ChooseCardOptionalAction = new ChooseCardAction(new List<CardEffect>(), isOptionalChoice: true, isLoop: true, withPreview: false);
            CardsEvent.ForEach(c => c.CardLogic.BeginGameActionEvents(this));
            if (ChooseCardOptionalAction.ChosenCardEffects.Count > 0) ChooseCardOptionalAction.AddActionTo();
            if (flow.Count > 0) yield return flow.Dequeue().StartAction();
            if (!IsActionCanceled)
            {
                yield return ActionLogic();
                ChooseCardOptionalAction = new ChooseCardAction(new List<CardEffect>(), isOptionalChoice: true, isLoop: true);
                CardsEvent.ForEach(c => c.CardLogic.EndGameActionEvents(this));
                if (ChooseCardOptionalAction.ChosenCardEffects.Count > 0) ChooseCardOptionalAction.AddActionTo();
                if (flow.Count > 0) yield return flow.Dequeue().StartAction();
            }
            GameControl.CurrentAction = ActionParent;
            if (ActionParent.flow.Count > 0 && !isRunNow) yield return ActionParent.flow.Dequeue().StartAction();
        }

        protected virtual IEnumerator Animation() => null;
    }
}