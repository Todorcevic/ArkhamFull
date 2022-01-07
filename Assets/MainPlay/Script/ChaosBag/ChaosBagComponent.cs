using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class ChaosBagComponent : MonoBehaviour
    {
        [SerializeField] Material[] tokenMaterials;
        [SerializeField] Sprite[] tokenSprite;
        public ChaosTokenComponent tokenPrefab;
        public Transform tokenPreview;
        public Transform tokenLeft;
        public Transform tokenRight;
        public List<ChaosTokenComponent> tokenList = new List<ChaosTokenComponent>();
        public List<ChaosTokenComponent> outTokenList = new List<ChaosTokenComponent>();

        /************************************************************************************************/
        public IEnumerator SetChaosBag(List<string> chaosBagList, bool isTestMode = false)
        {
            int i = 0;
            foreach (string token in chaosBagList)
            {
                i++;
                ChaosTokenComponent tokenComponent = Instantiate(tokenPrefab, transform.position + new Vector3(0, i, 0), transform.rotation, transform);
                SetToken(tokenComponent, token);
                tokenList.Add(tokenComponent);
            }
            if (!isTestMode) yield return GetAnimation(tokenList.ToArray());
        }

        IEnumerator GetAnimation(ChaosTokenComponent[] allTokens)
        {
            foreach (ChaosTokenComponent token in allTokens)
                yield return DropToken(token);
            yield return new WaitForSeconds(GameData.ANIMATION_TIME_DEFAULT * 8);
            foreach (ChaosTokenComponent token in allTokens)
                yield return ReturnToken(token);
        }

        public ChaosTokenComponent RandomChaosToken() => tokenList[UnityEngine.Random.Range(0, tokenList.Count - 1)];

        public IEnumerator DropToken(ChaosTokenComponent token)
        {
            tokenList.Remove(token);
            outTokenList.Add(token);
            token.gameObject.SetActive(true);
            token.RigidBody.isKinematic = false;
            token.RigidBody.AddForce(transform.up * UnityEngine.Random.Range(1f, 10f), ForceMode.Impulse);
            token.RigidBody.AddTorque(new Vector3(1, 2, 5), ForceMode.Impulse);
            yield return null;
        }
        public IEnumerator ReturnToken(ChaosTokenComponent token)
        {
            //token.PlaySoundReturnToken2();
            outTokenList.Remove(token);
            tokenList.Add(token);
            token.RigidBody.isKinematic = true;
            yield return DOTween.Sequence().Append(token.transform.DOMove(tokenPreview.position, GameData.ANIMATION_TIME_DEFAULT))
                .Join(token.transform.DORotate(tokenPreview.rotation.eulerAngles, GameData.ANIMATION_TIME_DEFAULT))
                .AppendCallback(token.PlaySoundReturnToken)
                .Append(token.transform.DOLocalMove(Vector3.zero, GameData.ANIMATION_TIME_DEFAULT))
                .SetId("ReturnToken").WaitForCompletion();
            token.gameObject.SetActive(false);
        }

        public IEnumerator ReturnAllTokens()
        {
            foreach (ChaosTokenComponent token in outTokenList.ToArray())
                yield return ReturnToken(token);
        }

        void SetToken(ChaosTokenComponent tokenComponent, string typeToken)
        {
            tokenComponent.MeshRenderer.sharedMaterial = Array.Find(tokenMaterials, c => c.name == typeToken);
            tokenComponent.ImageToken = Array.Find(tokenSprite, c => c.name == typeToken);

            string tokenNameWithoutPrefix = typeToken.Substring(5);
            try
            {
                if (Enum.IsDefined(typeof(ChaosTokenType), tokenNameWithoutPrefix))
                    tokenComponent.Type = (ChaosTokenType)Enum.Parse(typeof(ChaosTokenType), tokenNameWithoutPrefix, true);
                else throw new ArgumentException("Not in Enum", tokenNameWithoutPrefix);
            }
            catch
            {
                tokenComponent.Type = ChaosTokenType.Basic;
                tokenComponent.Value = int.Parse(tokenNameWithoutPrefix);
            }
        }
    }
}