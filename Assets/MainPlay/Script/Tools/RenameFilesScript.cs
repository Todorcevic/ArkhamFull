using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class RenameFilesScript
    {
        public void Rename()
        {
            DirectoryInfo di = new DirectoryInfo(GameFiles.RESOURCE_PATH + "output");
            foreach (FileInfo fi in di.GetFiles("*.png"))
            {
                string fileName = Path.GetFileNameWithoutExtension(fi.FullName);
                if (Card.DataCard.ToList().Exists(f => f.Octgn_id?.Contains(fileName) ?? false))
                {
                    Card card = Card.DataCard.ToList().Find(f => f.Octgn_id?.Contains(fileName) ?? false);
                    File.Move(fi.FullName, GameFiles.RESOURCE_PATH + "output/renamed/" + card.Code + ".png");
                }
                else
                {
                    string[] fileNameWithoutB = fileName.Split('.');
                    Card card = Card.DataCard.ToList().Find(f => f.Octgn_id?.Contains(fileNameWithoutB[0]) ?? false);
                    File.Move(fi.FullName, GameFiles.RESOURCE_PATH + "output/renamed/" + card.Code + "b.png");
                }
            }
        }
    }
}