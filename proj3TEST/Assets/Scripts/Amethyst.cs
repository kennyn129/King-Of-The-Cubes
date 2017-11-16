using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amethyst : Item {
    public GameObject amethystShardPrefab;

    public override int Use(PlayerController p)
    {
        //transform.GetComponent<MeshRenderer>().enabled = true;
        //transform.position = p.transform.position;
        GameObject newShard = Instantiate(amethystShardPrefab);
        newShard.transform.position = p.transform.position + new Vector3(0,2,0);
        Vector3 dir = p.GetDirection();
        dir = new Vector3(dir.x*2, 1, dir.z*2);
        newShard.transform.GetComponent<AmethystShard>().dir = dir;
        newShard.transform.GetComponent<AmethystShard>().team = p.team;
        newShard.transform.GetComponent<AmethystShard>().fragmentCount = 1;


        newShard.transform.GetComponent<Rigidbody>().AddForce(dir*200);
        newShard.transform.SetParent(GameManager.gameManager.inGameParticlesAndEffects.transform);
        //activeTimeStamp = GameManager.time + timeToLive;
        return UpdateUse(p);
    }

    protected override void Start()
    {
        activate = false;
        uses = 3;
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

    protected override void Update()
    {
        if (activeTimeStamp <= GameManager.time && !taken)
        {
            Destroy(gameObject);
        }
        //if (activate)
        //{
        //    if (activeTimeStamp < GameManager.time)
        //    {
                //transform.GetComponent<SphereCollider>().enabled = true;

                //if (activeTimeStamp + explosionDuration < GameManager.time)
                //{
                //GameObject explosion = Instantiate(explosionPrefab);
                //explosion.transform.position = transform.position;
        //        Destroy(gameObject);
                //}
        //    }
        //}
    }
    /*
    protected new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
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
    }*/
}
