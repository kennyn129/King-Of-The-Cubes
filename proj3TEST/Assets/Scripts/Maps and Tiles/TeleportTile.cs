using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTile : Tile {
    public GameObject receiver;
    public List<GameObject> senders;

    protected new void Start()
    {
        base.Start();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") 
            && active && receiver && receiver.transform.GetComponent<TeleportTile>().active)
        {
            collision.transform.position = receiver.transform.position + new Vector3(0, 1f, 0);
        }
    }

    public override void Reset()
    {
        base.Reset();
    }

    public override void Break()
    {
        base.Break();

        if (receiver)
        {
            TeleportTile r = receiver.transform.GetComponent<TeleportTile>();
            if (r.active && r.senders.Count == 1)
                r.Break();
        }
        foreach (GameObject sender in senders)
        {
            TeleportTile s = sender.transform.GetComponent<TeleportTile>();
            if (s.active)
                s.Break();
        }

        currentColor = Color.black;
        transform.GetComponent<Renderer>().material.color = currentColor;
    }

    // Update is called once per frame
    protected new void Update()
    {
        active = active = respawnTimeStamp <= GameManager.time; // && suddenDeathTimeStamp >= GameManager.time;
        if(!active)
            transform.GetComponent<Renderer>().material.color = Color.black;
        else
        {
            ChangeColor();
        }

    }
}
