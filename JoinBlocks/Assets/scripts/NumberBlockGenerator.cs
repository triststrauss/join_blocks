using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberBlockGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    public NumberBlock numberBlock;
    
  

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTrigger(GameObject startSpaceBlock, SpaceBlock targetSpaceBlock)
    {
        //Debug.Log("OnTriggerFromPlayer");
        //NumberBlock generatedNumberBlock  = Instantiate(numberBlock, startSpaceBlock.transform.position, Quaternion.identity);
        //generatedNumberBlock.shoot(startSpaceBlock.transform.position, targetSpaceBlock.transform.position,1);
        //targetSpaceBlock.setNumberBlock(generatedNumberBlock);
        
    }

   
}
