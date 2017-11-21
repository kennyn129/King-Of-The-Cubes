using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTile : Tile {

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            //print("hllo");
            Player p = collision.transform.GetComponent<Player>();
            //p.canPerformAction = false;
            //if (!p.isSliding)
            //{
                p.isSliding = true;
              /*  if (p.isMoving)
                {
                    if (p.dir == 0)
                        p.slideSpd[0] = p.moveSpd;
                    else if (p.dir == 1)
                        p.slideSpd[0] = -p.moveSpd;
                    else if (p.dir == 2)
                        p.slideSpd[1] = -p.moveSpd;
                    else if (p.dir == 3)
                        p.slideSpd[1] = p.moveSpd;
                }
            }*/
            //p.transform.position += new Vector3(0, 0, .1f);
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            //print("hllo");
            Player p = collision.transform.GetComponent<Player>();
            //p.canPerformAction = false;
            p.isSliding = false;
            //p.transform.position += new Vector3(0, 0, .1f);
        }
    }
    // Use this for initialization
    protected void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected void Update () {
        base.Update();
	}
}
