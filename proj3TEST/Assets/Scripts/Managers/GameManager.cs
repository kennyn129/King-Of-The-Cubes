﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		Debug.Log ("Movespeed = " + _MoveSpeedValue);
		Debug.Log ("HitForce = " + _HitForceValue);
		Debug.Log ("JumpForce = " + _JumpForceValue);
		Debug.Log ("HookDistance = " + _HookDistance);
		Debug.Log ("HookReloadTime = " + _HookReloadTimeValue);
		Debug.Log ("HammerReloadTime = " + _HammerReloadTimeValue);



		if (gameManager.gameStarted) {
           // print("??");
		    gameManager.mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
			gameManager.inGameParticlesAndEffects = GameObject.Find("InGame Particles and Effects");
			gameManager.mapHolder = GameObject.Find ("Map Holder");
			time = 0;
			Map m = Instantiate (map);
			m.transform.name = "Map " + (gameManager._mapChoice + 1);
			m.transform.SetParent (gameManager.mapHolder.transform);
            for (int i = 0; i < 4; i++)
            {
				GameObject player = (GameObject)Instantiate (gameManager.players [i]);
				player.transform.name = "Player " + (i + 1);
				PlayerController playerController = player.GetComponent<PlayerController> ();
				playerController.MaxVelocity = 5 + 2 *_MoveSpeedValue;
				playerController.HealthScalar = 200 + 75 *_HitForceValue;
				playerController.JumpForce = 200 + 100 *_JumpForceValue;
				playerController.HookDistance = 8 + 3 * _HookDistance;
				playerController.ReloadHook = 3 + _HookReloadTimeValue;
				playerController.ReloadHammer = 1.5f + _HammerReloadTimeValue;
				Debug.Log ("PlayerController = " + playerController);
				Debug.Log("MaxVelocity = " + playerController.MaxVelocity);
				Debug.Log("MaxVelocity should be " + _MoveSpeedValue);
				Debug.Log("HealthScalar = " + playerController.HealthScalar);
				Debug.Log("JumpForce = " + playerController.JumpForce);



            }
            m.SpawnMap(gameManager._mapChoice,4);
			GameObject camHolder = GameObject.Find ("Camera Holder");
			Debug.Log (camHolder);
			CameraControl cam = camHolder.GetComponentInChildren<CameraControl> ();
			cam.SetUpCamera ();
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
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//			if (gameManager._mapChoice < gameManager.mapHolder.transform.childCount)
//            {
//				gameManager._mapChoice = (gameManager._mapChoice + 1) % MapLibrary.bitmaps.Length;
//                //selectedMap = (selectedMap + 1) % MapLibrary.mapCount;
//                //ResetGame();
//                //mapHolder.transform.GetChild(selectedMap).gameObject.SetActive(false);
//
//                //mapHolder.transform.GetChild(selectedMap).gameObject.SetActive(true);
//                gameManager.ResetGame();
//            }
//            //SceneManager.LoadScene("main");
//        }
//        if (Input.GetKeyDown(KeyCode.E))
//            gameManager.ResetGame();
//        if (Input.GetKeyDown(KeyCode.Backspace))
//        {
//            gameManager.playerCount = (gameManager.playerCount + 1) % 5;
//            if (gameManager.playerCount < 2)
//                gameManager.playerCount = 2;
//            gameManager.playersInGame = gameManager.playerCount;
//            gameManager.ResetGame();
//        }
        if (gameManager.playersInGame < 2)
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
