using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Percentage : PlayerController {
    public Text textpercent;
    public float percent;
	// Use this for initialization
    void Awake()
    {
        textpercent = GetComponent<Text>() as Text;
    }
	void Start () {
        GameObject player = GameObject.FindGameObjectWithTag("Player" + playerNum.ToString());
        PlayerController playerscript = player.GetComponent<PlayerController>();
        textpercent.text = playerscript.healthScalar.ToString("f0") + "%";
	}
	
	// Update is called once per frame
	void Update () {
        GameObject player = GameObject.FindGameObjectWithTag("Player" + playerNum.ToString());
        PlayerController playerscript = player.GetComponent<PlayerController>();
        percent = playerscript.healthScalar - 200;
        textpercent.text = percent.ToString("f0") + "%";
	}
}
