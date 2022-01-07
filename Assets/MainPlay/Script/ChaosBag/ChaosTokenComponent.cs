using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class ChaosTokenComponent : MonoBehaviour
    {
        int? value;
        [SerializeField] List<AudioClip> soundsTable;
        [SerializeField] List<AudioClip> soundsTokens;
        [SerializeField] List<AudioClip> returnTokens;
        [SerializeField] AudioSource audioSource;
        [SerializeField] Renderer meshRenderer;
        [SerializeField] Rigidbody rigidBody;
        public ChaosTokenType Type { get; set; }
        public Sprite ImageToken { get; set; }
        public Renderer MeshRenderer => meshRenderer;
        public Rigidbody RigidBody => rigidBody;
        CardScenario ScenarioCard => GameControl.CurrentScenarioCard;
        public int? Value
        {
            get
            {
                if (value == null) return ScenarioCard.GetTokenValue(Type);
                return (int)value;
            }
            set => this.value = value;
        }
        public Effect Effect
        {
            get
            {
                if (!(ChaosTokenType.Basic).HasFlag(Type)) return ScenarioCard.GetTokenEffect(Type);
                return null;
            }
        }
        public AudioSource AudioSource { get => audioSource; set => audioSource = value; }

        /*****************************************************************************************/
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Table"))
                audioSource.PlayOneShot(soundsTable[Random.Range(0, 5)], Mathf.Clamp(collision.relativeVelocity.magnitude / 2.5f, 0.2f, 0.8f));
            if (collision.gameObject.CompareTag("ChaosToken"))
                audioSource.PlayOneShot(soundsTokens[Random.Range(0, 7)], Mathf.Clamp(collision.relativeVelocity.magnitude / 2.5f, 0, 1));
        }

        public void PlaySoundReturnToken() => audioSource.PlayOneShot(returnTokens[Random.Range(0, 3)]);
    }
}