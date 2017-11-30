using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTile : Tile {
    public GameObject explosionPrefab;
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            int i = Random.Range(0, 100);
            if (i <= 20)
            {
                Break();
            }
        }
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.transform.SetParent(GameManager.gameManager.inGameParticlesAndEffects.transform);
    }

    public override void Break()
    {
        base.Break();
        Explode();
    }
}
