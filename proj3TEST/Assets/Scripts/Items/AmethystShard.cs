using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmethystShard : MonoBehaviour
{
    public GameObject amethystShardPrefab;
    public float timeToLive, activeTimeStamp, gravity;
    public int maxShards, minShards, fragmentCount, team;
    public Vector3 dir;
    float spread;

	public AudioSource audioSource;
	public AudioClip amethystSound;
    // Use this for initialization
    void Start()
    {
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = amethystSound;
		//audioSource.Play ();
        gravity = 20;
        minShards = 2;
        maxShards = 5;
        timeToLive = 30;
        activeTimeStamp = GameManager.time + timeToLive;
        spread = 45;
    }

    private void FixedUpdate()
    {
        transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTimeStamp <= GameManager.time)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator PlayAudio()
    {
        audioSource.Play();
        Color c = new Color();
        c.a = 0;
        GetComponent<Renderer>().material.color = c;
        GetComponent<BoxCollider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (fragmentCount > 0)
            {
                //float[] angle = new float[] { 30, 40, 50, 60,70 };

                int shards = Random.Range(minShards, maxShards);
                shards = 5;
                for (int i = 0; i < shards; i++)
                {
                    GameObject shard = Instantiate(amethystShardPrefab);
                    //print(Quaternion.Euler(transform.eulerAngles.x , 0, transform.eulerAngles.z));
                    //Quaternion angleAdjustment = Quaternion.FromToRotation()
                    shard.transform.position = transform.position + new Vector3(0, 1.5f, 0);
                    shard.transform.GetComponent<AmethystShard>().fragmentCount = fragmentCount - 1;
                    Vector3 newDir = dir;
                    shard.transform.localScale = transform.localScale / 2;

                    //float x, z;
                    //x = //Random.Range(-1, 1);
                    //z = //Random.Range(-1, 1);
                    //newDir += new Vector3(x, .5f, z);
                    float angle = i * 90 / (shards - 1) - spread + Vector3.Angle(transform.position, newDir);
                    newDir = new Vector3(Mathf.Cos(angle) * newDir.x, newDir.y, newDir.z * Mathf.Sin(angle));
                    shard.transform.GetComponent<AmethystShard>().dir = newDir;
                    shard.transform.GetComponent<AmethystShard>().team = team;
                    shard.transform.GetComponent<Rigidbody>().AddForce(200 * newDir);
                    shard.transform.SetParent(GameManager.gameManager.inGameParticlesAndEffects.transform);
                }

                StartCoroutine(PlayAudio());
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player") &&
            other.transform.GetComponent<PlayerController>().team != team)
        {

            Vector3 newDir = new Vector3(dir.x * 50, 10, dir.z * 50);
            other.transform.GetComponent<Rigidbody>().AddForce(newDir.normalized * 500 * (1 + fragmentCount));
            PlayerController p = other.transform.GetComponent<PlayerController>();
            //p.audioSource.Stop();
            p.audioSource.clip = p.hitSound;
            p.audioSource.Play();
            Destroy(gameObject);
        }
    }
}
