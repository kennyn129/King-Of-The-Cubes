//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adhesive : MonoBehaviour {
    public Tile tile;
    public float timeToLive, activeTimeStamp, maxSize, destroyTimeStamp;
    public float stickyStrength;
    bool destroying;
    // Use this for initialization
    void Start() {
        timeToLive = 100f;
        activeTimeStamp = GameManager.time + timeToLive;
        maxSize = 7;
        stickyStrength = 1;
        destroying = false;
    }

    // Update is called once per frame
    void Update() {
        if (transform.localScale.x < maxSize && transform.localScale.z < maxSize)
        {
            float x, y;
            x = Random.Range(0, .01f);
            y = Random.Range(0, .01f);
            transform.localScale = new Vector3(transform.localScale.x * (1 + x), transform.localScale.y, transform.localScale.z * (1 + y));
        }
        stickyStrength = (activeTimeStamp - GameManager.time) / timeToLive;
        Color c = transform.GetComponent<Renderer>().material.color;
        transform.GetComponent<Renderer>().material.color = new Color(c.r, c.g, c.b, stickyStrength * .5f);
        if (activeTimeStamp <= GameManager.time || !tile.active && !destroying)
        {
            destroying = true;
            destroyTimeStamp = GameManager.time + 10f;
            transform.GetComponent<MeshRenderer>().enabled = false;
            // Destroy(gameObject);
        }
        if (destroying && destroyTimeStamp <= GameManager.time)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            float stickiness = 1 - stickyStrength;
            if (stickyStrength > .5f)
                stickiness = .5f;
            if (stickyStrength < .01f || destroying)
                stickiness = 1;
            
            other.transform.GetComponent<PlayerController>().SetMoveDebuff(stickiness);
           // other.transform.GetComponent<Rigidbody>().AddForce(new Vector3(0,10,0));

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.GetComponent<PlayerController>().SetMoveDebuff(1);
        }
    }
}
