using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using System.Linq;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class CardComponent : MonoBehaviour
    {
        CardState cardState;
        [SerializeField] CardTools cardTools;
        [SerializeField] CardSensor cardSensor;
        [SerializeField] SpriteRenderer currentGlow;
        [SerializeField] TokenComponent healthToken;
        [SerializeField] TokenComponent sanityToken;
        [SerializeField] TokenComponent resourcesToken;
        [SerializeField] TokenComponent cluesToken;
        [SerializeField] TokenComponent doomToken;
        [SerializeField] AudioClip turnClip;
        [SerializeField] AudioClip turnClip2;
        [SerializeField] AudioClip previewClip;
        [SerializeField] AudioClip moveToClip;
        [SerializeField] AudioClip moveFastClip;
        [SerializeField] AudioClip moveToClip2;
        [SerializeField] AudioClip moveFastClip2;
        [SerializeField] AudioClip idleClip;
        [SerializeField] AudioClip exhaustClip;
        public CardTools CardTools => cardTools;
        public CardSensor CardSensor => cardSensor;

        /************************************************************************************************/
        public event Action<bool> OnCanBePlayed; //Used in TokenStack only for the light
        /************************************************************************************************/
        public Card Info => Card.DataCardDictionary[ID];
        public string ID { get; set; }
        public bool IsBack { get; set; }
        public bool CanBePlayedNow
        {
            get => GameControl.CurrentInteractableAction?.CardEffects.Exists(c => c.Card == this) ?? false;
            set
            {
                if ((value && CardState == CardState.Active) || (!value && CardState == CardState.Inactive)) return;
                CardState = value ? CardState.Active : CardState.Inactive;
                OnCanBePlayed?.Invoke(value);
            }
        }
        public bool IsDiscarting { get; set; }
        public bool IsExausted { get; set; }
        public bool CanBeDiscard { get; set; } = true;
        public bool IsEliminated { get; set; }
        public bool IsOutGame => CurrentZone.ZoneType == Zones.CardBuilder && GameControl.GameIsStarted;
        public bool IsWeakness => Info.Subtype_code?.Contains("weakness") ?? false;
        public bool IsInPlay => (Zones.Investigator | Zones.Assets | Zones.Threat | Zones.Scenario | Zones.Location | Zones.IntoCard).HasFlag(CurrentZone.ZoneType)
            || GameControl.CurrentAct == CardLogic || GameControl.CurrentAgenda == CardLogic;
        public bool IsInFullPlay => IsInPlay || Zones.PlayCardZone.HasFlag(CurrentZone.ZoneType);
        public bool IsInCenterPreview => AllComponents.Table.CenterPreview.ListCards.Contains(this);
        public int? Position { get; set; }
        public int UniqueId { get; set; }
        public int Intellect => Info.Skill_intellect ?? default;
        public int Combat => Info.Skill_combat ?? default;
        public int Willpower => Info.Skill_willpower ?? default;
        public int Agility => Info.Skill_agility ?? default;
        public int Wild => Info.Skill_wild ?? default;
        public int VictoryPointModier { get; set; }
        public int VictoryPoint => VictoryPointModier + (Info.Victory ?? 0);
        public List<string> KeyWords => Info.Real_traits?.Split(new string[] { ". ", ".", " " }, StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
        public Zone CurrentZone { get; set; }
        public Zone MyOwnZone { get; set; }
        public CardLogic CardLogic { get; set; }
        public CardType CardType { get; set; }
        public CardState CardState
        {
            get => cardState;
            set
            {
                cardState = value;
                CardTools.CurrentGlow(cardState);
            }
        }
        public InvestigatorComponent Owner { get; set; }
        public InvestigatorComponent VisualOwner
        {
            get
            {
                if (IsOutGame) return Owner;
                if (Zones.PlayCardZone.HasFlag(CurrentZone.ZoneType)) return Owner;
                if (CurrentZone.ZoneType == Zones.TokenStack) return GameControl.CurrentInvestigator;
                return GameControl.AllInvestigatorsInGame.Find(i => Array.Exists(i.InvestigatorZones, z => z.ListCards.Contains(this)));
            }
        }
        public TokenComponent HealthToken => healthToken;
        public TokenComponent SanityToken => sanityToken;
        public TokenComponent ResourcesToken => resourcesToken;
        public TokenComponent CluesToken => cluesToken;
        public TokenComponent DoomToken => doomToken;
        public List<TokenComponent> AllTokens => new List<TokenComponent>() { healthToken, sanityToken, resourcesToken, cluesToken, doomToken };
        public AudioClip ShowCardFront => CardTools.ClipsForCard?.clip1;
        public AudioClip ShowCardBack => CardTools.ClipsForCard?.clip2;
        public AudioClip Effect1 => CardTools.ClipsForCard?.clip3;
        public AudioClip Effect2 => CardTools.ClipsForCard?.clip4;
        public AudioClip Effect3 => CardTools.ClipsForCard?.clip5;
        public AudioClip Effect4 => CardTools.ClipsForCard?.clip6;
        public AudioClip Effect5 => CardTools.ClipsForCard?.clip7;
        public AudioClip Effect6 => CardTools.ClipsForCard?.clip8;
        public AudioClip Effect7 => CardTools.ClipsForCard?.clip9;
        public AudioClip Effect8 => CardTools.ClipsForCard?.clip10;
        public AudioClip ClipType1 => CardTools.ClipsForType?.clip1;
        public AudioClip ClipType2 => CardTools.ClipsForType?.clip2;
        public AudioClip ClipType3 => CardTools.ClipsForType?.clip3;
        public AudioClip ClipType4 => CardTools.ClipsForType?.clip4;
        public AudioClip ClipType5 => CardTools.ClipsForType?.clip5;
        public AudioClip ClipType6 => CardTools.ClipsForType?.clip6;
        public AudioClip ClipType7 => CardTools.ClipsForType?.clip7;
        public AudioClip ClipType8 => CardTools.ClipsForType?.clip8;

        /************************************************************************************************/
        public Tween Idle()
        {
            cardTools.SecundaryAudioSource.loop = true;
            cardTools.SecundaryAudioSource.volume = 0;
            cardTools.SecundaryAudioSource.clip = idleClip;
            cardTools.SecundaryAudioSource.Play();
            cardTools.SecundaryAudioSource.DOFade(1f, GameData.ANIMATION_TIME_DEFAULT * 4);
            return transform.DOSpiral(3, speed: 0.025f, frequency: 50, depth: 0, mode: SpiralMode.ExpandThenContract)
             .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetId("Idle")
             .OnKill(() => cardTools.SecundaryAudioSource.DOFade(0, GameData.ANIMATION_TIME_DEFAULT * 4)
                .OnComplete(() => cardTools.SecundaryAudioSource.Stop()));
        }

        public Tween TurnDown(bool isBack, float timeAnimation = GameData.ANIMATION_TIME_DEFAULT, bool withAudio = true)
        {
            if (IsBack == isBack) return null;
            IsBack = isBack;
            return cardTools.RotationTransformCard.DOLocalRotate(isBack ? new Vector3(0, 0, 180) : new Vector3(0, 0, 0), timeAnimation)
              .SetDelay(timeAnimation / 2).SetEase(Ease.InOutQuad)
              .SetId("IsRotating").OnPlay(() => { if (withAudio) cardTools.PlayOneShotSound(isBack ? turnClip2 : turnClip); });
        }

        public Tween Preview(float timeAnimation = GameData.ANIMATION_TIME_DEFAULT, float timePause = 0f)
        {
            return DOTween.Sequence().Append(transform.DOMove(AllComponents.CenterPreview.position, timeAnimation * 2).SetEase(CurrentZone.ZoneType == Zones.Hand ? Ease.OutBack : Ease.InOutCubic, 1.2f))
             .Join(transform.DORotate(AllComponents.CenterPreview.rotation.eulerAngles, timeAnimation).SetDelay(timeAnimation / 2).SetEase(Ease.InOutCubic))
             .Join(transform.DOScale(AllComponents.CenterPreview.localScale, timeAnimation))
             .Append(transform.DOSpiral(timePause, speed: 0.025f, frequency: 50, depth: 0, mode: SpiralMode.ExpandThenContract).SetEase(Ease.Linear))
             .SetId("Preview").OnPlay(() => cardTools.PlayOneShotSound(previewClip));
        }

        public Tween Exhaust(bool isExhausted, float timeAnimation = GameData.ANIMATION_TIME_DEFAULT)
        {
            IsExausted = isExhausted;
            cardTools.PlayOneShotSound(exhaustClip);
            return DOTween.Sequence().Append(transform.DOLocalMoveY(1, timeAnimation))
                .AppendCallback(() => CardTools.ChangeColor(IsExausted ? GameData.ExaustedColor : Color.white))
                .Append(transform.DOLocalRotate(new Vector3(0, 0, 360), timeAnimation * 2, mode: RotateMode.FastBeyond360))
                .Append(transform.DOLocalMoveY(0, timeAnimation))
                .SetId("Exhausting");
        }

        public Tween PutAtPosition(int position, float timeAnimation = GameData.ANIMATION_TIME_DEFAULT)
        {
            return DOTween.Sequence().Append(transform.DOLocalMoveX(1, timeAnimation))
                .Append(transform.DOLocalMoveY(GameData.CARD_THICK * position, timeAnimation))
                .Append(transform.DOLocalMoveX(0, timeAnimation))
                .AppendCallback(() => CurrentZone.ZoneBehaviour.PreCardMove())
                .AppendCallback(() => CurrentZone.ZoneBehaviour.PostCardMove())
                .SetId("PutAtPosition");
        }

        public Tween MoveTo(Zone zone, float timeAnimation = GameData.ANIMATION_TIME_DEFAULT, bool? withAudio = true)
        {
            DOTween.Kill(UniqueId + "ExitCard");
            if (zone != CurrentZone) MoveLogic(zone);
            else AllComponents.Table.CenterPreview.ListCards.Remove(this);
            return DOTween.Sequence().Append(transform.DOMove(zone.ZoneBehaviour.StayCard.position, timeAnimation * 2).SetEase(Ease.InOutCubic).SetId("MoveTo2"))
             .Join(transform.DOScale(1, timeAnimation))
             .Join(transform.DOLocalRotate(zone.ZoneBehaviour.transform.localRotation.eulerAngles, timeAnimation).SetDelay(timeAnimation / 2).SetEase(Ease.InOutCubic))
             .PrependCallback(() => zone.ZoneBehaviour.PreCardMove())
             .AppendCallback(() => zone.ZoneBehaviour.PostCardMove()).SetId("MoveTo")
             .OnPlay(() => { if (withAudio != null) cardTools.PlayOneShotSound((bool)withAudio ? moveToClip : moveToClip2); });
        }

        public Tween MoveFast(Zone zone, int? indexPosition = null, float timeAnimation = GameData.ANIMATION_TIME_DEFAULT, bool? withAudio = true, bool withPreCard = true)
        {
            DOTween.Kill(UniqueId + "ExitCard");
            MoveLogic(zone, isFast: true, indexPosition: indexPosition);
            if (withPreCard) zone.ZoneBehaviour.PreCardMove();
            return DOTween.Sequence().Append(transform.DOMove(zone.ZoneBehaviour.StayCard.position, timeAnimation * 2).SetEase(Ease.InOutCubic))
                .Join(transform.DOScale(1, timeAnimation))
                .Join(transform.DOLocalRotate(zone.ZoneBehaviour.transform.localRotation.eulerAngles, timeAnimation).SetDelay(timeAnimation / 2).SetEase(Ease.InOutCubic))
                .SetId("MoveFast").OnPlay(() => { if (withAudio != null) cardTools.PlayOneShotSound((bool)withAudio ? moveFastClip : moveFastClip2); });
        }

        void MoveLogic(Zone zone, bool isFast = false, int? indexPosition = null)
        {
            transform.SetParent(zone.ZoneBehaviour.transform);
            SettingZones(zone);
            CardTools.CardCanvas.sortingOrder = 1;
            if (indexPosition < CurrentZone.ListCards.Count)
            {
                zone.InsertCard(this, (int)indexPosition);
                transform.SetSiblingIndex((int)indexPosition);
            }
            else zone.AddCard(this);
            if (!isFast && cardSensor.CurrentBehaviourZone)
            {
                cardSensor.CurrentBehaviourZone.PostCardMove();
                cardSensor.CurrentBehaviourZone.PreCardMove();
            }
            cardSensor.CurrentBehaviourZone = zone.ZoneBehaviour;
        }

        void SettingZones(Zone zone)
        {
            AllComponents.Table.CenterPreview.ListCards.Remove(this);
            CurrentZone?.RemoveCard(this);
            CurrentZone = zone;
        }
    }
}