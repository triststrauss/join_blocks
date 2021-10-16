using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerformanceDisplay : MonoBehaviour
{
    Text performanceText;
    float deltaTime;

    // Start is called before the first frame update
    void Start()
    {
        performanceText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        performanceText.text = Mathf.Ceil(fps).ToString();
    }
}
