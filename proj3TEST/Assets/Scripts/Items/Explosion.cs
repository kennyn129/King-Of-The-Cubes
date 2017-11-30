using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public float explosionDuration, activeTimeStamp, growthRate;
    public AudioSource audioSource;
    public AudioClip explosionSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
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
        audioSource = GetComponent<AudioSource>();
        explosionDuration = 5f;
        activeTimeStamp = explosionDuration + GameManager.time;
        growthRate = .2f;

        //audioSource.Stop();
        audioSource.clip = explosionSound;
        audioSource.Play();
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
