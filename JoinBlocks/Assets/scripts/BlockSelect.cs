using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelect : MonoBehaviour
{
    public NumberBlock numberBlock;

    private NumberBlock[] numberBlocks;
    public float startPositionX;
    public float offset;
    public float scale;
    SpriteRenderer numberBlockRenderer;

    // Start is called before the first frame update
    void Start()
    {
        numberBlocks = new NumberBlock[6];

        numberBlockRenderer = numberBlock.GetComponent<SpriteRenderer>();

        float blockSize = numberBlockRenderer.bounds.size.x * scale;
        

        for (int i = 0; i < numberBlocks.Length; i++)
        {
            float x = startPositionX + (blockSize * i) + (offset * i);
            float y = transform.position.y;
            NumberBlock go = Instantiate(numberBlock, new Vector3(x, y), Quaternion.identity);
            go.number = (int)Mathf.Pow(2, (i + 1));
            go.transform.localScale = new Vector3(scale, scale, 1);
            numberBlocks[i] = go;
           
        }


       
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
