using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : PlayerController {
    public Sprite[] itemSprites;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().sprite = items[0].transform.GetComponent<SpriteRenderer>().sprite;
    }
}
