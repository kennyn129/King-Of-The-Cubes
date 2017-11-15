using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager gameManager;
    public GameObject[] itemPrefabs;
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
        time = 0;
        playerCount = 4;
        playersInGame = 4;
        players = new GameObject[playerCount];
        for (int i = 0; i < playerCount; i++)
            players[i] = GameObject.FindGameObjectWithTag("Player" + (i + 1));
        //InstantiateItems();
        for(int i = 0; i < MapLibrary.mapCount; i++)
        {
            Map m = Instantiate(map);
            m.transform.name = "Map " + (i + 1);
            m.transform.position += new Vector3(i * 50, 0, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
        time += .1f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            selectedMap = (selectedMap + 1) % MapLibrary.mapCount;
            ResetGame();
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
        GameObject.Find("Map "+ (selectedMap+1)).transform.GetComponent<Map>().Reset();
        playersInGame = playerCount;
        for (int i = 0; i < 4; i++)
        {
            players[i].SetActive(i < playerCount);
            players[i].transform.GetComponent<PlayerController>().Reset();
        }
                //GameObject.Find("Player" + (i + 1)).SetActive(true);
    }
}
