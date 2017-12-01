using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    protected MeshRenderer mr;
    protected BoxCollider c;
    public float respawnTimeStamp, respawnTime, floorSize, suddenDeathTimeStamp;
    public bool isBreakable, active;
    public int hp, tileID;
    public Color originalColor, currentColor;

    // Used for changing color of tiles upon focus
    Color[] colors = { Color.red, Color.blue, Color.magenta, Color.black };
    bool[] focusedBy = { false, false, false, false };


    // Use this for initialization
    protected void Start () {
        mr = transform.GetComponent<MeshRenderer>();
        c = transform.GetComponent<BoxCollider>();
        originalColor = transform.GetComponent<Renderer>().material.color;
        currentColor = originalColor;
        respawnTime = 40.0f;
        floorSize = .32f;
        Reset();
    }

    public virtual void Reset()
    {
        //transform.GetComponent<Renderer>().material.color = originalColor;
        currentColor = originalColor;
        active = true;
        respawnTimeStamp = 0;
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void FocusOnTile(int p, bool b)
    {
        focusedBy[p] = b;
        ChangeColor();
    }

    public void ChangeColor()
    {
        float r = 0, g = 0, b = 0, ct = 0;
        for (int i = 0; i < 4; i++)
        {
            if (focusedBy[i])
            {
                r += colors[i].r;
                g += colors[i].g;
                b += colors[i].b;
                ct++;
            }
        }
        if (ct > 0)
            currentColor = new Color(r, g, b) / ct;
        else
            currentColor = originalColor;
        transform.GetComponent<Renderer>().material.SetColor("_Color", currentColor);
       
    }
 

    public void SetRespawnTime()
    {
        respawnTimeStamp = GameManager.time + respawnTime;
    }

    public virtual void Break()
    {
        SetRespawnTime();
        active = false;
    }
	
	// Update is called once per frame
	protected void Update ()
    {
		active = respawnTimeStamp <= GameManager.time; // && suddenDeathTimeStamp >= GameManager.time;
        mr.enabled = active;
        c.enabled = active;
    }
}
