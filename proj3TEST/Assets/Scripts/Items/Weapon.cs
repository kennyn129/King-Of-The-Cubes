using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public bool hitSomething;
    public float cooldownTimeStamp, cooldownTime, activeTimeStamp, activeTime;
    MeshRenderer mr;
    BoxCollider bc;

    private void OnTriggerEnter(Collider other)
    {
        print(1);
        if(other.transform.tag == "HitBox")
        {
            print(other.transform.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("ENTER");
        if (collision.transform.tag == "Tile" && !hitSomething)
        {
            Tile t = collision.transform.GetComponent<Tile>();
            //if (!t.isHole)
            //{
                //print("ok");
                //sr.enabled = activeTimeStamp >= GameManager.time;

                //bc.enabled = activeTimeStamp >= GameManager.time;
                // hitSomething = true;
            t.Break();
            //}
        }
        //onLand++;
    }
    private void OnCollisionStay(Collision collision)
    {
        //print("STAY");
        if (collision.transform.tag == "Tile" && !hitSomething)
        {
            Tile t = collision.transform.GetComponent<Tile>();
            //if (!t.isHole)
            //{
                //print("ok");
                //sr.enabled = activeTimeStamp >= GameManager.time;

                //bc.enabled = activeTimeStamp >= GameManager.time;
                // hitSomething = true;
                t.Break();
            //}
        }
        //onLand++;
    }



    public void Action()
    {
        cooldownTimeStamp = GameManager.time + cooldownTime;
        activeTimeStamp = GameManager.time + activeTime;
        hitSomething = false;

    }

    public bool CanUse()
    {
        return cooldownTimeStamp <= GameManager.time;
    }

    // Use this for initialization
    void Start () {
        cooldownTime = 2.0f;
        cooldownTimeStamp = 0;
        activeTime = 1.0f;
        activeTimeStamp = 0;
        mr = transform.GetComponent<MeshRenderer>();
        bc = transform.GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        mr.enabled = activeTimeStamp >= GameManager.time;
        bc.enabled = activeTimeStamp >= GameManager.time;
	}
}
