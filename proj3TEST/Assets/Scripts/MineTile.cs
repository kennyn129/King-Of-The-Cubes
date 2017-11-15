using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTile : Tile {
    public GameObject explosionPrefab;
    private void OnCollisionExit(Collision collision)
    {
        print(collision.transform.name);
        int i = Random.Range(0, 100);
        print(i);
        if (i <= 20)
        {
            print("explode");
            Break();
        }
    }

    public void Explode()
    {
        Instantiate(explosionPrefab).transform.position = transform.position;
    }

    public override void Break()
    {
        print("boom");
        base.Break();
        Explode();
    }
}
