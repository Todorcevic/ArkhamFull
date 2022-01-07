using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
namespace ArkhamGamePlay
{
    public class TokenStack : MonoBehaviour
    {
        readonly DoubleClick doubleClick = new DoubleClick();
        [SerializeField] CardComponent cardToken;
        [SerializeField] TokenComponent healthToken;
        [SerializeField] TokenComponent sanityToken;
        [SerializeField] TokenComponent resourcesToken;
        [SerializeField] TokenComponent cluesToken;
        [SerializeField] TokenComponent doomToken;
        [SerializeField] Light lightComponent;
        public TokenComponent HealthToken { get => healthToken; }
        public TokenComponent SanityToken { get => sanityToken; }
        public TokenComponent ResourcesToken { get => resourcesToken; }
        public TokenComponent CluesToken { get => cluesToken; }
        public TokenComponent DoomToken { get => doomToken; }
        public List<TokenComponent> AllTokens => new List<TokenComponent> { HealthToken, SanityToken, ResourcesToken, CluesToken, DoomToken };

        /*****************************************************************************************/
        public void SettingTokenStack()
        {
            cardToken.ID = "00000";
            cardToken.CurrentZone = AllComponents.Table.TokenStack;
            cardToken.CardSensor.CurrentBehaviourZone = AllComponents.Table.PlayZonesBehaviour[7];
            cardToken.OnCanBePlayed += ActivateStack;
            cardToken.CardLogic = new CardToken().WithThisCard(cardToken);
            HealthToken.TokenInThisCard = SanityToken.TokenInThisCard = ResourcesToken.TokenInThisCard = CluesToken.TokenInThisCard = DoomToken.TokenInThisCard = cardToken;
            GameControl.AllCardComponents.Add(cardToken);
        }

        void ActivateStack(bool canBePlayed) => lightComponent.enabled = canBePlayed;

        void OnMouseEnter()
        {
            if (cardToken.CardSensor.IsInteractable && !EventSystem.current.IsPointerOverGameObject())
            {
                cardToken.CardSensor.OnMouseEnter();
                lightComponent.intensity = 8;
            }
        }

        void OnMouseExit()
        {
            cardToken.CardSensor.OnMouseExit();
            lightComponent.intensity = 4;
        }

        void OnMouseUpAsButton()
        {
            if (cardToken.CardSensor.IsInteractable && cardToken.CanBePlayedNow && !EventSystem.current.IsPointerOverGameObject())
            {
                GameControl.CurrentInteractableAction.CardSelected(cardToken);
                if (doubleClick.IsDetected(Time.time))
                {
                    cardToken.CardTools.PlaySoundCard();
                    cardToken.CardSensor.OnMouseExit();
                    GameControl.CurrentInteractableAction.CardPlay(cardToken);
                }
                else cardToken.CardSensor.CurrentBehaviourZone.ClickCard(cardToken);
            }
        }
    }
}