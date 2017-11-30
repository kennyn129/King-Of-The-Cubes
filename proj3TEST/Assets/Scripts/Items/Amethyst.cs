using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amethyst : Item
{
    public GameObject amethystShardPrefab;

    public override int Use(PlayerController p)
    {
        //transform.GetComponent<MeshRenderer>().enabled = true;
        //transform.position = p.transform.position;
        GameObject newShard = Instantiate(amethystShardPrefab);
        newShard.transform.position = p.transform.position + new Vector3(0, 2, 0);
        Vector3 dir = p.GetDirection();
        dir = new Vector3(dir.x * 2, 1, dir.z * 2);
        newShard.transform.GetComponent<AmethystShard>().dir = dir;
        newShard.transform.eulerAngles = new Vector3(dir.x, 0, dir.z);
        newShard.transform.GetComponent<AmethystShard>().team = p.team;
        newShard.transform.GetComponent<AmethystShard>().fragmentCount = 1;


        newShard.transform.GetComponent<Rigidbody>().AddForce(dir * 300);
        newShard.transform.SetParent(GameManager.gameManager.inGameParticlesAndEffects.transform);
        return UpdateUse(p);
    }

    public override void Blink(float percentageOfTime)
    {
        Renderer r = transform.GetChild(0).GetComponent<Renderer>();
        if (r)
        {
            Color c = r.material.color;
            float blinkFrequency = .9f;
            if (percentageOfTime / .35f < .5f)
                blinkFrequency = .7f;
            c.a += alternateFade * (1 - blinkFrequency);
            if (c.a <= 0 || c.a >= 1)
                alternateFade *= -1;
            transform.GetChild(0).GetComponent<Renderer>().material.color = c;
        }
    }

    protected override void Start()
    {
        base.Start();
        activate = false;
        uses = 3;
        activate = false;
        taken = false;
    }

    protected override void Update()
    {
        base.Update();
        if (activeTimeStamp <= GameManager.time && !taken)
        {
            Destroy(gameObject);
        }
        transform.GetChild(0).gameObject.SetActive(!taken);
    }
}
