using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

	public Slider MoveSpeed;
	public Slider HitForce;
	public Slider JumpForce;
	public Slider HookDistance;
	public Slider HookReloadTime;
	public Slider HammerReloadTime;
	public Slider ItemProbability;

	public float _MoveSpeedValue;
	public float _HitForceValue;
	public float _JumpForceValue;
	public float _HookDistance;
	public float _HookReloadTimeValue;
	public float _HammerReloadTimeValue;
	public float _ItemProbabilityValue;

	GameManager gameManager;

	void Start() {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager> ();
	}

	void Update() {
		_MoveSpeedValue = MoveSpeed.value;
		_HitForceValue = HitForce.value;
		_JumpForceValue = JumpForce.value;
		_HookDistance = HookDistance.value;
		_HookReloadTimeValue = HookReloadTime.value;
		_HammerReloadTimeValue = HammerReloadTime.value;
		_ItemProbabilityValue = ItemProbability.value;

		gameManager.MoveSpeedValue = 5 + 2 * _MoveSpeedValue ;
		gameManager.HitForceValue = 200 + 50 *_HitForceValue;
		gameManager.JumpForceValue = 200 + 50 * _JumpForceValue;
		gameManager.HookDistance = 8 + 3 * _HookDistance;
		gameManager.HookReloadTimeValue = 3 + _HookReloadTimeValue;
		gameManager.HammerReloadTimeValue = 1.5f + _HammerReloadTimeValue;
		gameManager.ItemProbabilityValue = 40 + 10 * _ItemProbabilityValue;
	}

	public void LoadMainMenu() {
		SceneManager.LoadScene ("MainMenu");
	}
}

