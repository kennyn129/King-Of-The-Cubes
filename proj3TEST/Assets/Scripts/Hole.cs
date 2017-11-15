using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : Tile {
   /* SpriteRenderer sr;
    BoxCollider2D c2D;
    public float respawnTimeStamp, respawnTime, floorSize;
    public bool isBreakable, active, isHole;
    public int hp;
    public GameObject holePrefab;*/


    // Use this for initialization
    protected void Start()
    {
        base.Start();
        //sr = transform.GetComponent<SpriteRenderer>();
        //c2D = transform.GetComponent<BoxCollider2D>();
        //hole = transform.GetComponent<CircleCollider2D>();
        //respawnTime = 60.0f;
        //respawnTimeStamp = 0;
        //floorSize = .32f;
        //print("INST");
    }

   

    // Update is called once per frame
    void Update()
    {
        active = respawnTimeStamp >= GameManager.time;
        /*if (isHole)
        {
            print("TIME");
            print(GameManager.time);
            print("STAMP");
            print(respawnTimeStamp);
            print(active);
            if (active)
            {
                print(1);
                Destroy(gameObject);
            }
        }
        else
        {*/
        //sr.enabled = active;
        //c2D.enabled = active;
        //}
        if (active)
        {
            //if (transform.childCount > 0)
            //{
                Destroy(gameObject);
            //}
            //    c2D.size = new Vector3(floorSize, floorSize, 0);
        }
        //else
        // {
        //    c2D.size = new Vector3(holeSize, holeSize, 0);
        //}
        //hole.enabled = !active;

    }
}
