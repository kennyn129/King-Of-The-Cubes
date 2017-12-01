using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
	public bool gameStarted = false;

	//Game Playtesting Variables
	static float _MoveSpeedValue;
	static float _HitForceValue;
	static float _JumpForceValue;
	static float _HookDistance;
	static float _HookReloadTimeValue;
	static float _HammerReloadTimeValue;
	static float _ItemProbabilityValue;

	//Map variables
	public int _mapChoice;
	public int selectedMap;
	public Map map;
	bool isGameOver;



	//Scripts needed for game effects
	public GameObject[] itemPrefabs;
	public GameObject inGameParticlesAndEffects;
	public GameObject mapHolder;
	public Item[] items;
	public static float time;
	public Text winText;

	//For Initializing players
	public GameObject[] players;
	public GameObject[] spawnPoints;
	int playerCount;
	public int playersInGame;

//	void Start() {
//		if (gameManager == null)
//			gameManager = this;
//		gameStarted = false;
//		_mapChoice = 1;
//		playerCount = players.Length;
//		_MoveSpeedValue = 5;
//		_HitForceValue = 200;
//		_JumpForceValue = 200;
//		_HookDistance = 8;
//		_HookReloadTimeValue = 3;
//		_HammerReloadTimeValue = 1.5f;
//		_ItemProbabilityValue = 40;
//	}

    // Use this for initialization
    void Awake()
    {
        if (gameManager == null)
            gameManager = this;
		else if (gameManager != this) {
			Destroy (gameObject);
		}



		if (gameManager.gameStarted) {
           // print("??");
		    //gameManager.mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
			isGameOver = false;
			gameManager.winText = GameObject.Find("Canvas").GetComponentInChildren<Text>();
			gameManager.winText.gameObject.SetActive (false);
			gameManager.inGameParticlesAndEffects = GameObject.Find("InGame Particles and Effects");
			gameManager.mapHolder = GameObject.Find ("Map Holder");
			time = 0;
			Map m = Instantiate (map);
			m.transform.name = "Map " + (gameManager._mapChoice + 1);
			m.transform.SetParent (gameManager.mapHolder.transform);
            for (int i = 0; i < 4; i++)
            {
				GameObject player = (GameObject)Instantiate (gameManager.players [i]);
				gameManager.players [i] = player;
				player.transform.name = "Player " + (i + 1);
				PlayerController playerController = player.GetComponent<PlayerController> ();
				playerController.MaxVelocity = 5 + 2 *_MoveSpeedValue;
				playerController.HealthScalar = 200 + 75 *_HitForceValue;
				playerController.JumpForce = 200 + 100 *_JumpForceValue;
				playerController.HookDistance = 8 + 3 * _HookDistance;
				playerController.ReloadHook = 3 + _HookReloadTimeValue;
				playerController.ReloadHammer = 1.5f + _HammerReloadTimeValue;

				gameManager.playersInGame += 1;
				Debug.Log ("playercount = " + gameManager.playersInGame);

            }
            m.SpawnMap(gameManager._mapChoice,4);
			GameObject camHolder = GameObject.Find ("Camera Holder");
			CameraControl cam = camHolder.GetComponentInChildren<CameraControl> ();
			cam.SetUpCamera ();
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

	public void playerDeath() {
		gameManager.playersInGame -= 1;
		if (gameManager.playersInGame < 2 && !isGameOver)
		{
			if (gameManager.playersInGame == 0) {
				gameManager.winText.text = "Tie!";
				gameManager.winText.gameObject.SetActive (true);
			}
			else{
				
				for (int i = 0; i < gameManager.players.Length; i++) {
					Debug.Log (gameManager.players [i]);
					Debug.Log (gameManager.players [i].GetComponent < PlayerController> ().IsAlive);
					if (gameManager.players[i].GetComponent<PlayerController>().IsAlive) {
						gameManager.winText.gameObject.SetActive (true);
						gameManager.winText.text = "Player " + gameManager.players [i].GetComponent<PlayerController> ().playerNum + " Wins!";
						break;
					}
				}
			}
			isGameOver = true;
			StartCoroutine (BackToMainMenu ());
		}	
	}

    // Update is called once per frame
    void Update()
    {
        time += .1f;

    }

	IEnumerator BackToMainMenu() {
		gameManager.isGameOver = false;
		gameManager.gameStarted = false;
		yield return new WaitForSeconds (3);
		SceneManager.LoadScene ("MainMenu");
	}

    void ResetGame()
    {
        time = 0;
        //        for (int i = 0; i < mapHolder.transform.childCount; i++)
        //            mapHolder.transform.GetChild(i).gameObject.SetActive(i == selectedMap);
        //mapHolder.transform.GetChild (0).gameObject.SetActive (true);
        foreach (Transform child in gameManager.inGameParticlesAndEffects.transform)
        {
            Destroy(child.gameObject);
        }
		//if (gameManager._mapChoice < gameManager.mapHolder.transform.childCount)
		gameManager.mapHolder.transform.GetChild(0).transform.GetComponent<Map>().Reset();
        gameManager.playersInGame = gameManager.playerCount;
        for (int i = 0; i < 4; i++)
        {
            gameManager.players[i].SetActive(i < gameManager.playerCount);
            gameManager.players[i].transform.GetComponent<PlayerController>().Reset();
        }
        //GameObject.Find("Player" + (i + 1)).SetActive(true);
    }


	public float MoveSpeedValue {
		get {return GameManager._MoveSpeedValue;}
		set {GameManager._MoveSpeedValue = value;}
	}

	public float HitForceValue {
		get {return GameManager._HitForceValue;}
		set {GameManager._HitForceValue = value;}
	}

	public float JumpForceValue {
		get {return GameManager._JumpForceValue;}
		set {GameManager._JumpForceValue = value;}
	}

	public float HookDistance {
		get {return GameManager._HookDistance;}
		set { GameManager._HookDistance = value;}
	}

	public float HookReloadTimeValue {
		get {return GameManager._HookReloadTimeValue;}
		set { GameManager._HookReloadTimeValue = value;}
	}

	public float HammerReloadTimeValue {
		get {return GameManager._HammerReloadTimeValue;}
		set { GameManager._HammerReloadTimeValue = value;}
	}

	public float ItemProbabilityValue {
		get {return GameManager._ItemProbabilityValue;}
		set { GameManager._ItemProbabilityValue = value;}
	}

	public int MapChoice {
		get{ return gameManager._mapChoice; }
		set { gameManager._mapChoice = value; }
	}
}
