using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using ArkhamShared;
using ArkhamGamePlay;

namespace ArkhamMenu
{
    public static class ChangeNameFiles
    {
        public static void CreateUrlImageToText(Card[] listCards, string nameFile)
        {
            List<string> url = new List<string>();
            var sql = from card in listCards
                      where card.Imagesrc != null
                      select "https://es.arkhamdb.com" + card.Imagesrc;
            //var sql = listCards.Select(card =>"https://es.arkhamdb.com" + card.backimagesrc ).Where(backimagesrc => backimagesrc != null); //Lambda expression for practice
            url = sql.ToList();
            File.WriteAllLines(GameFiles.RESOURCE_PATH + nameFile + ".txt", url);
        }
    }
}
