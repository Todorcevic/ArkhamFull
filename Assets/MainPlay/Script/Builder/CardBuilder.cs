using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Runtime.Remoting;
using ArkhamShared;
using Newtonsoft.Json;

namespace ArkhamGamePlay
{
    public class CardBuilder : MonoBehaviour
    {
        static int cardsAmount;
        List<Sprite> cardsSprite;
        List<Texture> cardsTexture;
        [SerializeField] CardComponent cardVPrefab;
        [SerializeField] CardComponent cardHPrefab;
        [SerializeField] Material materialBase;
        [SerializeField] SpritesCards cardsSpriteEN;
        [SerializeField] SpritesCards cardsSpriteES;
        [SerializeField] List<TokenComponent> tokens;
        [SerializeField] ZoneBehaviour zoneBehaviour;
        [SerializeField] Sprite tokenGlow;
        public Zone Zone { get; set; }
        public ZoneBehaviour ZoneBehaviour { get => zoneBehaviour; set => zoneBehaviour = value; }
        public List<TokenComponent> Tokens { get => tokens; set => tokens = value; }
        public Sprite TokenGlow => tokenGlow;

        /*****************************************************************************************/
        public void Initializer()
        {
            GameControl.AllCardComponents = new List<CardComponent>();

            switch (GameData.Instance.Language)
            {
                default:
                case Language.EN:
                    {
                        cardsSprite = cardsSpriteEN.cardsSprite;
                        cardsTexture = cardsSpriteEN.cardsTexture;
                        break;
                    }
                case Language.ES:
                    {
                        cardsSprite = cardsSpriteES.cardsSprite;
                        cardsTexture = cardsSpriteES.cardsTexture;
                        break;
                    }
            }
        }

        public CardComponent BuildCard(string idCard, string backFace)
        {
            Texture front = GetTexure(idCard);
            Texture back = GetTexure(backFace);
            CardComponent card = CreatingCard(front, back, idCard);
            card.CardTools.SpriteOwner = GetSprite(idCard);
            card.CardType = (CardType)Enum.Parse(typeof(CardType), card.Info.Type_code ?? "None", true);
            card.CardTools.ClipsForCard = AllComponents.AudioComponent.LoadClips(idCard);
            card.CardTools.ClipsForType = AllComponents.AudioComponent.LoadClips(card.CardType.ToString());
            card.CardLogic = AddCardLogic(card);
            card.CardTools.ActiveInfo();
            return card;
        }

        public CardComponent BuildPlayCard(string idCard)
        {
            string[] real_otgn_id = Card.DataCardDictionary[idCard].Octgn_id.Split(':');
            Texture front = GetTexure(real_otgn_id[1]);
            Texture back = GetTexure(real_otgn_id[1] + ".B");
            CardComponent playCard = CreatingCard(front, back, idCard);
            playCard.CardTools.SpriteOwner = GetSprite(real_otgn_id[1]);
            playCard.CardType = CardType.PlayCard;
            playCard.CardTools.ClipsForCard = AllComponents.AudioComponent.LoadClips(idCard);
            playCard.CardTools.ClipsForType = AllComponents.AudioComponent.LoadClips(playCard.CardType.ToString());
            playCard.CardLogic = new CardLogic().WithThisCard(playCard);
            return playCard;
        }

        CardComponent CreatingCard(Texture front, Texture back, string idCard)
        {
            CardComponent card = (front.height > front.width) ? Instantiate(cardVPrefab, transform) : Instantiate(cardHPrefab, transform);
            card.CardTools.RendFront.material = new Material(materialBase);
            card.CardTools.RendFront.material.SetTexture("_MainTex", front);
            card.CardTools.RendBack.material = new Material(materialBase);
            card.CardTools.RendBack.material.SetTexture("_MainTex", back);
            card.ID = card.gameObject.name = idCard;
            card.UniqueId = ++cardsAmount;
            card.MyOwnZone = new Zone(Zones.IntoCard)
            {
                ZoneBehaviour = card.CardSensor.StackerZone,
                ThisCard = card
            };
            card.CurrentZone = Zone;
            GameControl.AllCardComponents.Add(card);
            return card;
        }

        public Sprite GetSprite(string idCard) => cardsSprite.Find(c => c.name == idCard);

        public Texture GetTexure(string idCard) => cardsTexture.Find(c => c.name == idCard);

        CardLogic AddCardLogic(CardComponent card)
        {
            try
            {
                ObjectHandle handle = Activator.CreateInstance(null, "ArkhamGamePlay.Card" + card.ID);
                return ((CardLogic)handle.Unwrap()).WithThisCard(card);
            }
            catch (TypeLoadException)
            {
                return new CardLogic().WithThisCard(card);
            }
        }

        public void BuildScenario()
        {
            GameControl.Deck = new Dictionary<DeckType, List<CardComponent>>();
            foreach (TextAsset textAsset in Resources.LoadAll(GameFiles.ScenarioPath, typeof(TextAsset)))
            {
                List<CardComponent> deckList = new List<CardComponent>();
                foreach (string idCard in JsonConvert.DeserializeObject<IEnumerable<string>>(textAsset.text).Reverse())
                    deckList.Add(BuildCard(idCard, GetSprite(idCard + "b") ? idCard + "b" : GameFiles.ENCOUNTER_BACK_IMAGE));
                GameControl.Deck.Add((DeckType)Enum.Parse(typeof(DeckType), textAsset.name, true), deckList);
            }
        }
    }
}