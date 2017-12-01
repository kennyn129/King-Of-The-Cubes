using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    //    public static Map mapManager;
    static int mapCt = 0;
    GameObject tilesHolder;
    GameObject[] playerSpawnPoints;
    GameObject itemDropHolder;

	GameManager gameManager;

    int[] mapLayout;    // Each element consists of what the tile for the terrain will be, -1 == empty
    public GameObject[] tilePrefabs;
    public GameObject[] itemDropPrefabs;
    public int height, width, mapID, dim, itemSpawnFrequency;
    public float spawnItemTimeStamp, scaling;
    public float[] spawnItemTimes;
    public bool canSpawnItems;



    void Awake()
    {

		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		mapID = gameManager.MapChoice;
        itemSpawnFrequency = 1;
        canSpawnItems = true;
        spawnItemTimes = new float[] { 20f, 50f, 120f };
		spawnItemTimeStamp = GameManager.time + spawnItemTimes[itemSpawnFrequency];
        playerSpawnPoints = new GameObject[4];
        for (int i = 0; i < 4; i++)
            playerSpawnPoints[i] = transform.Find("Player Spawn " + (i + 1)).gameObject;

        tilesHolder = transform.Find("Tiles Holder").gameObject;
        dim = 25;
        scaling = 2f;

        //SetSuddenDeathTimeStamps3(20f);
        //SetSuddenDeathTimeStamps2(0, 30f);
        //        SetSuddenDeathTimeStamps1(0, 30f);
        //SetSuddenDeathTimeStamps(0, 10f);
    }

    public void SpawnMap(int mapID, int playerCount)
    {
        Texture2D bitmap = MapLibrary.bitmaps[mapID];
        width = bitmap.width;
        height = bitmap.height;
        int playerSpawn = 0;
        

        List<GameObject> tpTiles = new List<GameObject>();
        List<GameObject> movingTiles = new List<GameObject>();
        for (int y = 0; y < bitmap.height; y++)
            for (int x = 0; x < bitmap.width; x++)
            {
                int k;
               

                Color32 c = bitmap.GetPixel(x, y);// *255;
                k = MapLibrary.ColorToTile(c);

                if (k % 2 == 0 && k > 0)
                {
                    if (playerSpawn < 4)
                    {
                        playerSpawnPoints[playerSpawn].transform.position = transform.position + new Vector3(x * scaling, 0, -y * scaling);
                        playerSpawn++;
                    }
                    k /= 2;
                }
                else if (k > 2)
                {
                    k = (k + 1) / 2;
                }
                if (k > 0)
                {
                    GameObject tile = Instantiate(tilePrefabs[k]);
                    tile.transform.GetComponent<Tile>().tileID = (y * width) + x;
                    tile.transform.SetParent(tilesHolder.transform);
                    tile.transform.position = transform.position + new Vector3(x * scaling, 0, -y * scaling);

                    if (k == 5)
                    {
                        tpTiles.Add(tile);
                    }
                    else if (k == 6)
                    {
                        movingTiles.Add(tile);
						tile.transform.position += new Vector3 (0, 1.5f, 0);
                    }
                }
            }
        //print(bitmap.GetPixel(j, i) * 255);
        /*
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
                k = MapLibrary.maps25x25[mapID, i, j];
                if (k % 2 == 0 && k > 0)
                {
                    if (playerSpawn < 4)
                    {
                        playerSpawnPoints[playerSpawn].transform.position = transform.position + new Vector3(j * 1.5f, 0, i * 1.5f);
                        playerSpawn++;
                    }
                    k /= 2;
                }
                else if (k > 2)
                {
                    k = (k + 1) / 2;
                }
                GameObject tile = Instantiate(tilePrefabs[k]);
                tile.transform.GetComponent<Tile>().tileID = (i * dim) + j;
                tile.transform.SetParent(tilesHolder.transform);
                tile.transform.position = transform.position + new Vector3(j * 1.5f, 0, i * 1.5f);
                if (k == 5)
                {
                    tpTiles.Add(tile);
                }
            }
        }*/
        List<int> tpTileMap = (List<int>)MapLibrary.teleportTiles[mapID];
        if (tpTileMap != null)
        {
            for (int i = 0; i < tpTiles.Count; i++)
            {
                TeleportTile t = tpTiles[i].transform.GetComponent<TeleportTile>();
                int rcver = tpTileMap[i];
                if (rcver >= 0)
                {
                    t.receiver = tpTiles[rcver];
                    tpTiles[rcver].transform.GetComponent<TeleportTile>().senders.Add(t.gameObject);

                }
            }
        }

        List<List<float>> destinations = (List<List<float>>)MapLibrary.movingTiles[mapID];
        List<Vector3> sizes = (List<Vector3>)MapLibrary.movingTilesSizes[mapID];
        if (destinations != null)
        {
            for (int i = 0; i < destinations.Count; i++)
            {
                MovingTile m = movingTiles[i].transform.GetComponent<MovingTile>();
                m.destinations = new List<Vector3>();

                m.transform.localScale = new Vector3(scaling * sizes[i].x, sizes[i].y, scaling * sizes[i].z);

                m.originalPosition = m.transform.position;
                List<float> coords = destinations[i];
                for (int j = 0; j < coords.Count; j += 3)
                {
                    float xScale = sizes[i].x, zScale = sizes[i].z, yScale = sizes[i].y;
                    xScale--;
                    yScale--;
                    zScale--;
                    Vector3 dest = m.originalPosition +
                        new Vector3(coords[j] , coords[j + 1], coords[j + 2] ) * scaling +
                        new Vector3(xScale, yScale, -zScale) * scaling / 2; ;
                    m.destinations.Add(dest);
                }
            }
        }
        transform.name = "Map " + (mapID + 1);
		for (int i = 0; i < 4; i++) {
			GameObject.Find("Player " + (i+1)).transform.position =
				new Vector3 (playerSpawnPoints [i].transform.position.x,
				2,
				playerSpawnPoints [i].transform.position.z);
            
        }
			
    }

    private void Update()
    {
		if (spawnItemTimeStamp <= GameManager.time && canSpawnItems)
        {
            SpawnItem();
			spawnItemTimeStamp = GameManager.time + spawnItemTimes[itemSpawnFrequency];
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnItem();
        }
    }
    public void SpawnItem()
    {
        List<GameObject> availableTiles = new List<GameObject>();
        for (int j = 0; j < tilesHolder.transform.childCount; j++)
            if (tilesHolder.transform.GetChild(j).GetComponent<Tile>().active &&
                tilesHolder.transform.GetChild(j).childCount == 0 &&
                tilesHolder.transform.GetChild(j).gameObject.active)
                availableTiles.Add(tilesHolder.transform.GetChild(j).gameObject);
        if (availableTiles.Count > 0)
        {
            int item = Random.Range(0, itemDropPrefabs.Length);
            GameObject itemDrop = Instantiate(itemDropPrefabs[item]);
            GameObject tile = availableTiles[Random.Range(0, availableTiles.Count)];
            itemDrop.transform.position = new Vector3(tile.transform.position.x, 1, tile.transform.transform.position.z);
            itemDrop.transform.SetParent(tile.transform);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < 4; i++)
            gameManager.players[i].transform.position =
                new Vector3(playerSpawnPoints[i].transform.position.x,
                            1,
                            playerSpawnPoints[i].transform.position.z);


        for (int i = 0; i < tilesHolder.transform.childCount; i++)
            tilesHolder.transform.GetChild(i).GetComponent<Tile>().Reset();


    }


    /* Tiles slowly disappear in spiral behavior from out to in */
    private void SetSuddenDeathTimeStamps(int offset, float deathTime)
    {
        if (offset > dim / 2)
        {
            return;
        }
        int index = 0, col = offset, row = offset;
        float timeDiff = .5f;
        for (; col < dim - offset; col++)
        {

            index = row * dim + col;
            tilesHolder.transform.GetChild(index).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            deathTime += timeDiff;
        }
        for (row++, col--; row < dim - offset; row++)
        {
            index = row * dim + col;
            tilesHolder.transform.GetChild(index).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            deathTime += timeDiff;
        }
        for (row--, col--; col >= offset; col--)
        {
            index = row * dim + col;
            tilesHolder.transform.GetChild(index).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            deathTime += timeDiff;
        }
        for (row--, col++; row > offset; row--)
        {
            index = row * dim + col;
            tilesHolder.transform.GetChild(index).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            deathTime += timeDiff;
        }
        SetSuddenDeathTimeStamps(offset + 1, deathTime);
    }

    /* Tiles disappear from outer to inner */
    private void SetSuddenDeathTimeStamps1(int offset, float deathTime)
    {
        if (offset > dim / 2)
        {
            return;
        }
        int index = 0, col = offset, row = offset;
        float timeDiff = 5f;
        for (; col < dim - offset; col++)
        {

            index = row * dim + col;
            tilesHolder.transform.GetChild(index).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            //deathTime += timeDiff;
        }
        for (row++, col--; row < dim - offset; row++)
        {
            index = row * dim + col;
            tilesHolder.transform.GetChild(index).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            //deathTime += timeDiff;
        }
        for (row--, col--; col >= offset; col--)
        {
            index = row * dim + col;
            tilesHolder.transform.GetChild(index).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            //deathTime += timeDiff;
        }
        for (row--, col++; row > offset; row--)
        {
            index = row * dim + col;
            tilesHolder.transform.GetChild(index).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            //deathTime += timeDiff;
        }
        SetSuddenDeathTimeStamps1(offset + 1, deathTime + timeDiff);
    }

    /* Tiles disappear from inner to outer */
    private void SetSuddenDeathTimeStamps2(int offset, float deathTime)
    {
        if (offset > dim / 2)
        {
            return;
        }

        SetSuddenDeathTimeStamps2(offset + 1, deathTime - 1f);
        int index = 0, col = offset, row = offset;
        //float timeDiff = 1f;
        for (; col < dim - offset; col++)
        {

            index = row * dim + col;
            tilesHolder.transform.GetChild(index).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            //deathTime += timeDiff;
        }
        for (row++, col--; row < dim - offset; row++)
        {
            index = row * dim + col;
            tilesHolder.transform.GetChild(index).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            //deathTime += timeDiff;
        }
        for (row--, col--; col >= offset; col--)
        {
            index = row * dim + col;
            tilesHolder.transform.GetChild(index).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            //deathTime += timeDiff;
        }
        for (row--, col++; row > offset; row--)
        {
            index = row * dim + col;
            tilesHolder.transform.GetChild(index).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            //deathTime += timeDiff;
        }
    }

    /* Tiles disappear spiraling from inner to outer */
    private void SetSuddenDeathTimeStamps3(float deathTime)
    {
        int r = dim / 2, c = r;
        if (dim % 2 == 0)
        {
            r--;
            c--;
        }

        tilesHolder.transform.GetChild(r * dim + c).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
        SetSuddenDeathTimeStamp3Helper(r, c + 1, 1, deathTime, 1);
    }

    private void SetSuddenDeathTimeStamp3Helper(int r, int c, int offset, float deathTime, int dir)
    {
        if (r < 0 || r >= dim)
            return;
        float timeDiff = .5f;
        int o = 0;
        for (o = 0; o < offset; o++)
        {
            if (c < 0 || c >= dim)
                return;
            tilesHolder.transform.GetChild(r * dim + c).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            deathTime += timeDiff;
            c += dir;
        }
        for (o = 0, c -= dir, r += dir; o < offset; o++)
        {
            tilesHolder.transform.GetChild(r * dim + c).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            deathTime += timeDiff;
            r += dir;
        }
        r -= dir;
        SetSuddenDeathTimeStamp3Helper(r, c - dir, offset + 1, deathTime, dir * -1);

    }


}
