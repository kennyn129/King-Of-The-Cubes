using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookItem : MonoBehaviour {

	public float _playerNum;

	int playerLayer;
	PlayerController playerScript;
	CapsuleCollider hookCollider;


	void Awake() {
		playerLayer = LayerMask.NameToLayer ("Player");
		hookCollider = GetComponent<CapsuleCollider> ();
		hookCollider.enabled = false;
		StartCoroutine (getStarted ());
	}

	IEnumerator getStarted() {
		yield return new WaitForSeconds (3);
		hookCollider.enabled = true;
	}


	void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.layer != playerLayer) {
			return;
		}

		playerScript = coll.gameObject.GetComponent<PlayerController> ();
		if (playerScript == null) {
			return;
		}

		if (playerScript.playerNum == _playerNum) {
			playerScript.pickupHook ();

			Destroy (gameObject);
		}
	}

	public float playerNum {
		get { return _playerNum; }
		set { _playerNum = value; }
	}
}
