using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {
    public float timeToLive, activeTimeStamp;
    public int uses, team;
    public bool activate, taken;

    // Use this for initialization
    protected abstract void Start();

    // Update is called once per frame
    protected abstract void Update();

    

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController p = other.transform.GetComponent<PlayerController>();
            Interaction(p);
        }
    }

    public void Interaction(PlayerController p)
    {
        if (!taken)
        {
            taken = true;
            if (p.items.Count == 0)
            {
                p.items.Add(this);
                transform.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                Destroy(gameObject);
            }
            //Destroy(gameObject);
        }
    }

    public int UpdateUse(PlayerController p)
    {
        print("USING item");
        uses--;
        activate = true;
        activeTimeStamp = GameManager.time + timeToLive;

        if (uses == 0)
        {
            p.items.Remove(this);
        }
        //Destroy(gameObject);
        return uses;
    }

    public abstract int Use(PlayerController p);
    
}
