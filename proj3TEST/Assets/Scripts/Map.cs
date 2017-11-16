using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    //    public static Map mapManager;
    static int mapCt = 0;
    GameObject tilesHolder;
    GameObject[] playerSpawnPoints;
    GameObject itemDropHolder;

    int[] mapLayout;    // Each element consists of what the tile for the terrain will be, -1 == empty
    public GameObject[] tilePrefabs;
    public GameObject[] itemDropPrefabs;
    public int dim, mapID;

    

    void Start () {
        

        mapID = mapCt;
        mapCt++;


        //Texture2D bitmap = MapLibrary.bitmaps[mapID];
        
        int playerSpawn = 0;
        playerSpawnPoints = new GameObject[4];
        for (int i = 0; i < 4; i++)
            playerSpawnPoints[i] = transform.Find("Player Spawn " + (i + 1)).gameObject;

        tilesHolder = transform.Find("Tiles Holder").gameObject;
        //itemDropHolder = transform.Find("ItemDrops Holder").gameObject;
//        if (!mapManager)
//            mapManager = this;
        dim = 25;
        //mapLayout = new int[dim * dim];
        //for (int i = 0; i < dim * dim; i++)
        //{
        //    mapLayout[i] = 0;
        //}

        List<GameObject> tpTiles = new List<GameObject>();
        /*
        for (int y = 0; y < bitmap.height; y++)
            for (int x = 0; x < bitmap.width; x++)
            {
                int k;
                //if (i > bitmap.height / 2)
                //    k = 0;
                //else
                //    k = 1;
                //k = 0;
                //k = MapLibrary.maps25x25[mapID, i, j];

                Color32 c = bitmap.GetPixel(x, y);// *255;
                Color cc = bitmap.GetPixel(x, y) ;
                //string hexa = (int)c.r + "" + (int)c.g + "" + (int)c.b;
                //print(hexa);
                //print(RGBtoString(c));
                k = MapLibrary.ColorToTile(c);
                if (k == -1)
                    print(cc+ " " + x + " " + y + " " + c + " " +MapLibrary.RGBtoString(c) + " " + k);
                //print(k);
                if (k % 2 == 0 && k > 0)
                {
                    if (playerSpawn < 4)
                    {
                        playerSpawnPoints[playerSpawn].transform.position = transform.position + new Vector3(x * 1.5f, 0, y * 1.5f);
                        playerSpawn++;
                    }
                    k /= 2;
                }
                else if (k > 2)
                {
                    k = (k + 1) / 2;
                }
                GameObject tile = Instantiate(tilePrefabs[k]);
                tile.transform.GetComponent<Tile>().tileID = (y * dim) + x;
                tile.transform.SetParent(tilesHolder.transform);
                tile.transform.position = transform.position + new Vector3(x * 1.5f, 0, y * 1.5f);
                if (k == 5)
                {
                    tpTiles.Add(tile);
                }
            }*/
                //print(bitmap.GetPixel(j, i) * 255);
        
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
                k = MapLibrary.maps25x25[mapID,i,j];
                if (k % 2 == 0 && k > 0)
                {
                    if (playerSpawn < 4)
                    {
                        playerSpawnPoints[playerSpawn].transform.position = transform.position + new Vector3(j * 1.5f, 0, i * 1.5f);
                        playerSpawn++;
                    }
                    k /= 2;
                }else if (k > 2)
                {
                    k = (k + 1) / 2;
                }
                GameObject tile = Instantiate(tilePrefabs[k]);
                tile.transform.GetComponent<Tile>().tileID = (i * dim) + j;
                tile.transform.SetParent(tilesHolder.transform);
                tile.transform.position = transform.position + new Vector3(j*1.5f, 0, i * 1.5f);
                if (k == 5)
                {
                    tpTiles.Add(tile);
                }
            }
        }
        List<int> tpTileMap = (List<int>) MapLibrary.teleportTiles[mapID];
        if (tpTileMap != null)
        {
            //print("OKAOSK");
            for (int i = 0; i < tpTiles.Count; i++)
            {
                TeleportTile t = tpTiles[i].transform.GetComponent<TeleportTile>();
                int rcver = tpTileMap[i];
                //print(rcver);
                if (rcver >= 0)
                {
                    t.receiver = tpTiles[rcver];
                    tpTiles[rcver].transform.GetComponent<TeleportTile>().senders.Add(t.gameObject);
                    //print("HERE");
                }
            }
        }
        //Reset();
        //SetSuddenDeathTimeStamps3(20f);
        //SetSuddenDeathTimeStamps2(0, 30f);
//        SetSuddenDeathTimeStamps1(0, 30f);
        //SetSuddenDeathTimeStamps(0, 10f);
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
        if(dim % 2 == 0)
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
        for(o = 0, c -= dir, r += dir; o < offset; o++)
        {
            tilesHolder.transform.GetChild(r * dim + c).GetComponent<Tile>().suddenDeathTimeStamp = deathTime;
            deathTime += timeDiff;
            r += dir;
        }
        r -= dir;
        SetSuddenDeathTimeStamp3Helper(r, c - dir, offset+1, deathTime, dir * -1);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnItem();
        }
        //print(tilesHolder.transform.childCount);
    }
    public void SpawnItem()
    {
        //bool validToSpawnItem = false;
        List<GameObject> availableTiles = new List<GameObject>();
        //int tileCt = dim * dim;
        print(tilesHolder.transform.childCount);
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
        //tiles.Reset();
        for (int i = 0; i < 4; i++)
            GameManager.gameManager.players[i].transform.position =
                new Vector3(playerSpawnPoints[i].transform.position.x,
                            1,
                            playerSpawnPoints[i].transform.position.z);


        for (int i = 0; i < dim * dim; i++)
            tilesHolder.transform.GetChild(i).GetComponent<Tile>().Reset();


    }



}
