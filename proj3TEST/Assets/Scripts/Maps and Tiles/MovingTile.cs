using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : Tile {
    public List<GameObject> players;
    public List<Vector3> destinations;
    //public int alt = 1;
    //public float nextDirTimeStamp, nextDirTime;
    public Vector3 direction, originalPosition, prevPosition;
    public float moveSpeed;
    public int currentDest;

    float t, timeToReachTarget;

    protected new void Start()
    {
        base.Start();
        currentDest = 0;
        moveSpeed = .125f;
        t = 2.5f;
        timeToReachTarget = 3f;
        currentDest = 0;
        direction = Vector3.zero;
        players = new List<GameObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            //     && active && receiver && receiver.transform.GetComponent<TeleportTile>().active)
        {
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
            players.Remove(collision.gameObject);
            //collision.transform.SetParent(null);
            //collision.transform.position = receiver.transform.position + new Vector3(0, .2f, 0);
        }
    }

    public override void Reset()
    {
        base.Reset();
        transform.position = originalPosition;
        //prevPosition = null;
        currentDest = 0;
        //print("?");
        //transform.position = new Vector3(220, -1, 10);
        //nextDirTimeStamp = nextDirTime + GameManager.time;
        //alt = 1;
    }

    public override void Break()
    {
        //print("CAnt break");
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
            if (Vector3.Distance(transform.position,
                destinations[currentDest]) <= .1f ||
               direction == Vector3.zero)
            {
                prevPosition = destinations[currentDest];
                currentDest = (currentDest + 1) % destinations.Count;
                direction = (destinations[currentDest] -
                             transform.position).normalized;
                // t = 0;
            }
            float spd = t / Vector3.Distance(prevPosition, destinations[currentDest]);
            for (int i = 0; i < players.Count; i++) {
                Vector3 offset = Vector3.MoveTowards(transform.position, destinations[currentDest], moveSpeed) -transform.position;
                players[i].transform.position += offset; //= offset + Vector3.MoveTowards(transform.position,
                    //new Vector3(destinations[currentDest].x, players[i].transform.position.y, destinations[currentDest].z), spd);
            }
            //players[i].transform.position += direction * moveSpeed;
            //players[i].transform.position += new Vector3(.1f, 0, 0) * alt;
            //f//loat step = 20 * Time.deltaTime;
            //float tt = 10f;
            // t += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.MoveTowards(transform.position, destinations[currentDest], moveSpeed);
            //transform.position = Vector3.Lerp(transform.position, destinations[currentDest], moveSpeed * Time.deltaTime);/// Vector3.Distance(prevPosition,destinations[currentDest]));
            //transform.position += direction * moveSpeed;
        }
        //base.Update();

    }
}
