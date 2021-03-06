using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    // Start is called before the first frame update
    public BlockManager blockManager;
    public SaveGameSystem saveGameSystem;

    void Start()
    {
        d("START");
        //Application.targetFrameRate = 60;
    }


    private void Awake()
    {
        d("AWAKE");
        Application.targetFrameRate = 60;

        float orthoSize = (float)(7.2 * Screen.height / Screen.width * 0.5f);

        Camera.main.orthographicSize = orthoSize;
    }

    void Update()
    {

        if (validatePlayerTouch())
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                d(hit.collider.gameObject.name);
                blockManager.OnTriggerFromPlayer(hit.collider.gameObject);
                              
            }


            //blockManager.OnTriggerFromPlayer(blockManager.spaceBlocks[0].gameObject);


        }

        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            saveGameSystem.Save();
            Application.Quit();
        }
    }


    public static void d(string str)
    {
        Debug.Log("<IQ> : " + str);

    }

    public bool validatePlayerTouch()
    {
        return blockManager.canShoot && Input.GetMouseButtonDown(0);
        //return blockManager.canShoot && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began;
    }

}
