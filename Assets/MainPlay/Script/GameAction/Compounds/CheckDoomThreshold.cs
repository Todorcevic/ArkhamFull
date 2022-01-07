using System.Collections;

namespace ArkhamGamePlay
{
    public class CheckDoomThreshold : GameAction
    {
        readonly CardAgenda currentAgenda;
        public override string GameActionInfo => "Comprobando umbral de Perdición.";
        public override GameActionType GameActionType => GameActionType.Compound;

        /*****************************************************************************************/
        public CheckDoomThreshold(CardAgenda currentAgenda) => this.currentAgenda = currentAgenda;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (currentAgenda.CheckAgendaAdvance())
                yield return new EffectAction(currentAgenda.AgendaAdvance, currentAgenda.AgendaAdvanceAnimation).RunNow();
        }
    }
}