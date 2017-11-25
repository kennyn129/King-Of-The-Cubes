using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseMap : MonoBehaviour {

	GameManager gameManager;

	void Awake() {
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

	public void LoadMainMenu () {
		SceneManager.LoadScene ("MainMenu");
	}

	public void LoadMap1 () {
		gameManager.MapChoice = 0;
        
		loadGameScene ();
	}

	public void LoadMap2 () {
		gameManager.MapChoice = 01;
		loadGameScene ();
	}

	public void LoadMap3 () {
		gameManager.MapChoice = 02;
		loadGameScene ();
	}

	public void LoadMap4 () {
		gameManager.MapChoice = 03;
		loadGameScene ();
	}

	public void LoadMap5 () {
		gameManager.MapChoice = 04;
		loadGameScene ();
	}

	public void LoadMap6 () {
		gameManager.MapChoice = 05;
		loadGameScene ();
	}

	void loadGameScene() {
		//GameManager.gameManager.gameStarted = true;

        gameManager.gameStarted = true;
        SceneManager.LoadScene ("Main");
	}
}
