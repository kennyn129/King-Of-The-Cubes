using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Text textCountdown;
    public float timeRemaining = 5;
    // Use this for initialization
    void Start()
    {
        textCountdown = GetComponent<Text>() as Text;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            textCountdown.text = "Time Remaining : " + timeRemaining.ToString("f0");
        }
        else
        {
            textCountdown.text = "Sudden Death";
        }
    }
}
