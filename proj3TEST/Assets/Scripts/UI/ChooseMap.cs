using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseMap : MonoBehaviour {

	public void LoadMainMenu () {
		SceneManager.LoadScene ("MainMenu");
	}

	public void LoadMap1 () {
		SceneManager.LoadScene ("Map1");
	}

	public void LoadMap2 () {
		SceneManager.LoadScene ("Map2");
	}

	public void LoadMap3 () {
		SceneManager.LoadScene ("Map3");
	}

	public void LoadMap4 () {
		SceneManager.LoadScene ("Map4");
	}

	public void LoadMap5 () {
		SceneManager.LoadScene ("Map5");
	}

	public void LoadMap6 () {
		SceneManager.LoadScene ("Map6");
	}
}
