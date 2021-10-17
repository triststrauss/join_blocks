using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string[] data;

    public GameData(SpaceBlock[] spaceBlocks)
    {
        data = new string[spaceBlocks.Length];
        
        for (int i = 0; i < spaceBlocks.Length; i++)
        {

            if(spaceBlocks[i].getNumberBlock() != null)
            {
                string parentSpaceBlock = spaceBlocks[i].name;
                string numberBlock = spaceBlocks[i].getNumberBlock().number.ToString();

                data[i] = numberBlock + "-" + parentSpaceBlock;
            }
         
        }
    }

}
