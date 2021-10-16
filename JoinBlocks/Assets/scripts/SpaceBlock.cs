using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBlock : MonoBehaviour
{
    private int id;
    public enum STATE {
        AVAILABLE,
        TAKEN
        }

    private STATE currentState;

    private NumberBlock numberBlock, approachingNumberBlock;

    public int columnNumber;

    public SpaceBlock bottomSpaceBlock;


    // Start is called before the first frame update
    void Start()
    {
        columnNumber = Int16.Parse(name.Substring(0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        if(numberBlock == null)
        {
            currentState = STATE.AVAILABLE;
        }
        else
        {
            currentState = STATE.TAKEN;
        }
    }

    public void setId(int id)
    {
        this.id = id;
    }

   
    public STATE GetSTATE()
    {
        return currentState;
    }


    public void setNumberBlock(NumberBlock numberBlock)
    {
        if (numberBlock == null)
            currentState = STATE.AVAILABLE;
        this.numberBlock = numberBlock;
    }

    public NumberBlock getNumberBlock()
    {
        return this.numberBlock;
    }

    internal void onNumberBlockMovedToAnotherSpaceBlock()
    {
        setNumberBlock(null);
        if(bottomSpaceBlock != null)
            bottomSpaceBlock.topSpaceBlockAvailable(this);
    }

    private void topSpaceBlockAvailable(SpaceBlock spaceBlock)
    {
        if(numberBlock != null)
            numberBlock.moveToAvailableSpace(spaceBlock);
    }


    public NumberBlock getApproachingNumberBlock()
    {
        return approachingNumberBlock;
    }


    public void setApproachingNumberBlock(NumberBlock numberBlock)
    {
        this.approachingNumberBlock = numberBlock;
    }
}
