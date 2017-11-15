using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmethystShard : MonoBehaviour {
    public GameObject amethystShardPrefab;
    public float timeToLive, activeTimeStamp;
    public int maxShards, minShards, fragmentCount;
    public Vector3 dir;
	// Use this for initialization
	void Start () {
        print("SHARD");
        minShards = 2;
        maxShards = 5;
        timeToLive = 30;
        activeTimeStamp = GameManager.time + timeToLive;
	}
	
	// Update is called once per frame
	void Update () {
        print(fragmentCount);
		if (activeTimeStamp <= GameManager.time)
        {
            print("dead");
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (fragmentCount > 0)
            {
                print("FLOOR");
                print(other.transform.name);
                int shards = Random.Range(minShards, maxShards);
                for (int i = 0; i < shards; i++)
                {
                    GameObject shard = Instantiate(amethystShardPrefab);
                    shard.transform.position = transform.position + new Vector3(0,1,0);
                    shard.transform.GetComponent<AmethystShard>().fragmentCount = fragmentCount - 1;
                    Vector3 newDir = dir;
                    shard.transform.localScale = transform.localScale / 2;
                    float x, z;
                    x = Random.Range(-1, 1);
                    z = Random.Range(-1, 1);
                    newDir += new Vector3(x, .5f, z);
                    shard.transform.GetComponent<AmethystShard>().dir = newDir;
                    shard.transform.GetComponent<Rigidbody>().AddForce(newDir*150);
                }
                //Tile t = other.transform.GetComponent<Tile>();
                //t.Break();
            }
            Destroy(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print("HIT");
            
            Vector3 newDir = new Vector3(dir.x*45, dir.y*5, dir.z*45);
            other.transform.GetComponent<Rigidbody>().AddForce(newDir.normalized * 100*(1+fragmentCount));

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print("HIT");
            
            Vector3 newDir = new Vector3(dir.x * 45, dir.y * 5, dir.z * 45);
            other.transform.GetComponent<Rigidbody>().AddForce(newDir.normalized * 200 * (1 + fragmentCount));

        }
    }
}
