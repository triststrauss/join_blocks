using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveGameSystem : MonoBehaviour
{

    public NumberBlock numberBlock;
    public BlockManager blockManager;

    void Start()
    {
       Restore();
    }
    
   public void Save()
    {
        CameraScript.d("Saving ...");
        GameData gameData = new GameData(blockManager.spaceBlocks);

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedGame2.iq";

        CameraScript.d("Path : " + path);

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    public void Restore()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedGame2.iq";
        CameraScript.d(path);


        if(File.Exists(path))
        {
            CameraScript.d("Saved Game Found");
            CameraScript.d("Restoring ...");


            FileStream stream = new FileStream(path, FileMode.Open);

            GameData gameData = formatter.Deserialize(stream) as GameData;
            stream.Close();

            Vector2 position = new Vector2();

            for (int i = 0; i < gameData.data.Length; i++)
            {
                if (gameData.data[i] == null)
                    continue;

                CameraScript.d("GameData : " + gameData.data[i]);

                string[] data = gameData.data[i].Split(new string[] { "-" }, StringSplitOptions.None);

                String numberBlockNumbe = data[0];
                SpaceBlock parentSpaceBlock = getSpaceBlock(data[1], blockManager.spaceBlocks);
                position.x = parentSpaceBlock.transform.position.x;
                position.y = parentSpaceBlock.transform.position.y;

                NumberBlock block = Instantiate(numberBlock, position, Quaternion.identity);
                block.setCurrentState(NumberBlock.STATE.IDLE);
                block.setNumber(Int16.Parse(data[0]));
                block.setParetSpaceBloc(parentSpaceBlock);
                parentSpaceBlock.setNumberBlock(block);
            }
        }
        else
        {
            CameraScript.d("No Saved Game Found");
        }




    }

    public static SpaceBlock getSpaceBlock(string spaceBlockName, SpaceBlock[] spaceBlocks)
    {
        for(int i = 0; i < spaceBlocks.Length; i++)
        {
            if (spaceBlocks[i].name.Equals(spaceBlockName))
                return spaceBlocks[i];
        }

        return null;
    }


    void OnApplicationPause(bool pause)
    {
        CameraScript.d("On Pause" + pause);
        if (pause)
            Save();
    }
}
