using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static KeyCode[] p1ctrls = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Space };
    static KeyCode[] p2ctrls = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.G };
    static KeyCode[] p3ctrls = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Space };
    static KeyCode[] p4ctrls = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Space };
    static KeyCode[][] playerControls = { p1ctrls, p2ctrls, p3ctrls, p4ctrls };
    public static int numPlayers;
    public float moveSpd = 0.5f, timer, iceFriction, floorFriction, crntMoveSpd,
                 moveAccel, slideAccel, knockAccel, maxMoveVelocity;
    public Vector3 velocity;
    //public float[] slideAccel, slideSpd;
    public bool falling, canPerformAction, wasSliding, isSliding, isMoving;
    public int id,dir,team;
    Rigidbody rb;
    SpriteRenderer sr;
    public Item[] items;
    public Transform direction;
    public List<GameObject> flooring;
    public Weapon weapon;
    public bool[] touchingFloor;
	// Use this for initialization
	void Start () {
        items = new Item[1];
        id = numPlayers;
        crntMoveSpd = 0;
        numPlayers++;
        moveAccel = 8f;
        slideAccel = .05f;
        velocity = new Vector3(0,0,0);
        maxMoveVelocity = 1;
        floorFriction = .65f;
        //slideSpd = new float[] { 0, 0};
        //slideAccel = new float[] { .05f, .05f };
        iceFriction = .98f;
        falling = false;
        wasSliding = false;
        rb = transform.GetComponent<Rigidbody>();
        direction = transform.Find("Direction");
        sr = transform.GetComponent<SpriteRenderer>();
        //onLand = 1;
        weapon = direction.Find("Weapon").GetComponent<Weapon>();
        timer = 0;
        canPerformAction = true;
        touchingFloor = new bool[9];
        for (int i = 0; i < touchingFloor.Length; i++)
            touchingFloor[i] = true;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "ItemDrop")
        {
            print(1);
        }
    }

    /*private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.tag == "Tile")
            falling = false;

    }*/

    /*

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Tile")
        {
            Tile t = collision.transform.GetComponent<Tile>();
            if (t.isHole)
            {
                //falling = true;
                print("HEY");
               // onLand = 0;
                //onLand--;
            }
           // else
           // {
               // print("NOT FALSE");
                //onLand++;
            //}
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Tile")
        {
            Tile t = collision.transform.GetComponent<Tile>();
            if (t.isHole)
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
            }
            else
            {
                //print("NOT FALSE");
                //onLand++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.transform.tag == "Tile")
        //{
        //    Tile t = collision.transform.GetComponent<Tile>();
        //    if (t.active)
         //       onLand--;
        //}
                //falling = true;
    }*/

    public void Fall()
    {
        transform.localScale /= 1.05f;
       
        sr.sortingOrder -= 1;
        //rb2D.gravityScale += 1;
    }

    public void TouchingFloor(int detector)
    {
        touchingFloor[detector % touchingFloor.Length] = true;
    }

    public void NoFloor(int detector)
    {
        touchingFloor[detector % touchingFloor.Length] = false;
    }

    public void BreakFloor()
    {
        if (timer > 5.0f)
        {
            weapon.transform.GetComponent<Weapon>().hitSomething = false;
            weapon.gameObject.SetActive(true);
            timer = 0;
        }
    }

    void PlayerControls()
    {
        isMoving = false;
        //crntMoveSpd = 0;
        if (Input.GetKey(playerControls[id][0])) // Move Up
        {
            isMoving = true;
            dir = 0;
            if (isSliding)
            {
                velocity += new Vector3(0,0,slideAccel);
                //slideSpd[0] += slideAccel[0];
                //transform.position += new Vector3(0, 0, slideSpd[0]);
            }
            else
            {
                crntMoveSpd = moveSpd;
                velocity += new Vector3(0,0,moveAccel);
                //slideSpd[0] = 0;
                //transform.position += new Vector3(0, 0, crntMoveSpd);
            }
            //print("HEY");
            //rb.AddForce(new Vector3(0,0,10));//, 0);
            Vector3 angle = transform.eulerAngles;
            angle.y = 0;
            direction.eulerAngles = angle;//new Quaternion(0,0,0,0);
        }
        if (Input.GetKey(playerControls[id][1])) // Move Down
        {
            isMoving = true;
            dir = 1;
            if (isSliding)
            {
                velocity -= new Vector3(0, 0, slideAccel);
                //slideSpd[0] += slideAccel[0];
                //transform.position += new Vector3(0, 0, slideSpd[0]);
            }
            else
            {
                crntMoveSpd = moveSpd;
                velocity -= new Vector3(0, 0, moveAccel);
                //slideSpd[0] = 0;
                //transform.position += new Vector3(0, 0, crntMoveSpd);
            }
            Vector3 angle = transform.eulerAngles;
            angle.y = 180;
            direction.eulerAngles = angle;
            //direction.localRotation = new Quaternion(0, 0, 180, 0);
        }
        if (Input.GetKey(playerControls[id][2])) // Move Left
        {
            isMoving = true;
            dir = 2;
            //print("HEY");
            if (isSliding)
            {
                velocity -= new Vector3(slideAccel,0,0);
                //slideSpd[0] += slideAccel[0];
                //transform.position += new Vector3(0, 0, slideSpd[0]);
            }
            else
            {
                crntMoveSpd = moveSpd;
                velocity -= new Vector3(moveAccel,0,0);
                //slideSpd[0] = 0;
                //transform.position += new Vector3(0, 0, crntMoveSpd);
            }
            //transform.position += new Vector3(-moveSpd, 0, 0);
            Vector3 angle = transform.eulerAngles;
            angle.y = 270;
            direction.eulerAngles = angle;
            //direction.localRotation = new Quaternion(0, 0, 90, 0);
        }
        if (Input.GetKey(playerControls[id][3])) // Move Right
        {
            isMoving = true;
            dir = 3;
            //print("HEY");
            if (isSliding)
            {
                velocity += new Vector3(slideAccel, 0, 0);
                //slideSpd[0] += slideAccel[0];
                //transform.position += new Vector3(0, 0, slideSpd[0]);
            }
            else
            {
                crntMoveSpd = moveSpd;
                velocity += new Vector3(moveAccel, 0, 0);
                //slideSpd[0] = 0;
                //transform.position += new Vector3(0, 0, crntMoveSpd);
            }
            //transform.position += new Vector3(moveSpd, 0, 0);
            Vector3 angle = transform.eulerAngles;
            angle.y = 90;
            direction.eulerAngles = angle;
            // direction.localRotation = new Quaternion(0, 0, 270, 0);
        }
        if (Input.GetKey(playerControls[id][4])) // Attack
        {
            if (weapon.CanUse())
            {

                weapon.Action();
                //print("HEY");
                //BreakFloor();
            }
            //transform.position += new Vector3(moveSpd, 0, 0);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
        if (transform.position.y <= -100)
        {
            print("HEY");
            Die();
        }
        if (items[0] != null)
            print("OBTAIN");
        //Rigidbody dirRB = direction.GetComponent<Rigidbody>();
        direction.position = transform.position;//new Vector3(direction.position.x,transform.position.y,direction.position.z);
        float angle = direction.eulerAngles.y;
        Vector3 d = new Vector3(0, 0, 0);
        if (angle == 0)
        {
            d.z = 2;
        }
        else if (angle == 180)
        {
            d.z = -2;
        } else if (angle == 90)
        {
            d.x = 2;
        }
        else if (angle == 270)
        {
            d.x = -2;
        }
        //dirRB
        //rb.transform.position = new Vector3(rb.transform.position.x, 0, rb.transform.position.z);// = transform.position;
        //weapon.transform.localPosition = new Vector3( )
        //else
        //    print(transform.localPosition.y);
        //print("slide:" + wasSliding);
        //print(slideSpd[0] < .00000001);
        //print("val:" +slideSpd[0]);
        //onLand--;
        //if (onLand > 4)
        //     onLand = 4;
        //if (onLand < 0)
        //    onLand = 0;
        //Map.mapManager.onFloor(this);
        //timer += .1f;
        //weapon.gameObject.SetActive(timer < 1.0f);
        /*
        if(!falling)
        {
            //falling = true;
            int a = 0;
            for(int i = 0; i < touchingFloor.Length; i++)
            {
                if (!touchingFloor[i])
                {
                    a++;
                }
            }
            //print(a);
            falling = a == 9;
        }
           // falling = !(touchingFloor[0] || touchingFloor[1] || touchingFloor[2] || touchingFloor[3]); //onLand <= 0;
        if (falling)
        {
            print("FALLING");
            Fall();
        }*/
        //else
        //rb2D.gravityScale = 0;
        //falling = false;
        if (isMoving) {
            if (velocity.x > maxMoveVelocity)
                velocity.x = maxMoveVelocity;
            if (velocity.x < -maxMoveVelocity)
                velocity.x = -maxMoveVelocity;
            if (velocity.z > maxMoveVelocity)
                velocity.z = maxMoveVelocity;
            if (velocity.z < -maxMoveVelocity)
                velocity.z = -maxMoveVelocity;
        }
        //velocity = Vector3.Normalize(velocity);
        //velocity = velocity.normalized;
        if (isSliding)
        {
            velocity *= iceFriction;
            //transform.position += velocity;
            //velocity += new Vector3(slideAccel, 0, 0);
            //slideSpd[0] += slideAccel[0];
            //transform.position += new Vector3(0, 0, slideSpd[0]);
        }
        else
        {

            velocity *= floorFriction;
            //slideSpd[0] = 0;
            //transform.position += new Vector3(0, 0, crntMoveSpd);
        }
        transform.position += velocity;
        if (canPerformAction)
        {
            PlayerControls();

        }

        wasSliding = isSliding;
        //falling = true;
    }
}
