using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

namespace ArkhamGamePlay
{
    public class CardPreview : MonoBehaviour
    {
        GameObject buffBoxClone;
        GameObject tokensClone;
        GameObject infoBoxClone;
        GameObject frontInfoClone;
        GameObject backInfoClone;
        GameObject glowClone;
        [SerializeField] MeshFilter cardMesh;
        [SerializeField] MeshRenderer showPreviewRend;
        [SerializeField] Material inactiveGlow;

        public void Active(CardComponent card)
        {
            Desactive();
            cardMesh.mesh = card.CardTools.CardMesh.mesh;
            showPreviewRend.sharedMaterial = card.CardTools.PreviewMaterial;
            glowClone = Instantiate(card.CardTools.GlowImage.gameObject, transform);
            glowClone.GetComponent<Image>().material = inactiveGlow;
            buffBoxClone = Instantiate(card.CardTools.BuffBox, transform);
            buffBoxClone.GetComponentsInChildren<BoxCollider>().ToList().ForEach(b => b.enabled = false);
            tokensClone = Instantiate(card.CardTools.TokensBox, transform);
            infoBoxClone = Instantiate(card.CardTools.InfoBox, transform);
            if (card.IsBack) backInfoClone = Instantiate(card.CardTools.InfoBack, transform);
            else frontInfoClone = Instantiate(card.CardTools.InfoFront, transform);
            gameObject.SetActive(true);
        }

        public void Desactive()
        {
            gameObject.SetActive(false);
            Destroy(glowClone);
            Destroy(buffBoxClone);
            Destroy(tokensClone);
            Destroy(infoBoxClone);
            Destroy(frontInfoClone);
            Destroy(backInfoClone);
        }

        void Update()
        {
            if (Input.GetKeyDown("a") && tokensClone) tokensClone.SetActive(false);
            if (Input.GetKeyUp("a") && tokensClone) tokensClone.SetActive(true);
        }
    }
}