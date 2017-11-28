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
		MoveSpeed.value = gameManager.MoveSpeedValue;
		HitForce.value = gameManager.HitForceValue;
		JumpForce.value = gameManager.JumpForceValue;
		HookDistance.value = gameManager.HookDistance;
		HookReloadTime.value = gameManager.HookReloadTimeValue;
		HammerReloadTime.value = gameManager.HammerReloadTimeValue;
		ItemProbability.value = gameManager.ItemProbabilityValue;



	}

	void Update() {
		_MoveSpeedValue = MoveSpeed.value;
		_HitForceValue = HitForce.value;
		_JumpForceValue = JumpForce.value;
		_HookDistance = HookDistance.value;
		_HookReloadTimeValue = HookReloadTime.value;
		_HammerReloadTimeValue = HammerReloadTime.value;
		_ItemProbabilityValue = ItemProbability.value;

		gameManager.MoveSpeedValue = _MoveSpeedValue ;
		gameManager.HitForceValue = _HitForceValue;
		gameManager.JumpForceValue = _JumpForceValue;
		gameManager.HookDistance = _HookDistance;
		gameManager.HookReloadTimeValue = _HookReloadTimeValue;
		gameManager.HammerReloadTimeValue = _HammerReloadTimeValue;
		gameManager.ItemProbabilityValue = _ItemProbabilityValue;
	}

	public void LoadMainMenu() {
		SceneManager.LoadScene ("MainMenu");
	}
}

