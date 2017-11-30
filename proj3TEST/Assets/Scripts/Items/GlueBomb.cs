using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueBomb : Item
{
    public GameObject adhesivePrefab;
    // Use this for initialization
    public float y, x, gravity;

	public AudioSource audioSource;
	public AudioClip glueSound;

    protected override void Start()
    {
        base.Start();
        //timeToLive = 60f;
        //activeTimeStamp = GameManager.time + timeToLive;
        uses = 1;
        activate = false;
        taken = false;
        y = 10f;
        x = 10f;
        gravity = 20;
		audioSource = GetComponent<AudioSource> ();

    }

    private void FixedUpdate()
    {
        if (activate)
            transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity, 0));
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (activeTimeStamp <= GameManager.time && !taken)
        {
            Destroy(gameObject);
        }
        if (activate)
        {
			audioSource.Stop ();
			audioSource.clip = glueSound;
			audioSource.Play ();
            //transform.GetComponent<Rigidbody>().velocity = new Vector3(x,y,0);
            // transform.
            //y -= .4f;
            if (activeTimeStamp < GameManager.time)
            {
                //transform.GetComponent<SphereCollider>().enabled = true;

                //if (activeTimeStamp + explosionDuration < GameManager.time)
                //{
                //GameObject explosion = Instantiate(explosionPrefab);
                //explosion.transform.position = transform.position;
                Destroy(gameObject);
                //}
            }
        }

    }


    protected new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (activate && other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            GameObject adhesive = Instantiate(adhesivePrefab);
            adhesive.transform.position = other.transform.position + new Vector3(0, .5f, 0);
            adhesive.transform.GetComponent<Adhesive>().tile = other.transform.GetComponent<Tile>();
            adhesive.transform.SetParent(GameManager.gameManager.inGameParticlesAndEffects.transform);
            Destroy(gameObject);
            activate = false;
            //PlayerController p = other.transform.GetComponent<PlayerController>();
            //Interaction(p);
        }
    }

    public override int Use(PlayerController p)
    {

        transform.position = p.transform.position + new Vector3(0, 1, 0);
        Vector3 dir = p.GetDirection();
        dir = new Vector3(dir.x * 2, 2f, dir.z * 2);
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.GetComponent<Rigidbody>().AddForce(dir * 300);
        transform.GetComponent<MeshRenderer>().enabled = true;
        activeTimeStamp = GameManager.time + timeToLive;
        return UpdateUse(p);
    }
}
