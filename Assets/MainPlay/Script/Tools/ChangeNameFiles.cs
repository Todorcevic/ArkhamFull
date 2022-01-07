using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class ChangeNameFiles
    {
        public void ChangeName()
        {
            int i = 0;
            foreach (Card card in Card.DataCard)
            {
                if (card.Octgn_id != null)
                {
                    string[] real_otgn_id = card.Octgn_id.Split(new System.Char[] { ':' });
                    if (File.Exists("d:/AllCards/" + real_otgn_id[0] + ".png"))
                        File.Move("d:/AllCards/" + real_otgn_id[0] + ".png", "d:/AllCardsRenamed/" + card.Code + ".png");
                    else i++;
                    if (File.Exists("d:/AllCards/" + real_otgn_id[0] + ".b.png"))
                        File.Move("d:/AllCards/" + real_otgn_id[0] + ".b.png", "d:/AllCardsRenamed/" + card.Code + "b.png");
                    if (File.Exists("d:/AllCards/" + real_otgn_id[0] + ".B.png"))
                        File.Move("d:/AllCards/" + real_otgn_id[0] + ".B.png", "d:/AllCardsRenamed/" + card.Code + "b.png");
                }
            }
            Debug.Log(i + "cards without image");
        }
    }
}