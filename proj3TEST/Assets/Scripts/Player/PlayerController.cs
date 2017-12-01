using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	// play testing variables
	public float maxVelocity;
	public float slowVelocity;
	public float turnSpeed;
	public float jumpForce;
	public int playerNum;
	public float reloadHammer;
	public float reloadHook;
	public float timeToPlaceHook;
	public float hookDistance;
	public float hookSpeed;
	public float healthScalar;
	public float forceY;

	public AudioSource audioSource;
	public AudioClip hammerSound;
	public AudioClip hookSound;
	public AudioClip hitSound;
	public AudioClip deadSound;


	//Unity Components
	public GameObject hookPrefab;
	public GameObject hookItemPrefab;

	Text percentText;
	int currPercent;
	Animator anim;
	GameObject player;
	Rigidbody playerRB;
	CapsuleCollider playerCollider;
	CameraControl cam;

	// Holds player's useable items
	public List<Item> items;
	bool reset;

	//private variables and internal timers
	public bool isAlive;
	bool canMove;
	bool isDisabled;
	bool isGrounded;
	bool _hasHook;
	bool hasPlacedHook;
	float currVelocity;
	float hitDistance;
	float moveHorizontal;
	float moveVertical;
	float hammerTime;
	float hookTime;
	float placeHookTime;
	Vector3 movement;
	Direction currDirection;
	Vector3 currForceDirection;
	LayerMask playerLayer;
	LayerMask floorLayer;
	RaycastHit floorHit;
	Transform hitOrigin;

	// Input Control Strings
	string horizontalAxis;
	string verticalAxis;
	string hammerControl;
	string hookControl;
	string breakGroundControl;
	string jumpControl;
	string itemControl;

	// Tile focus
	Tile focusedTile;

	// Player Movement
	float moveDebuff;

	// Player team
	public int team;

	enum Direction
	{
		North,
		South,
		East,
		West,
		NorthWest,
		SouthWest,
		NorthEast,
		SouthEast,
	}

	public void Reset()
	{
		items.Clear();
		if(focusedTile)
			focusedTile.Reset();
		focusedTile = null;
		hammerTime = reloadHammer;
		hookTime = reloadHook;
		isDisabled = false;
		canMove = true;
		reset = true;
		hasHook = true;
	}

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player" + playerNum.ToString());
		playerRB = player.GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		playerCollider = GetComponent<CapsuleCollider>();
		playerLayer = LayerMask.GetMask("Player");
		floorLayer = LayerMask.GetMask("Floor");
		hitOrigin = transform.GetChild (1);
		hitOrigin.localPosition = new Vector3 (0, 3.5f, 0);
		items = new List<Item>();
		GameObject camHolder = GameObject.Find ("Camera Holder");
		cam = camHolder.GetComponentInChildren<CameraControl> ();
		audioSource = GetComponent<AudioSource> ();
//		currPercent = 0;
//		percentText.text = currPercent.ToString();

		isAlive = true;
		reset = false;
		isDisabled = false;
		hammerTime = 0;
		hookTime = 0;
		_hasHook = true;
		hasPlacedHook = false;
		placeHookTime = 0;
		canMove = true;
		currVelocity = maxVelocity;
		focusedTile = null;
		moveDebuff = 1;
		team = playerNum;
		/*horizontalAxis = "XboxHorizontal" + playerNum.ToString();
		verticalAxis = "XboxVertical" + playerNum.ToString();
		hammerControl = "XboxX" + playerNum.ToString();
		hookControl = "XboxY" + playerNum.ToString();
		breakGroundControl = "XboxB" + playerNum.ToString();
		jumpControl = "XboxA" + playerNum.ToString();
		itemControl = "XboxRB" + playerNum.ToString();*/

		horizontalAxis = "Horizontal" + playerNum.ToString();
		verticalAxis = "Vertical" + playerNum.ToString();
		hammerControl = "Hammer" + playerNum.ToString();
		hookControl = "Hook" + playerNum.ToString();
		breakGroundControl = "BreakGround" + playerNum.ToString();
		jumpControl = "Jump" + playerNum.ToString ();
		itemControl = "ItemControl" + playerNum.ToString ();

		//items = new Item[2];


	}

	public Vector3 GetDirection()
	{
		return currForceDirection.normalized;
	}

	public void SetMoveDebuff(float f)
	{
		moveDebuff = f;
	}

	void isGroundedCheck () {
		if (Physics.Raycast (hitOrigin.position + new Vector3(.05f, 0, .05f), -Vector3.up, 1.45f, floorLayer)
			|| Physics.Raycast (hitOrigin.position + new Vector3(-.05f, 0 ,.05f), -Vector3.up, 1.45f, floorLayer)
			|| Physics.Raycast (hitOrigin.position + new Vector3(-.05f , 0, -.05f), -Vector3.up, 1.45f, floorLayer)
			|| Physics.Raycast (hitOrigin.position + new Vector3(.05f, 0, -.05f), -Vector3.up, 1.45f, floorLayer)) {
			isGrounded = true;
		} else {
			isGrounded = false;
		}
	}
	void Update()
	{
		if (reset)
		{
			reset = false;
			Rigidbody rb = transform.GetComponent<Rigidbody>();
			rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
			transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
		else
		{

			Rigidbody rb = transform.GetComponent<Rigidbody>();
			rb.constraints = ~RigidbodyConstraints.FreezePositionZ & ~RigidbodyConstraints.FreezePositionX & ~RigidbodyConstraints.FreezePositionY;

		}
		// Change direction of player to help determine which tile is focused
		UseTheForce();
		floorHit = new RaycastHit();
		// If tile is in range, highlight
		if (Physics.Raycast(hitOrigin.position + currForceDirection.normalized * 2.25f, -Vector3.up, out floorHit, 2f, floorLayer))
		{
			Tile tileObj = floorHit.collider.gameObject.GetComponent<Tile>();
			Transform t = floorHit.collider.gameObject.transform;
			if (tileObj != null)
			{
				if (t.gameObject != focusedTile) {
					if (focusedTile != null)
						focusedTile.FocusOnTile(playerNum-1,false);

					focusedTile = tileObj;
					focusedTile.FocusOnTile(playerNum-1,true);

				}
			}
		}
		// Else un-highlight
		else
		{
			if (focusedTile != null)
			{
				focusedTile.FocusOnTile(playerNum-1, false);
				focusedTile = null;
			}
		}



		if (transform.position.y <= -5)
		{
			audioSource.Stop();
			audioSource.clip = deadSound;
			audioSource.Play ();
			isAlive = false;
		}
		if (transform.position.y <=-30) {

//			GameManager.gameManager.playersInGame--;
			GameManager.gameManager.playerDeath ();
			Destroy (this);
		}

		hammerTime += Time.deltaTime;
		hookTime += Time.deltaTime;

		isGroundedCheck();
		moveHorizontal = Input.GetAxis(horizontalAxis);
        moveVertical = Input.GetAxis(verticalAxis);

//		moveHorizontal = Input.GetAxisRaw(horizontalAxis);
//		moveVertical = Input.GetAxisRaw(verticalAxis);
		if (isDisabled)
		{
			return;
		}
		else
		{

			if (Input.GetButtonDown(hammerControl) && hammerTime >= reloadHammer)
			{
				useHammer(moveHorizontal, moveVertical);
			}
			else if (Input.GetButtonDown(breakGroundControl) && hammerTime >= reloadHammer)
			{
				breakGround();
			}
			else if (placeHookTime >= timeToPlaceHook && !hasPlacedHook && _hasHook) //if button has held down long enough and isn't placed but has hook
			{
				Debug.Log ("Calling placeHOok();");
				placeHook (); //still need to implement
				hasPlacedHook = true;
				currVelocity = maxVelocity; 
			}
			else if (Input.GetButtonDown(hookControl) && !hasPlacedHook && _hasHook) //if button has just started being pressed
			{
				Debug.Log ("just started pressing hookButton");
				hasPlacedHook = false;
				placeHookTime = 0;
				currVelocity = slowVelocity;
			}
			else if (Input.GetButton(hookControl) && !hasPlacedHook && _hasHook) //if currently charging 
			{
				//Debug.Log ("currently Charging hook");
				placeHookTime += Time.deltaTime;
				currVelocity = slowVelocity;
			}
			else if (Input.GetButtonUp(hookControl) && !hasPlacedHook && _hasHook) // if lets go before the timer gets to placeHOokTimer;
			{  
				Debug.Log ("button was let go before timer reached placeHookTimer");
				//StartCoroutine (useHook ());
				useHook (moveHorizontal, moveVertical);
				currVelocity = maxVelocity;
			} else if (Input.GetButtonDown(jumpControl) && isGrounded) {
				Debug.Log ("Jump Key Pressed");
				jump ();
			}

            if(playerNum == 1 && Input.GetKeyDown(KeyCode.P))
            {
                if(items.Count > 0)
                {
                    items[0].Use(this);
                }
            }
			if (isGrounded && canMove) {
				move (moveHorizontal, moveVertical);
				rotateGround (currDirection);
			} 
			else if(!isGrounded) {
				rotateAir (moveHorizontal, moveVertical);
				moveAir (moveHorizontal, moveVertical);
			}
		}

	}

	void jump() {
		playerRB.AddForce (new Vector3 (0, jumpForce, 0));
	}

	void placeHook() {
		Debug.Log ("Just Placed Hook");
		GameObject hookItem = (GameObject)Instantiate (hookItemPrefab, hitOrigin.position, transform.rotation);
		hookItem.GetComponent<HookItem> ().playerNum = playerNum;
		_hasHook = false;
	}

	public void pickupHook() {
		Debug.Log ("Just Picked Up Hook");
		_hasHook = true;
		hasPlacedHook = false;
		placeHookTime = 0;
	}


	void useHammer(float hor, float ver)
	{
		anim.SetTrigger ("HammerHit");
		audioSource.Stop();
		audioSource.clip = hammerSound;
		audioSource.Play ();

		hammerTime = 0;
		UseTheForce();
		Vector3 shootOut;
		if (hor == 0 && ver == 0) {
			shootOut = currForceDirection;
		} else {
			shootOut = new Vector3 (hor, 0, ver);
		}
			
		Collider[] colls = Physics.OverlapSphere(hitOrigin.position + shootOut.normalized * 1.5f, 1f, playerLayer);
		Debug.Log(colls);
		foreach (Collider x in colls)
		{
			PlayerController playerScript = x.gameObject.GetComponent<PlayerController>();
			if (playerScript == null)
			{
				continue;
			}
			if (playerScript.playerNum == playerNum)
			{
				continue;
			}
			playerScript.hammerSomeone(currForceDirection);
		}
	}

	void breakGround()
	{
		hammerTime = 0;
		anim.SetTrigger ("HammerGround");
		UseTheForce();
		floorHit = new RaycastHit();
		if (Physics.Raycast(hitOrigin.position + currForceDirection.normalized * 2.25f, -Vector3.up, out floorHit, 2f, floorLayer))
		{
			Debug.Log(floorHit.collider);
			Tile tileObj = floorHit.collider.gameObject.GetComponent<Tile>();
			if (tileObj != null)
			{
				tileObj.Break();
			}
		}
	}

	public void useHook(float hor, float ver) {
		//		isDisabled = true;
		//		canMove = false;
		hasHook = false;

		audioSource.Stop();
		audioSource.clip = hookSound;
		audioSource.Play ();


		anim.SetTrigger ("Hook");
		UseTheForce();
		// Instantiate hook prefab with a certain velocity
		Vector3 shootOut;
		if (hor == 0 && ver == 0) {
			shootOut = currForceDirection;
		} else {
			shootOut = new Vector3 (hor, 0, ver);
		}

		GameObject hook = (GameObject)Instantiate(hookPrefab, hitOrigin.position, transform.rotation);
		hook.GetComponent<HookScript> ()._playerNum = playerNum;
		hook.GetComponent<HookScript> ().maxDistance = hookDistance;
		hook.GetComponent<Rigidbody>().velocity = shootOut.normalized * hookSpeed;


		//		yield return new WaitForSeconds(.7f);
		hookTime = 0;
		canMove = true;
		isDisabled = false;
		Debug.Log ("Can Move = " + canMove);
		Debug.Log ("isDisabled = " + isDisabled);
		Debug.Log ("Max Velocity = " + maxVelocity);

	}
		

	public void hammerSomeone(Vector3 direction)
	{
		StartCoroutine(getHammered(direction));
	}
	public void hookSomeone(Vector3 direction)
	{
		StartCoroutine(getHooked(direction));
	}

	public IEnumerator getHammered(Vector3 direction)
	{

		audioSource.Stop();
		audioSource.clip = hitSound;
		audioSource.Play ();

		currPercent += 5;
		percentText.text = currPercent.ToString();
		isDisabled = true;
		canMove = false;
		playerRB.velocity = Vector3.zero;
		direction = direction.normalized;
		Vector3 forceVector = new Vector3(direction.x * healthScalar, forceY, direction.z * healthScalar);
		Debug.Log(forceVector);

		healthScalar += 5f;

		playerRB.AddForce(forceVector);
		yield return new WaitForSeconds(.1f);
		isDisabled = false;
		canMove = true;
	}

	public IEnumerator getHooked(Vector3 direction)
	{
		audioSource.Stop();
		audioSource.clip = hitSound;
		audioSource.Play ();

		currPercent += 5;
		percentText.text = currPercent.ToString();
		Debug.Log("In GetHooked");
		Debug.Log(direction);
		isDisabled = true;
		Vector3 forceVector = direction * 500;

		playerRB.AddForce(forceVector);
		yield return new WaitForSeconds(.8f);
		isDisabled = false;
	}

	void moveAir(float hor, float ver) {
		Vector3 AirTilt = new Vector3 (hor, 0, ver);
		playerRB.AddForce (AirTilt * 15);
		if (playerRB.velocity.x > maxVelocity) {
			playerRB.velocity = new Vector3 (maxVelocity, playerRB.velocity.y, playerRB.velocity.z);
		} else if (playerRB.velocity.x < -maxVelocity) {
			playerRB.velocity = new Vector3 (-maxVelocity, playerRB.velocity.y, playerRB.velocity.z);
		}

		if (playerRB.velocity.z > maxVelocity) {
			playerRB.velocity = new Vector3 (playerRB.velocity.x, playerRB.velocity.y, maxVelocity);
		} else if (playerRB.velocity.z < -maxVelocity) {
			playerRB.velocity = new Vector3 (playerRB.velocity.x, playerRB.velocity.y, -maxVelocity);
		}

	}

	void move(float hor, float ver)
	{
		//add movement to player
		movement = new Vector3(hor, 0f, ver);
		//movement = movement.normalized * currVelocity * moveDebuff;
		movement = movement * currVelocity * moveDebuff;

		playerRB.velocity = movement;
		if (hor != 0 || ver != 0) {
			anim.SetBool ("Running", true);
		} else {
			anim.SetBool ("Running", false);
		}



		if (hor > 0 && ver > 0) {
			currDirection = Direction.NorthEast;
		} else if (hor > 0 && ver == 0) {
			currDirection = Direction.East;
		} else if (hor > 0 && ver < 0) {
			currDirection = Direction.SouthEast;
		} else if (hor == 0 && ver < 0) {
			currDirection = Direction.South;
		} else if (hor < 0 && ver < 0) {
			currDirection = Direction.SouthWest;
		} else if (hor < 0 && ver == 0) {
			currDirection = Direction.West;
		} else if (hor < 0 && ver > 0) {
			currDirection = Direction.NorthWest;
		} else if (hor == 0 && ver > 0) {
			currDirection = Direction.North;
		}   
	}

	void rotateGround(Direction dir) {
		Quaternion rotation;

		switch (dir)
		{
		case Direction.North:
			rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
			transform.rotation = Quaternion.Lerp (transform.rotation, rotation, turnSpeed * Time.deltaTime);
			return;
		case Direction.NorthEast:
			rotation = Quaternion.Euler (new Vector3 (0, 45, 0));
			transform.rotation = Quaternion.Lerp (transform.rotation, rotation, turnSpeed * Time.deltaTime);
			return;
		case Direction.East:
			rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
			transform.rotation = Quaternion.Lerp (transform.rotation, rotation, turnSpeed * Time.deltaTime);
			return;
		case Direction.SouthEast:
			rotation = Quaternion.Euler (new Vector3 (0, 135, 0));
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation,turnSpeed * Time.deltaTime);
			return;
		case Direction.South:
			rotation = Quaternion.Euler (new Vector3 (0, 180, 0));
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
			return;
		case Direction.SouthWest:
			rotation = Quaternion.Euler (new Vector3 (0, 225, 0));
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
			return;
		case Direction.West:
			rotation = Quaternion.Euler (new Vector3 (0, 270, 0));
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
			return;
		case Direction.NorthWest:
			rotation = Quaternion.Euler (new Vector3 (0, 315, 0));
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
			return;
		default:
			break;
		}
	}


	void rotateAir(float hor, float ver) {
		Quaternion rotation;

		if (hor > 0 && ver > 0) {
			currDirection = Direction.NorthEast;
			rotation = Quaternion.Euler (new Vector3 (0, 45, 0));
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
		} else if (hor > 0 && ver == 0) {
			currDirection = Direction.East;
			rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
			transform.rotation = Quaternion.Lerp (transform.rotation, rotation, turnSpeed * Time.deltaTime);
		} else if (hor > 0 && ver < 0) {
			currDirection = Direction.SouthEast;
			rotation = Quaternion.Euler (new Vector3 (0, 135, 0));
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation,turnSpeed * Time.deltaTime);
		} else if (hor == 0 && ver < 0) {
			currDirection = Direction.South;
			rotation = Quaternion.Euler (new Vector3 (0, 180, 0));
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
		} else if (hor < 0 && ver < 0) {
			currDirection = Direction.SouthWest;
			rotation = Quaternion.Euler (new Vector3 (0, 225, 0));
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
		} else if (hor < 0 && ver == 0) {
			currDirection = Direction.West;
			rotation = Quaternion.Euler (new Vector3 (0, 270, 0));
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
		} else if (hor < 0 && ver > 0) {
			currDirection = Direction.NorthWest;
			rotation = Quaternion.Euler (new Vector3 (0, 315, 0));
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
		} else if (hor == 0 && ver > 0) {
			currDirection = Direction.North;
			rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
			transform.rotation = Quaternion.Lerp (transform.rotation, rotation, turnSpeed * Time.deltaTime);
		} 



	}

	void UseTheForce()
	{
		switch (currDirection)
		{
		case Direction.North:
			currForceDirection = new Vector3(0, 0, 1);
			return;
		case Direction.NorthEast:
			currForceDirection = new Vector3(1, 0, 1);
			return;
		case Direction.East:
			currForceDirection = new Vector3(1, 0, 0);
			return;
		case Direction.SouthEast:
			currForceDirection = new Vector3(1, 0, -1);
			return;
		case Direction.South:
			currForceDirection = new Vector3(0, 0, -1);
			return;
		case Direction.SouthWest:
			currForceDirection = new Vector3(-1, 0, -1);
			return;
		case Direction.West:
			currForceDirection = new Vector3(-1, 0, 0);
			return;
		case Direction.NorthWest:
			currForceDirection = new Vector3(-1, 0, 1);
			return;
		default:
			break;
		}
	}

	public bool hasHook {
		get { return _hasHook; }
		set { _hasHook = value; }
	}

	public float MaxVelocity {
		get { return maxVelocity; }
		set { maxVelocity = value; }
	}

	public float HealthScalar {
		get { return healthScalar; }
		set { healthScalar = value; }
	}

	public float JumpForce {
		get { return forceY; }
		set { forceY = value; }
	}

	public float HookDistance {
		get { return hookDistance; }
		set { hookDistance = value; }
	}

	public float ReloadHook {
		get { return reloadHook; }
		set { reloadHook = value; }
	}

	public float ReloadHammer {
		get { return reloadHammer; }
		set { reloadHammer = value; }
	}

	public bool IsAlive {
		get { return isAlive; }
	}

}