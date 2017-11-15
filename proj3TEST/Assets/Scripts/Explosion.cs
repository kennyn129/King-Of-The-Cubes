using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public float explosionDuration, activeTimeStamp, growthRate;

    private void OnTriggerEnter(Collider other)
    {
        //print(1);
        //if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        //{
        //    print("HIT");
        //    print(other.transform.name);
        //}
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            print("FLOOR");
            print(other.transform.name);
            Tile t = other.transform.GetComponent<Tile>();
            t.Break();

        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector3 dir = other.transform.position - transform.position;
            dir = new Vector3(dir.x, dir.y + 5, dir.z);
            other.transform.GetComponent<Rigidbody>().AddForce(dir.normalized * 50);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector3 dir = other.transform.position - transform.position;
            dir = new Vector3(dir.x, dir.y + 5, dir.z);
            other.transform.GetComponent<Rigidbody>().AddForce(dir.normalized * 100);

        }
    }

    // Use this for initialization
    void Start () {
        explosionDuration = 5f;
        activeTimeStamp = explosionDuration + GameManager.time;
        growthRate = .2f;
    }
	
	// Update is called once per frame
	void Update () {
		if(activeTimeStamp >= GameManager.time)
        {
            transform.localScale += new Vector3(growthRate,growthRate,growthRate);
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
