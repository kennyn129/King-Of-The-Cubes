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
	public AudioClip explosionTimerSound;
    public bool lightedUp;


    // Use this for initialization
    protected override void Start()
    {
		audioSource = GetComponent<AudioSource> ();

        base.Start();
        activate = false;
        uses = 1;
        timeBeforeExplosion = 15f;
        activate = false;
        lightedUp = false;
        taken = false;
    }
    /*
    public override void Blink(float percentageOfTime)
    {
        Renderer r = transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
        if (r)
        {
            Color c = r.material.color;
            float blinkFrequency = .9f;
            if (percentageOfTime / .35f < .5f)
                blinkFrequency = .7f;
            c.a += alternateFade * (1 - blinkFrequency);
            if (c.a <= 0 || c.a >= 1)
                alternateFade *= -1;
            foreach (Transform child in transform.GetChild(0))
                child.GetComponent<Renderer>().material.color = c;
        }
    }*/

    public IEnumerator LightItUp()
    {
        //transform.GetChild(0).gameObject.SetActive(true);// = true;
       
        lightedUp = true;
        audioSource.clip = explosionTimerSound;
        audioSource.Play();
        yield return new WaitForSeconds(explosionTimerSound.length);
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.transform.SetParent(GameManager.gameManager.inGameParticlesAndEffects.transform);

        Destroy(gameObject);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (activeTimeStamp <= GameManager.time && !taken)
        {
            Destroy(gameObject);
        }
        //transform.gameObject.SetActive(lightedUp || !taken);
        //if (activate)
        //{
            //StartCoroutine(LightItUp());
            //audioSource.Stop();
        //    if (activeTimeStamp < GameManager.time)
        //    {
                //transform.GetComponent<SphereCollider>().enabled = true;

                //if (activeTimeStamp + explosionDuration < GameManager.time)
                //{
                
                //}
        //    }
        //}
    }

    public override int Use(PlayerController p)
    {

        transform.GetComponent<MeshRenderer>().enabled = true;
        transform.position = p.transform.position;

        activeTimeStamp = GameManager.time + timeBeforeExplosion;
        transform.SetParent(GameManager.gameManager.inGameParticlesAndEffects.transform);
        //transform.GetChild(0).gameObject.SetActive(true);
        int usages = UpdateUse(p);
        StartCoroutine(LightItUp());
        return usages;
    }
}
