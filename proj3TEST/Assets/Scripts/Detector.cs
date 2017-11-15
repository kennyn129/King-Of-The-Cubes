using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour {
    public static int ID;
    int id;
	// Use this for initialization
	void Start () {
        id = ID;
        ID++;
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Hole")
        {
            //Tile t = collision.transform.GetComponent<Tile>();
            if (transform.parent && transform.parent.tag == "Player")
            {
                print("FLOOR" + id);

                Player p = transform.parent.GetComponent<Player>();
                p.NoFloor(id);
                //p.falling = true;
            }
            /*if (t.isHole)
            {
                print("HOLE");

                float dist = Vector3.Distance(t.transform.GetComponent<Renderer>().bounds.center, transform.GetComponent<Renderer>().bounds.center);
                print(dist);
                if (dist < 1.0f)
                {
                    falling = true;
                    print("HEY");
                    //   onLand = 0;
                }
                //onLand--;
            }*/
            //else
            //{
                //print("NOT FALSE");
                //onLand++;
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Hole")
        {
            if (transform.parent && transform.parent.tag == "Player")
            {
                print("NOFLOOR" + id);
                Player p = transform.parent.GetComponent<Player>();
                p.TouchingFloor(id);
            }
            //    Tile t = collision.transform.GetComponent<Tile>();
            //    if (t.active)
            //       onLand--;
        }
        //falling = true;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
