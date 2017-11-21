using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour {
    public float timeToLive, activeTimeStamp;
    //GameObject itemPrefab;
    public Item item;
    bool taken;
    // Use this for initialization



    void Start()
    {
        timeToLive = 200;
        //item = null;

        //int item = Random.Range(0, GameManager.gameManager.itemPrefabs.Length);
       
        //items = new List<Item>();
        //itemPrefab = GameManager.gameManager.itemPrefabs[item];
        //this.item = Instantiate(GameManager.gameManager.items[item]);
        //item = Instantiate(GameManager.gameManager.itemPrefabs[Random.Range(0,GameManager.gameManager.itemPrefabs.Length)]);
        activeTimeStamp = GameManager.time + timeToLive;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTimeStamp <= GameManager.time)
        {
            Destroy(gameObject);
            Destroy(item.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //print("COO");
            PlayerController p = other.transform.GetComponent<PlayerController>();
            Interaction(p);
        }
    }

    public void Interaction(PlayerController p)
    {
        if (!taken)
        {
            print("here");
            taken = true;
            if (p.items.Count == 0)
            {
                print("OK");
                Item copy = Instantiate(item);
                p.items.Add(copy);
            }
            else
            {
                Destroy(item.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
