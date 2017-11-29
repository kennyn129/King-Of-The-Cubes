﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmethystShard : MonoBehaviour
{
    public GameObject amethystShardPrefab;
    public float timeToLive, activeTimeStamp, gravity;
    public int maxShards, minShards, fragmentCount, team;
    public Vector3 dir;
    float spread;
    // Use this for initialization
    void Start()
    {
        gravity = 20;
        minShards = 2;
        maxShards = 5;
        timeToLive = 30;
        activeTimeStamp = GameManager.time + timeToLive;
        spread = 45;
    }

    private void FixedUpdate()
    {
        transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTimeStamp <= GameManager.time)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (fragmentCount > 0)
            {
                //float[] angle = new float[] { 30, 40, 50, 60,70 };
                int shards = Random.Range(minShards, maxShards);
                for (int i = 0; i < shards; i++)
                {
                    GameObject shard = Instantiate(amethystShardPrefab);
                    //print(Quaternion.Euler(transform.eulerAngles.x , 0, transform.eulerAngles.z));
                    //Quaternion angleAdjustment = Quaternion.FromToRotation()
                    shard.transform.position = transform.position + new Vector3(0, 1.5f, 0);
                    shard.transform.GetComponent<AmethystShard>().fragmentCount = fragmentCount - 1;
                    Vector3 newDir = dir;
                    shard.transform.localScale = transform.localScale / 2;

                    //float x, z;
                    //x = //Random.Range(-1, 1);
                    //z = //Random.Range(-1, 1);
                    //newDir += new Vector3(x, .5f, z);
                    float angle = i * 90 / (shards - 1) - spread + Vector3.Angle(transform.position, newDir);
                    newDir = new Vector3(Mathf.Cos(angle) * newDir.x, newDir.y, newDir.z * Mathf.Sin(angle));
                    shard.transform.GetComponent<AmethystShard>().dir = newDir;
                    shard.transform.GetComponent<AmethystShard>().team = team;
                    shard.transform.GetComponent<Rigidbody>().AddForce(200 * newDir);
                    shard.transform.SetParent(GameManager.gameManager.inGameParticlesAndEffects.transform);
                }
                //Tile t = other.transform.GetComponent<Tile>();
                //t.Break();
            }
            Destroy(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player") &&
            other.transform.GetComponent<PlayerController>().team != team)
        {

            Vector3 newDir = new Vector3(dir.x * 50, 10, dir.z * 50);
            other.transform.GetComponent<Rigidbody>().AddForce(newDir.normalized * 500 * (1 + fragmentCount));

            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") &&
            other.transform.GetComponent<PlayerController>().team != team)
        {

            //Vector3 newDir = new Vector3(dir.x * 45, dir.y * 5, dir.z * 45);
            //other.transform.GetComponent<Rigidbody>().AddForce(newDir.normalized * 200 * (1 + fragmentCount));

        }
    }
}