using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberBlock : MonoBehaviour
{
    public enum STATE
    {
        QUEUE_ONE,
        QUEUE_MOVEMENT,
        QUEUE_TWO,
        START_SHOOT,
        SHOOT_MOVEMENT,
        HOLD_AFTER_SHOOT_COMPLETE,
        MERGE_MOVEMENT,
        MOVE_TO_AVAILABLE_SPACE,
        IDLE,
    }

    private STATE currentState;

    public int[] numbers = new int[] { 2, 4, 8, 16, 32, 64 };

    public Vector2 targetPosition;
    public Vector2 startPosition;
    public float speed;
    public int number;
    private Text debugText;
    private Text numberText;
    public float TRANSITION_STEP;
    public float QUEUE_TRANSITION_STEP;

    private Vector2 queueFirstPosition;

    public SpaceBlock parentSpaceBlock;
   

    private NumberBlock mergerNumberBlock;

    private SpriteRenderer sprite;


    public ScoreManager scoreManager;

    public SpaceBlock targetSpaceBlock;



    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        queueFirstPosition = new Vector2(1, -4.76f);
        Text[] text = GetComponentsInChildren<Text>();
        foreach (Text t in text)
        {
            if (t.name.Equals("NumberText"))
            {
                numberText = t;
            }
            else if (t.name.Equals("DebugText"))
            {
                debugText = t;
            }
        }

        scoreManager = GameObject.FindObjectOfType<ScoreManager>();

       
    }

    // Start is called before the first frame updat
    void Start()
    {
        numberText.text = "" + number;
        setColor();
    }

    public void moveToAvailableSpace(SpaceBlock spaceBlock)
    {
        parentSpaceBlock.onNumberBlockMovedToAnotherSpaceBlock();
        parentSpaceBlock = spaceBlock;
        spaceBlock.setNumberBlock(this);
        setCurrentState(STATE.MOVE_TO_AVAILABLE_SPACE);
    }


    public void setNumber(int number)
    {
        this.number = number;
        
    }

    public void setNumber()
    {
        this.number = numbers[UnityEngine.Random.Range(0, numbers.Length)];
        setNumber(number);
    }

    public void setColor()
    {

        string colorString = "";

        switch(number)
        {
            case 0:
                colorString = "#ef5350";
                break;
            case 2:
                colorString = "#ec407a";
                break;
            case 4:
                colorString = "#ab47bc";
                break;
            case 8:
                colorString = "#673ab7";
                break;
            case 16:
                colorString = "#3f51b5";
                break;
            case 32:
                colorString = "#2196f3";
                break;
            case 64:
                colorString = "#03a9f4";
                break;
            case 128:
                colorString = "#00bcd4";
                break;
            case 256:
                colorString = "#009688";
                break;
            case 512:
                colorString = "#4caf50";
                break;
            case 1024:
                colorString = "#8bc34a";
                break;
            case 2048:
                colorString = "#cddc39";
                break;
            case 4096:
                colorString = "#ffeb3b";
                break;
        }

        Color color;
        ColorUtility.TryParseHtmlString(colorString, out color);
        sprite.color = color;
    }

    public void moveToAvailableSpace()
    {
        if (canNotMove)
            return;

        transform.position = Vector2.MoveTowards(transform.position, parentSpaceBlock.transform.position, TRANSITION_STEP * Time.deltaTime);
        if (Vector2.Distance(transform.position, parentSpaceBlock.transform.position) <= 0.1f)
        {
            transform.position = parentSpaceBlock.transform.position;
            StartCoroutine(HoldAfterShootComplete());
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {

            case STATE.QUEUE_TWO:

                break;
            case STATE.QUEUE_MOVEMENT:
                transform.position = Vector2.MoveTowards(transform.position, queueFirstPosition, QUEUE_TRANSITION_STEP * Time.deltaTime);
                if (Vector2.Distance(transform.position, queueFirstPosition) <= 0.01f)
                {
                    transform.position = queueFirstPosition;
                    setCurrentState(STATE.QUEUE_ONE);
                }
                break;
            case STATE.QUEUE_ONE:
                break;
            case STATE.START_SHOOT:
                onShootCalled();
                break;
            case STATE.SHOOT_MOVEMENT:
                moveToTargetPositionOnShoot();
                break;
            case STATE.HOLD_AFTER_SHOOT_COMPLETE:
                //StartCoroutine("HoldAfterShootComplete");
                break;
            case STATE.MERGE_MOVEMENT:
                mergerMovement();
                break;
            case STATE.MOVE_TO_AVAILABLE_SPACE:
                moveToAvailableSpace();
                break;
            case STATE.IDLE:
                break;
        }

        
    }

    private void setMergeEvent(NumberBlock numberBlock)
    {
        setCurrentState(STATE.MERGE_MOVEMENT);
        mergerNumberBlock = numberBlock;
    }



    public void shoot(Vector2 startPosition, SpaceBlock targetSpaceBlock)
    {
        this.targetSpaceBlock = targetSpaceBlock;
        this.targetPosition = targetSpaceBlock.transform.position;
        this.startPosition = startPosition;
        setCurrentState(STATE.START_SHOOT);
    }

    public void directMerge(SpaceBlock targetSpaceBlock, NumberBlock numberBlock)
    {
        setCurrentState(STATE.IDLE);
        Destroy(numberBlock.gameObject);
        onMergeBlockDestroy();
        transform.position = targetSpaceBlock.transform.position;
    }

    public STATE getCurrentState()
    {
        return currentState;
    }

    public void setCurrentState(STATE stateToChange)
    {
        currentState = stateToChange;
        if(debugText != null)
            debugText.text = currentState.ToString();
    }


    public void checkAndMoveToFirstPositionInQueue()
    {
        NumberBlock[] numberBlocks = GameObject.FindObjectsOfType<NumberBlock>();

        bool isFirstPlaceEmpty = true;

        foreach (NumberBlock number in numberBlocks)
        {
            if (number.getCurrentState().Equals(STATE.QUEUE_ONE) || number.getCurrentState().Equals(STATE.QUEUE_MOVEMENT))
                isFirstPlaceEmpty = false;
        }

        if (isFirstPlaceEmpty)
        {
            setCurrentState(STATE.QUEUE_MOVEMENT);

        }
    }

    
    public void setParetSpaceBloc(SpaceBlock spaceBlock)
    {
        this.parentSpaceBlock = spaceBlock;
    }



    public void onShootCalled()
    {
        transform.position = startPosition;
        setCurrentState(STATE.SHOOT_MOVEMENT);
    }

    public void moveToTargetPositionOnShoot()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, TRANSITION_STEP * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPosition) <= 0.1f)
        {
            transform.position = targetPosition;
            StartCoroutine(HoldAfterShootComplete());
            
        }
    }

    IEnumerator HoldAfterShootComplete()
    {
        setCurrentState(STATE.HOLD_AFTER_SHOOT_COMPLETE);
        yield return new WaitForSeconds(0.2f);

        if(currentState.Equals(STATE.HOLD_AFTER_SHOOT_COMPLETE))
            checkAndMergeWithNeighbouringBlocks();

        //yield return null;
    }
    int noOfBlocksMerged = -1;
    private bool canNotMove;

    public void checkAndMergeWithNeighbouringBlocks()
    {
        

    
            SpaceBlock spaceBlockOnTop = getSpaceBlockOnTop();

            if (spaceBlockOnTop != null)
            {
                if (spaceBlockOnTop.getNumberBlock() != null && spaceBlockOnTop.getNumberBlock().number.Equals(number))
                {

                }
                else
                {
                    spaceBlockOnTop = null;
                }
            }

            SpaceBlock spaceBlockOnLeft = getSpaceBlockOnLeft();
            if (spaceBlockOnLeft != null)
            {
                if (spaceBlockOnLeft.getNumberBlock() != null && spaceBlockOnLeft.getNumberBlock().number.Equals(number))
                {

                }
                else
                {
                    spaceBlockOnLeft = null;
                }
            }

            SpaceBlock spaceBlockOnRight = getSpaceBlockOnRight();
            if (spaceBlockOnRight != null)
            {
                if (spaceBlockOnRight.getNumberBlock() != null && spaceBlockOnRight.getNumberBlock().number.Equals(number))
                {

                }
                else
                {
                    spaceBlockOnRight = null;
                }
            }

            if (spaceBlockOnTop != null)
            {
                spaceBlockOnTop.getNumberBlock().setMergeEvent(this);
                noOfBlocksMerged++;
            }
            if (spaceBlockOnLeft != null)
            {
                spaceBlockOnLeft.getNumberBlock().setMergeEvent(this);
                noOfBlocksMerged++;
            }
            if (spaceBlockOnRight != null)
            {
                spaceBlockOnRight.getNumberBlock().setMergeEvent(this);
                noOfBlocksMerged++;
            }


            if(spaceBlockOnTop == null && spaceBlockOnLeft == null && spaceBlockOnRight == null)
        {
            noOfBlocksMerged = 0;
        }    

       
        setCurrentState(STATE.IDLE);

        if (noOfBlocksMerged > 0)
            canNotMove = true;

        /*if (noOfBlocksMerged == 1)
        {
            number *= 2;
        }
        if (noOfBlocksMerged == 2)
        {
            number *= 4;
        }
        if (noOfBlocksMerged == 3)
        {
            number *= 8;
        }

        if (noOfBlocksMerged != 0)
        {
            scoreManager.incrementScore(number);
        }

        setColor();

        numberText.text = "" + number;
        debugText.text = currentState.ToString();*/






    }


    private SpaceBlock getSpaceBlockOnTop()
    {
        string stringToCheck = parentSpaceBlock.name.Substring(1, 1);
        int yToCheck = Int16.Parse(stringToCheck) - 1;

        string spaceBlockToCheckName = parentSpaceBlock.name.Substring(0, 1) + yToCheck;

        SpaceBlock[] spaceBlocks = GameObject.FindObjectsOfType<SpaceBlock>();

        SpaceBlock spaceBlockToCheck = null;

        foreach (SpaceBlock spaceBlock in spaceBlocks)
        {
            if (spaceBlock.name.Equals(spaceBlockToCheckName))
            {
                spaceBlockToCheck = spaceBlock;
                break;
            }
        }

        return spaceBlockToCheck;
    }

    private SpaceBlock getSpaceBlockOnLeft()
    {
        string stringToCheck = parentSpaceBlock.name.Substring(0, 1);
        int x = Int16.Parse(stringToCheck) - 1;

        string spaceBlockToCheckName = x + parentSpaceBlock.name.Substring(1, 1);

        SpaceBlock[] spaceBlocks = GameObject.FindObjectsOfType<SpaceBlock>();

        SpaceBlock spaceBlockToCheck = null;

        foreach (SpaceBlock spaceBlock in spaceBlocks)
        {
            if (spaceBlock.name.Equals(spaceBlockToCheckName))
            {
                spaceBlockToCheck = spaceBlock;
                break;
            }
        }

        return spaceBlockToCheck;
    }

    private SpaceBlock getSpaceBlockOnRight()
    {
        string stringToCheck = parentSpaceBlock.name.Substring(0, 1);
        int x = Int16.Parse(stringToCheck) + 1;

        string spaceBlockToCheckName = x + parentSpaceBlock.name.Substring(1, 1);

        SpaceBlock[] spaceBlocks = GameObject.FindObjectsOfType<SpaceBlock>();

        SpaceBlock spaceBlockToCheck = null;

        foreach (SpaceBlock spaceBlock in spaceBlocks)
        {
            if (spaceBlock.name.Equals(spaceBlockToCheckName))
            {
                spaceBlockToCheck = spaceBlock;
                break;
            }
        }

        return spaceBlockToCheck;
    }


    public void mergerMovement()
    {
        Vector2 mergePosition = mergerNumberBlock.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, mergePosition, QUEUE_TRANSITION_STEP * Time.deltaTime);
        if (Vector2.Distance(transform.position, mergePosition) <= 0.01f)
        {
            transform.position = mergePosition;
            onMergeComplete();
            mergerNumberBlock.onMergeBlockDestroy();
        }
    }

    private void onMergeBlockDestroy()
    {
        canNotMove = false;
        number *= 2;

        scoreManager.incrementScore(number);
        setColor();

        numberText.text = "" + number;
        debugText.text = currentState.ToString();

        if (currentState != STATE.MOVE_TO_AVAILABLE_SPACE && currentState != STATE.HOLD_AFTER_SHOOT_COMPLETE)
            StartCoroutine(HoldAfterShootComplete());
    }

    public void onMergeComplete()
    {
        parentSpaceBlock.onNumberBlockMovedToAnotherSpaceBlock();
        Destroy(gameObject);
    }

}
