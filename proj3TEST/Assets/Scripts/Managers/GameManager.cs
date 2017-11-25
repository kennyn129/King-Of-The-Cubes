using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
	public bool gameStarted;

	//Game Playtesting Variables
	public float _MoveSpeedValue;
	public float _HitForceValue;
	public float _JumpForceValue;
	public float _HookDistance;
	public float _HookReloadTimeValue;
	public float _HammerReloadTimeValue;
	public float _ItemProbabilityValue;

	//Map variables
	public int _mapChoice;
	MapManager mapManager;
	public int selectedMap;
	public Map map;



	//Scripts needed for game effects
	public GameObject[] itemPrefabs;
	public GameObject inGameParticlesAndEffects;
	public GameObject mapHolder;
	public Item[] items;
	public static float time;


	//For Initializing players
	public GameObject[] players;
	public GameObject[] spawnPoints;
	int playerCount;
	public int playersInGame;

	void Start() {
		if (gameManager == null)
			gameManager = this;
		gameStarted = false;
		_mapChoice = 1;
		playerCount = players.Length;

		_MoveSpeedValue = 5;
		_HitForceValue = 200;
		_JumpForceValue = 200;
		_HookDistance = 8;
		_HookReloadTimeValue = 3;
		_HammerReloadTimeValue = 1.5f;
		_ItemProbabilityValue = 40;
	}

    // Use this for initialization
    void Awake()
    {
        if (gameManager == null)
            gameManager = this;
		else if (gameManager != this) {
			Destroy (gameObject);
		}
		if (gameStarted) {
			mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
			inGameParticlesAndEffects = GameObject.Find("InGame Particles and Effects");
			mapHolder = GameObject.Find ("Map Holder");
			time = 0;
			Map m = Instantiate (map);
			m.transform.name = "Map " + (_mapChoice + 1);
			m.transform.SetParent (mapHolder.transform);
			Debug.Log ("everything should be set up");
		}






        //InstantiateItems();

        /*for (int i = 0; i < MapLibrary.mapCount; i++)
        {
            Map m = Instantiate(map);
            m.transform.name = "Map " + (i + 1);
            m.transform.position += new Vector3(i * 50, 0, 0);
            //m.gameObject.SetActive(i == selectedMap);
            m.transform.SetParent(mapHolder.transform);
        }*/
//
//        for (int i = 0; i < MapLibrary.bitmaps.Length; i++)
//        {
//            Map m = Instantiate(map);
//            m.transform.name = "Map " + (i + 1);
//            m.transform.position += new Vector3(i * 50, 0, 0);
//            m.transform.SetParent(mapHolder.transform);
//        }
		DontDestroyOnLoad (this);
    }

    // Update is called once per frame
    void Update()
    {
        time += .1f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
			if (_mapChoice < mapHolder.transform.childCount)
            {
				_mapChoice = (_mapChoice + 1) % MapLibrary.bitmaps.Length;
                //selectedMap = (selectedMap + 1) % MapLibrary.mapCount;
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
//            if (playersInGame == 0)
////                print("TIE");
//			else{
//				Debug.Log("one winner");
//
//			}
////            ResetGame();
        }
        // SceneManager.LoadScene("main");
    }

    void ResetGame()
    {
        time = 0;
//        for (int i = 0; i < mapHolder.transform.childCount; i++)
//            mapHolder.transform.GetChild(i).gameObject.SetActive(i == selectedMap);
		mapHolder.transform.GetChild (0).gameObject.SetActive (true);
        foreach (Transform child in inGameParticlesAndEffects.transform)
        {
            Destroy(child.gameObject);
        }
		if (_mapChoice < mapHolder.transform.childCount)
			mapHolder.transform.GetChild(_mapChoice).transform.GetComponent<Map>().Reset();
        playersInGame = playerCount;
        for (int i = 0; i < 4; i++)
        {
            players[i].SetActive(i < playerCount);
            players[i].transform.GetComponent<PlayerController>().Reset();
        }
        //GameObject.Find("Player" + (i + 1)).SetActive(true);
    }


	public float MoveSpeedValue {
		get {return _MoveSpeedValue;}
		set {_MoveSpeedValue = value;}
	}

	public float HitForceValue {
		get {return _HitForceValue;}
		set {_HitForceValue = value;}
	}

	public float JumpForceValue {
		get {return _JumpForceValue;}
		set {_JumpForceValue = value;}
	}

	public float HookDistance {
		get {return _HookDistance;}
		set {_HookDistance = value;}
	}

	public float HookReloadTimeValue {
		get {return _HookReloadTimeValue;}
		set {_HookReloadTimeValue = value;}
	}

	public float HammerReloadTimeValue {
		get {return _HammerReloadTimeValue;}
		set {_HammerReloadTimeValue = value;}
	}

	public float ItemProbabilityValue {
		get {return _ItemProbabilityValue;}
		set {_ItemProbabilityValue = value;}
	}

	public int MapChoice {
		get{ return _mapChoice; }
		set { _mapChoice = value; }
	}
}
