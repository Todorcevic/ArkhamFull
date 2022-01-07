using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public enum DirectionButton { up, down }

    public class TokenButton : MonoBehaviour
    {
        [SerializeField] TokenComponent thisToken;
        [SerializeField] DirectionButton direction;
        [SerializeField] SpriteRenderer sRenderer;

        public DirectionButton Direction => direction;

        /*****************************************************************************************/
        public void OnMouseEnter()
        {
            if (thisToken.TokenInThisCard.CardSensor.IsInteractable)
            {
                thisToken.TokenInThisCard.CardSensor.CurrentBehaviourZone.PointEnterCardAnimation(thisToken.TokenInThisCard);
                sRenderer.color = Color.yellow;
            }
        }

        public void OnMouseExit()
        {
            if (thisToken.TokenInThisCard.CardSensor.IsInteractable)
            {
                thisToken.TokenInThisCard.CardSensor.CurrentBehaviourZone.PointExitCardAnimation(thisToken.TokenInThisCard);
                sRenderer.color = Color.green;
            }
        }

        public void OnMouseUpAsButton()
        {
            thisToken.PlaySoundButton();
            if (direction == DirectionButton.up) thisToken.AssignValue++;
            else thisToken.AssignValue--;
            GameControl.CurrentInteractableAction.SetButton();
        }
    }
}