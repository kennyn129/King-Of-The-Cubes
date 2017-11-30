using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayingTile : Tile {
    public float decayTime, decayTimeStamp, waitToResetOpacity;
    public bool triggered;
	// Use this for initialization
	protected new void Start () {
        base.Start();
        decayTime = 10f;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (active && !triggered && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            triggered = true;
            decayTimeStamp = GameManager.time + decayTime;
        }
    }

    public override void Reset()
    {
        base.Reset();
        triggered = false;
        decayTimeStamp = 0;
        waitToResetOpacity = 0;
    }

    public override void Break()
    {
        base.Break();
        waitToResetOpacity = GameManager.time + respawnTime;
    }

    // Update is called once per frame
    protected new void Update () {
        base.Update();
        if (!active && triggered)
        {
            triggered = false;
            transform.GetComponent<Renderer>().material.color = originalColor;
        }
        if (triggered)
        {
            Color c = transform.GetComponent<Renderer>().material.color;
            c.a = (decayTimeStamp - GameManager.time) / decayTime;
            transform.GetComponent<Renderer>().material.color = c;
        }
        if (decayTimeStamp <= GameManager.time && triggered && active)
        { 
            Break();
        }
	}
}
