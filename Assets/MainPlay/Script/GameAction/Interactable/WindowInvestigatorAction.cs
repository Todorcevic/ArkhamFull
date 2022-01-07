using System.Collections;

namespace ArkhamGamePlay
{
    public class WindowInvestigatorAction : InteractableAction
    {
        public override string GameActionInfo => "Prioridad de los Investigadores.";
        public override bool CanPlayFastAction => true;

        /*****************************************************************************************/
        public WindowInvestigatorAction()
        {
            GameControl.ActiveInvestigator = GameControl.LeadInvestigator;
        }
        /*****************************************************************************************/
        public override void SetButton()
        {
            if (!AnyIsClicked) AllComponents.ReadyButton.SetReadyButton(this, ButtonState.Ready);
        }

        public override void ReadyClicked(ReadyButton button)
        {
            if (cardSelected != null) CardPlay(cardSelected);
            AnyIsClicked = true;
        }

        protected override void PlayableCards() { }
    }
}