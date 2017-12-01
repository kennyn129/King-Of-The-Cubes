using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTag : PlayerController {
    public Sprite[] idsprites;
	// Use this for initialization
    void Awake()
    {

    }
	void Start () {
		if (playerNum == 1)
        {
            GetComponent<SpriteRenderer>().sprite = idsprites[0];
        }else if (playerNum == 2)
        {
            GetComponent<SpriteRenderer>().sprite = idsprites[1];
        }
        else if (playerNum == 3)
        {
            GetComponent<SpriteRenderer>().sprite = idsprites[2];
        }
        else if (playerNum == 4)
        {
            GetComponent<SpriteRenderer>().sprite = idsprites[3];
        }
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (Camera.main.transform.position, Vector3.up);
	}
}
