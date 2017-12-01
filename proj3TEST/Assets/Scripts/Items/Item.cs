using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public float timeToLive, activeTimeStamp;
    public int uses, team;
    public bool activate, taken;
    protected int alternate, alternateFade;
    protected float height;

    // Use this for initialization
    protected virtual void Start()
    {
        alternate = 1;
        alternateFade = -1;
        height = 0;
        timeToLive = 70;
        activeTimeStamp = GameManager.time + timeToLive;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!taken)
        {
            transform.position += new Vector3(0, .01f * alternate, 0);
            height += 0.01f * alternate;
            if (height >= .7f || height <= -.3f)
                alternate *= -1;
            transform.Rotate(0, 0, 1);
            float timeLeft = activeTimeStamp - GameManager.time;
            float percentageOfTime = timeLeft / timeToLive;
            if (percentageOfTime < .35f)
            {
                Blink(percentageOfTime);
            }
        }
    }

    public virtual void Blink(float percentageOfTime)
    {
        Renderer r = transform.GetComponent<Renderer>();
        if (r)
        {
            Color c = r.material.color;
            float blinkFrequency = .9f;
            if (percentageOfTime / .35f < .5f)
                blinkFrequency = .7f;
            c.a += alternateFade * (1 - blinkFrequency);
            if (c.a <= 0 || c.a >= 1)
                alternateFade *= -1;
            
            transform.GetComponent<Renderer>().material.color = c;
        }
    }
    

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
                Color c = transform.GetComponent<Renderer>().material.color;
                c.a = 1;
                transform.GetComponent<Renderer>().material.color = c;
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
        uses--;
        activate = true;
        //activeTimeStamp = GameManager.time + timeToLive;

        if (uses == 0)
        {
            p.items.Remove(this);
        }
        //Destroy(gameObject);
        return uses;
    }

    public abstract int Use(PlayerController p);

}
