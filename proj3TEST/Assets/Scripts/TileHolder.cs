using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHolder : MonoBehaviour {

    int[] mapLayout;    // Each element consists of what the tile for the terrain will be, -1 == empty
    public GameObject[] tilePrefabs;
    public GameObject[] itemDropPrefabs;
    int dim;
    // Use this for initialization

    void Start()
    {
        dim = 25;
        mapLayout = new int[dim * dim];
        for (int i = 0; i < dim * dim; i++)
        {
            mapLayout[i] = 0;
        }


        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                int k;
                if (i > dim / 2)
                    k = 0;
                else
                    k = 1;
                k = 0;

                GameObject tile = Instantiate(tilePrefabs[k]);
                tile.transform.SetParent(transform);
                tile.transform.position = new Vector3(j * 1.48f, 0, i * 1.48f);
                //RectTransform rt = tile.transform.GetComponent<RectTransform>();
                //tile.transform.position = new Vector3(rt.rect.width * j * 5 - translateX, rt.rect.height * i * 5 - translateY, 0);// - new Vector3(translateX,translateY,0);
            }
        }
    }

    public void Reset()
    {
        for (int i = 0; i < dim * dim; i++)
            transform.GetChild(i).GetComponent<Tile>().Reset();
    }

    public void SpawnItem()
    {
        print("spawning");
        bool validToSpawnItem = false;
        int tileCt = dim * dim;
        //for(int j = 0; j < tileCt; j++)
        //    if(transform)
        int i = Random.Range(0, dim * dim);
        while (!transform.GetChild(i).gameObject.activeSelf)
            i = Random.Range(0, dim * dim);
        GameObject itemDrop = Instantiate(itemDropPrefabs[0]);
        itemDrop.transform.position = new Vector3(transform.GetChild(i).position.x, 1, transform.GetChild(i).position.z);
    }
}
