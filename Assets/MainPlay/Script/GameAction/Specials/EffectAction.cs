using System.Collections;

namespace ArkhamGamePlay
{
    public class EffectAction : GameAction
    {
        readonly Effect effect;
        readonly Effect animation;
        public override GameActionType GameActionType => GameActionType.Basic;

        /*****************************************************************************************/
        public EffectAction(Effect effect, Effect animation = null)
        {
            this.effect = effect;
            this.animation = animation;
        }

        /*****************************************************************************************/
        protected override IEnumerator Animation() => animation?.Invoke();

        protected override IEnumerator ActionLogic() => effect?.Invoke();

    }
}