using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : Tile {
    public List<GameObject> players;
    public List<Vector3> destinations;
    //public int alt = 1;
    //public float nextDirTimeStamp, nextDirTime;
    public Vector3 direction, originalPosition;
    public float moveSpeed;
    public int currentDest;

    protected new void Start()
    {
        base.Start();
        currentDest = 0;
        moveSpeed = .1f;
        //destinations = new List<Vector3>();
        //nextDirTime = 10f;
        currentDest = 0;
        //nextDirTimeStamp = nextDirTime + GameManager.time;
        direction = Vector3.zero;
        players = new List<GameObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            //     && active && receiver && receiver.transform.GetComponent<TeleportTile>().active)
        {
            print("Touch player");
            players.Add(collision.gameObject);
            //collision.transform.SetParent(transform);
            //collision.transform.position = collision.transform.position + new Vector3(.1f*alt, 0, 0);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        //     && active && receiver && receiver.transform.GetComponent<TeleportTile>().active)
        {
            //collision.transform.position += new Vector3(.1f, 0, 0) * alt;
            //print("Touch player");
            //collision.transform.position = collision.transform.position + new Vector3(.1f*alt, 0, 0);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        //     && active && receiver && receiver.transform.GetComponent<TeleportTile>().active)
        {
            print("Touch BYE player");
            players.Remove(collision.gameObject);
            //collision.transform.SetParent(null);
            //collision.transform.position = receiver.transform.position + new Vector3(0, .2f, 0);
        }
    }

    public override void Reset()
    {
        base.Reset();
        transform.position = originalPosition;
        currentDest = 0;
        //print("?");
        //transform.position = new Vector3(220, -1, 10);
        //nextDirTimeStamp = nextDirTime + GameManager.time;
        //alt = 1;
    }

    public override void Break()
    {
        print("CAnt break");
        //base.Break();

        //nextDirTimeStamp = nextDirTime + GameManager.time;
        //alt = 1;
    }

    // Update is called once per frame
    protected new void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Reset();
        active = active = respawnTimeStamp <= GameManager.time; // && suddenDeathTimeStamp >= GameManager.time;
        if (!active)
            transform.GetComponent<Renderer>().material.color = Color.black;
        else
        {
            ChangeColor();
        }
        // if(nextDirTimeStamp <= GameManager.time)
        // {
        //     nextDirTimeStamp = nextDirTime + GameManager.time;
        //     alt *= -1;
        // }
        if (destinations.Count > 0)
        {
            //print("OK");
            if(Vector3.Distance(transform.position, 
                destinations[currentDest]) <= .1f ||
               direction == Vector3.zero)
            {
                //print("change");
                currentDest = (currentDest + 1) % destinations.Count;
                direction = (destinations[currentDest] -
                             transform.position).normalized;
            }
            for (int i = 0; i < players.Count; i++)
                players[i].transform.position += direction * moveSpeed;
            //players[i].transform.position += new Vector3(.1f, 0, 0) * alt;
            transform.position += direction * moveSpeed;
        }
        //base.Update();

    }
}
