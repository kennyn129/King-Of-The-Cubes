using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour {

	public float hookSpeed;
	public float maxDistance;
	public float _playerNum;
	public GameObject hookItemPrefab;

	LineRenderer rope;
	Rigidbody hookRB;
	LayerMask playerLayer;
	PlayerController hookOwner;

	Vector3 pullForce;
	Vector3 origin;
	Vector3 destination;
	Vector3 currPos;


	void Awake() {
		rope = GetComponent<LineRenderer> ();
		hookRB = GetComponent<Rigidbody> ();
		origin = transform.position;
		playerLayer = LayerMask.NameToLayer ("Player");


	}



	void Update() {
		rope.SetPosition (0, origin);
		rope.SetPosition (1, transform.position);
		currPos = origin - transform.position;
		if (Mathf.Abs (currPos.magnitude) > maxDistance) {
			hookRB.velocity = Vector3.zero;
			GameObject hookItem = (GameObject)Instantiate(hookItemPrefab, transform.position, transform.rotation);
			hookItem.GetComponent<HookItem> ().playerNum = _playerNum;
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.layer != playerLayer) {
			return;
		}

		PlayerController playerScript = coll.gameObject.GetComponent<PlayerController> ();
		if (playerScript == null) {
			return;
		}
		if (playerScript.playerNum == _playerNum) {
			hookOwner = playerScript;
			return;
		}


		destination = coll.gameObject.GetComponent<Rigidbody> ().position;
		pullForce = origin - destination;
		pullForce = pullForce.normalized;
		playerScript.hookSomeone (pullForce);
		hookRB.velocity = Vector3.zero;
		hookOwner.hasHook = true;
		Destroy (gameObject);
	}


	public float playerNum {
		get { return _playerNum; }
		set { _playerNum = value; }
	}
}
