using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager gameManager;
    public GameObject[] itemPrefabs;
    public GameObject inGameParticlesAndEffects;
    public GameObject mapHolder;
    public Item[] items;
    public static float time;
    public GameObject[] players;
    public int selectedMap;
    public Map map;

    int playerCount;
    public int playersInGame;

	// Use this for initialization
	void Start () {
        if (gameManager == null)
            gameManager = this;
        
        inGameParticlesAndEffects = GameObject.Find("InGame Particles and Effects");
        mapHolder = GameObject.Find("Map Holder");
        time = 0;
        playerCount = 4;
        playersInGame = 4;
        players = new GameObject[playerCount];
        for (int i = 0; i < playerCount; i++)
            players[i] = GameObject.FindGameObjectWithTag("Player" + (i + 1));
        //InstantiateItems();

        selectedMap = 0;
        for (int i = 0; i < MapLibrary.mapCount; i++)
        {
            Map m = Instantiate(map);
            m.transform.name = "Map " + (i + 1);
            m.transform.position += new Vector3(i * 50, 0, 0);
            //m.gameObject.SetActive(i == selectedMap);
            m.transform.SetParent(mapHolder.transform);
        }
        /*
        for( int i = 0; i < MapLibrary.bitmaps.Length; i++)
        {
            Map m = Instantiate(map);
            m.transform.name = "Map " + (i + 1);
            m.transform.position += new Vector3(i * 50, 0, 0);
            m.transform.SetParent(mapHolder.transform);
        }*/
	}
	
	// Update is called once per frame
	void Update () {
        time += .1f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedMap < mapHolder.transform.childCount)
            {
                selectedMap = (selectedMap + 1) % MapLibrary.mapCount;
                for (int i = 0; i < mapHolder.transform.childCount; i++)
                    mapHolder.transform.GetChild(i).gameObject.SetActive(i == selectedMap);
                //ResetGame();
                //mapHolder.transform.GetChild(selectedMap).gameObject.SetActive(false);
                
                //mapHolder.transform.GetChild(selectedMap).gameObject.SetActive(true);
                ResetGame();
            }
            //SceneManager.LoadScene("main");
        }
        if (Input.GetKeyDown(KeyCode.E))
            ResetGame();
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            playerCount = (playerCount + 1) % 5;
            if (playerCount < 2)
                playerCount = 2;
            playersInGame = playerCount;
            ResetGame();
        }
        if (playersInGame < 2)
        {
            if (playersInGame == 0)
                print("TIE");
            else
                print("one winner");
            ResetGame();
        }
                // SceneManager.LoadScene("main");
    }

    void ResetGame()
    {
        time = 0;

        foreach (Transform child in inGameParticlesAndEffects.transform)
        {
            Destroy(child.gameObject);
        }
        if(selectedMap < mapHolder.transform.childCount)
            mapHolder.transform.GetChild(selectedMap).transform.GetComponent<Map>().Reset();
        playersInGame = playerCount;
        for (int i = 0; i < 4; i++)
        {
            players[i].SetActive(i < playerCount);
            players[i].transform.GetComponent<PlayerController>().Reset();
        }
                //GameObject.Find("Player" + (i + 1)).SetActive(true);
    }
}
