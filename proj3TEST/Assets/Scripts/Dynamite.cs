using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : Item {
    //public float timeToLive, activeTimeStamp;
    //public int uses, team;
    //public bool activate;
    public float timeBeforeExplosion;
    public GameObject explosionPrefab;


    // Use this for initialization
    protected override void Start()
    {
        activate = false;
        uses = 1;
        timeBeforeExplosion = 10f;
        activate = false;
        taken = false;
        timeToLive = 30;
        //uses = 2;
        //timeToLive = 10;
        activeTimeStamp = GameManager.time + timeToLive;
        //timeToLive = 20;
        //explosionDuration = 5;
        //activeTimeStamp = 0;// GameManager.time + timeToLive;
    }

    // Update is called once per frame
    protected override void Update () {
        if (activeTimeStamp <= GameManager.time && !taken)
        {
            Destroy(gameObject);
        }
        if (activate)
        {
            if (activeTimeStamp < GameManager.time)
            {
                //transform.GetComponent<SphereCollider>().enabled = true;
                
                //if (activeTimeStamp + explosionDuration < GameManager.time)
                //{
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = transform.position;
                Destroy(gameObject);
                //}
            }
        }
	}

    public override int Use(PlayerController p)
    {
        print("USING dynamite");

        transform.GetComponent<MeshRenderer>().enabled = true;
        transform.position = p.transform.position;
        activeTimeStamp = GameManager.time + timeBeforeExplosion;
        return UpdateUse(p);
    }
}
