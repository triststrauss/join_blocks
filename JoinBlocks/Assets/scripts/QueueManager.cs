using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{

    public NumberBlock numberBlock;

    private Vector2 firstPosition;
    private Vector2 secondPosition;

    private NumberBlock firstInQueueNumberBlock;
    private NumberBlock secondInQueueNumberBlock;

    public float offsetBetweenTwoNumberBlocks;


    void Awake()
    {
        generateNumberBlock();
        generateNumberBlock();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    internal void OnTrigger(SpaceBlock startSpaceBlock, SpaceBlock targetSpaceBlock, bool isLastBlock)
    {
        if(isLastBlock && targetSpaceBlock.getNumberBlock().number == firstInQueueNumberBlock.number)
        {
            firstInQueueNumberBlock.directMerge(targetSpaceBlock,targetSpaceBlock.getNumberBlock());
            targetSpaceBlock.setNumberBlock(firstInQueueNumberBlock);
            firstInQueueNumberBlock.parentSpaceBlock = targetSpaceBlock;
            firstInQueueNumberBlock = secondInQueueNumberBlock;
            firstInQueueNumberBlock.setCurrentState(NumberBlock.STATE.QUEUE_MOVEMENT);
            generateNumberBlock();
        }
        else if(!isLastBlock)
        {
            firstInQueueNumberBlock.shoot(startSpaceBlock.transform.position, targetSpaceBlock);
            targetSpaceBlock.setNumberBlock(firstInQueueNumberBlock);
            firstInQueueNumberBlock.parentSpaceBlock = targetSpaceBlock;
            firstInQueueNumberBlock = secondInQueueNumberBlock;
            firstInQueueNumberBlock.setCurrentState(NumberBlock.STATE.QUEUE_MOVEMENT);
            generateNumberBlock();
        }
        
        

        //secondInQueueNumberBlock = firstInQueueNumberBlock;
        //firstInQueueNumberBlock.setCurrentState(NumberBlock.STATE.QUEUE_TRANSITION);
        //secondInQueueNumberBlock = Instantiate(numberBlock, secondPosition, Quaternion.identity);
    }



    public void generateNumberBlock()
    {
        secondPosition.y = transform.position.y;
        secondPosition.x = transform.position.x + offsetBetweenTwoNumberBlocks;
        secondInQueueNumberBlock = Instantiate(numberBlock, secondPosition, Quaternion.identity);
        secondInQueueNumberBlock.setCurrentState(NumberBlock.STATE.QUEUE_TWO);
        secondInQueueNumberBlock.setNumber();
        checkAndMoveToFirstPositionInQueue(secondInQueueNumberBlock);

    }


    public void checkAndMoveToFirstPositionInQueue(NumberBlock block)
    {
        NumberBlock[] numberBlocks = GameObject.FindObjectsOfType<NumberBlock>();

        bool isFirstPlaceEmpty = true;

        foreach (NumberBlock number in numberBlocks)
        {
            if (number.getCurrentState().Equals(NumberBlock.STATE.QUEUE_ONE) || number.getCurrentState().Equals(NumberBlock.STATE.QUEUE_MOVEMENT))
                isFirstPlaceEmpty = false;
        }

        if (isFirstPlaceEmpty)
        {
            block.setCurrentState(NumberBlock.STATE.QUEUE_MOVEMENT);
            firstInQueueNumberBlock = block;
        }
    }


    public static void RestoreGame()
    {
       
    }
}
