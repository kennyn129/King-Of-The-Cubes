using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HookTimer : MonoBehaviour
{
    public Sprite[] haveHook;
    public PlayerController player;
    public int playerNum;
    // Use this for initialization
    void Awake()
    {
        player = GameObject.Find("Player" + " " + playerNum).GetComponent<PlayerController>();
        gameObject.GetComponent<Image>().sprite = haveHook[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (player.hasHook)
        {
            GetComponent<Image>().sprite = haveHook[0];
        } else
        {
            GetComponent<Image>().sprite = haveHook[1];
        }
    }
}
