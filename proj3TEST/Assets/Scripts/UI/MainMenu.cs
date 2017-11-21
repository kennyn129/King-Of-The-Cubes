using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void LoadPlay() {
		SceneManager.LoadScene ("ChooseMap");
	}

	public void LoadControls() {
		SceneManager.LoadScene ("Controls");
	}

	public void LoadSettings() {
		SceneManager.LoadScene ("Settings");
	}

	public void LoadCredits() {
		SceneManager.LoadScene ("Credits");
	}
}
