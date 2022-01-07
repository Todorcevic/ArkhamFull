using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class AudioComponent : MonoBehaviour
    {
        public float delay;
        public AudioClip clipTotest;
        public Ease testEase;
        [SerializeField] List<AudioScriptableObject> audioList;

        public AudioScriptableObject LoadClips(string scriptableName) => audioList.Find(a => a.name == scriptableName);
    }
}
