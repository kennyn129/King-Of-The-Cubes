using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : Item
{
    //public float timeToLive, activeTimeStamp;
    //public int uses, team;
    //public bool activate;
    public float timeBeforeExplosion;
    public GameObject explosionPrefab;

	public AudioSource audioSource;
	public AudioClip explosionSound;


    // Use this for initialization
    protected override void Start()
    {
		audioSource = GetComponent<AudioSource> ();

        base.Start();
        activate = false;
        uses = 1;
        timeBeforeExplosion = 15f;
        activate = false;
        taken = false;
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
            if (activeTimeStamp < GameManager.time)
            {
				audioSource.Stop ();
				audioSource.clip = explosionSound;
				audioSource.Play ();
                //transform.GetComponent<SphereCollider>().enabled = true;

                //if (activeTimeStamp + explosionDuration < GameManager.time)
                //{
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = transform.position;
                explosion.transform.SetParent(GameManager.gameManager.inGameParticlesAndEffects.transform);

                Destroy(gameObject, 2f);
                //}
            }
        }
    }

    public override int Use(PlayerController p)
    {

        transform.GetComponent<MeshRenderer>().enabled = true;
        transform.position = p.transform.position;

        activeTimeStamp = GameManager.time + timeBeforeExplosion;
        transform.SetParent(GameManager.gameManager.inGameParticlesAndEffects.transform);
        return UpdateUse(p);
    }
}
