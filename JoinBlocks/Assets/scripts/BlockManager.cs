using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public SpaceBlock spaceBlock;
    private Vector2 startPosition;

    public float offsetBetweenBlocks;
    public float startOffsetY;
    public float distanceMultiplier;
    private float blockSize;
    private SpriteRenderer blockSpriteRenderer;


    public QueueManager queueManager;

    public SpaceBlock[] spaceBlocks;

    public bool canShoot;
    private bool isLastBlock = false;


    private void Awake()
    {
        blockSpriteRenderer = spaceBlock.GetComponent<SpriteRenderer>();



        blockSize = blockSpriteRenderer.bounds.size.x;


        float overAllWidth = (blockSize * 5) + (offsetBetweenBlocks * 4);
        float overAllHeight = (blockSize * 7) + (offsetBetweenBlocks * 6);

        transform.position = new Vector3(0, Camera.main.orthographicSize - (overAllHeight / 2f) - startOffsetY);


        startPosition.x = -overAllWidth / 2f + (blockSize / 2f);
        startPosition.y = transform.position.y + overAllHeight / 2f - blockSize / 2f;
        //startPosition.y = Camera.main.orthographicSize - (blockSize/2f) - startOffsetY;


        spaceBlocks = new SpaceBlock[35];

        SpaceBlock prvCreatedSpaceBlock = null;

        int k = 0;
        for (int i = 0; i < 5; i++)
        {
            prvCreatedSpaceBlock = null;

            for (int j = 0; j < 7; j++)
            {
                float posX = startPosition.x + (i * blockSize) + (i * offsetBetweenBlocks);
                float posY = startPosition.y - (j * blockSize) - (j * offsetBetweenBlocks);
                SpaceBlock gameObject = Instantiate(spaceBlock, new Vector3(posX, posY), Quaternion.identity);
                if (prvCreatedSpaceBlock != null)
                    prvCreatedSpaceBlock.bottomSpaceBlock = gameObject;
                prvCreatedSpaceBlock = gameObject;
                gameObject.name = i + "" + j;
                spaceBlocks[k] = gameObject;
                k++;
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        checkAndSetIfPlayerCanShootNextBlock();
        
    }

    private void checkAndSetIfPlayerCanShootNextBlock()
    {
        canShoot = true;

        foreach (SpaceBlock spaceBlock in spaceBlocks)
        {
            if (spaceBlock.getNumberBlock() != null)
            {
                if(!spaceBlock.getNumberBlock().getCurrentState().Equals(NumberBlock.STATE.IDLE))
                {
                    canShoot = false;
                    break;
                }
            }
        }
    }

    public void OnTriggerFromPlayer(GameObject gameObject)
    {
        Debug.Log("OnTriggerFromPlayer");
        SpaceBlock targetSpaceBlock = getTargetSpaceBlock(gameObject);
        if(targetSpaceBlock != null)
            queueManager.OnTrigger(getStartSpaceBlock(gameObject),targetSpaceBlock, isLastBlock);
    }

    public void d(string str)
    {
        Debug.Log("<IQ>" + str);
    }


    public SpaceBlock getTargetSpaceBlock(GameObject gameObject)
    {
        isLastBlock = false;

        string stringToCheck = gameObject.name.Substring(0, 1);

        SpaceBlock spaceBlockToReturn;
        SpaceBlock[] gameObjecColumntArray = new SpaceBlock[7];

        int k = 0;

        for (int i = 0; i < 35; i++)
        {   
            if(spaceBlocks[i].name.Substring(0,1).Equals(stringToCheck))
            {
                gameObjecColumntArray[k] = spaceBlocks[i];
                k++;
            }
        }


        spaceBlockToReturn = null;

        for (int  i = 0;  i < gameObjecColumntArray.Length;  i++)
        {
            if(spaceBlockToReturn == null)
            {
                if (gameObjecColumntArray[i].GetSTATE().Equals(SpaceBlock.STATE.AVAILABLE))
                {
                    spaceBlockToReturn = gameObjecColumntArray[i];
                }
            }
            else if(gameObjecColumntArray[i].transform.position.y > spaceBlockToReturn.transform.position.y && gameObjecColumntArray[i].GetSTATE().Equals(SpaceBlock.STATE.AVAILABLE))
            {
                spaceBlockToReturn = gameObjecColumntArray[i];
            }
        }


        if(spaceBlockToReturn == null)
        {
            spaceBlockToReturn = gameObjecColumntArray[gameObjecColumntArray.Length - 1];
            isLastBlock = true; ;
        }


        return spaceBlockToReturn;


    }


    public SpaceBlock getStartSpaceBlock(GameObject gameObject)
    {
        string stringToCheck = gameObject.name.Substring(0, 1);

        SpaceBlock gameObjectToReturn;
        SpaceBlock[] gameObjecColumntArray = new SpaceBlock[7];

        int k = 0;

        for (int i = 0; i < 35; i++)
        {
            if (spaceBlocks[i].name.Substring(0, 1).Equals(stringToCheck))
            {
                gameObjecColumntArray[k] = spaceBlocks[i];
                k++;
            }
        }
         

        gameObjectToReturn = gameObjecColumntArray[0];

        for (int i = 0; i < gameObjecColumntArray.Length; i++)
        {
            
            if (gameObjecColumntArray[i].transform.position.y < gameObjectToReturn.transform.position.y)
            {
                gameObjectToReturn = gameObjecColumntArray[i];
            }
        }


        return gameObjectToReturn;


    }

}
