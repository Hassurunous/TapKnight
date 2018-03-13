using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour {

	// Store for spawning TapTargets
	public GameObject TapTargetPrefab;

	// Store for Accuracy Indicator prefab
	public GameObject AccuracyIndicatorPrefab;

	// Keep track of the last time we spawned a target
	float lastSpawnTime = 0.0f;

	// Delay between the death of current target and spawn of the next
	float nextSpawnDelay;

	// Position to spawn the next target. 
	Vector3 nextSpawnPosition;

	// What is the current difficulty rating? 
	public float difficultyModifier = 0f;

	// This controls how fast the next target is. It is calculated in update for each target.
	float nextTargetDecaySpeedMod = 0f;

	// Conditional statement in Update() checks time, so we'll store it in order to keep from needing to call it more than once. 
	float currTime;

	// Explicitly track the object where we will be spawning the player's character.
	[SerializeField]
	GameObject playerCharacterObject;

	// This will allow us to track the hitpoints of the playerCharacter. 
	// TODO: Build a method into Character that tracks for death, then just SendMessage on death instead of tracking here in CombatController
	Character playerCharacter;

	// Explicitly track the object where we will be spawning the enemy's character.
	[SerializeField]
	GameObject enemyCharacterObject;

	Character enemyCharacter;

	// Singleton initialization of GameController static
	private static CombatController _instance = null;
	public static CombatController instance {
		get {
			return _instance;
		}
	}

	void Awake()
	{
		//Check if instance already exists
		if (_instance == null) {
			//if not, set instance to this
			_instance = this;
		} else if (_instance != this) {
			Destroy (this.gameObject);
		}
	}


	// We'll start in the "Ready" state, then switch to the "Active" state once combat begins.
	// Right now, combat begins immediately, but later it will begin on a button press or after a delay or something.
	CombatState currState = CombatState.Ready;
		
	// Check the hitpoints of both characters to see if combat is over
	void CheckForCombatEnd() {
		if (playerCharacter.hitpoints <= 0) {
			currState = CombatState.Defeat;
		} else if (enemyCharacter.hitpoints <= 0) {
			currState = CombatState.Victory;
		}
	}

	void DisplayCharacters() {

		Debug.Log ("Spawning: " + CharacterManager.instance.GetCharacterByName ("knight1"));

		GameObject player = Instantiate(CharacterManager.instance.GetCharacterByName("knight1"), playerCharacterObject.transform);

		GameObject enemy = Instantiate(CharacterManager.instance.GetCharacterByName("ork1"), enemyCharacterObject.transform);

		playerCharacter = player.GetComponent<Character> ();
		enemyCharacter = enemy.GetComponent<Character> ();

	}

	void ConfigureCharacters(Character player, Character enemy) {
		playerCharacter = player;
		enemyCharacter = enemy;
	}

	// Currently, the only thing to do is start spawning targets. 
	void Start () {

		currState = CombatState.Active;
		DisplayCharacters ();

		nextSpawnDelay = Random.Range (1f / Mathf.Pow(2f, difficultyModifier), 5f / Mathf.Pow(2f, difficultyModifier));
		nextSpawnPosition = new Vector3 (Random.Range (-6f, 7f), Random.Range (-2.5f, 4.25f), 1f);


	}

	// 
	void Update () {
		if (currState == CombatState.Active) {
			CheckForCombatEnd ();
			currTime = Time.time;
			if (currTime >= lastSpawnTime + nextSpawnDelay) {
				SpawnTarget (currTime);
			}

			#if UNITY_EDITOR && !UNITY_IOS
			if (Input.GetButtonDown ("Fire1")) {
				RaycastHit2D raycastHit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
				if (raycastHit) {
					if (raycastHit.collider.CompareTag ("TapTarget")) {
						TapTarget targetHit = raycastHit.collider.GetComponent<TapTarget> ();
						TargetOutcome (targetHit, true);
					}
				}
			}

			#elif UNITY_IOS
			if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began)) {
				RaycastHit2D raycastHit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
				if (raycastHit) {
					if (raycastHit.collider.CompareTag("TapTarget")) {
						TapTarget targetHit = raycastHit.collider.GetComponent<TapTarget>();
						UpdateScore(targetHit);
					}
				}
			}

			#endif
		}
	}

	public void TargetOutcome(TapTarget target, bool hit) {
		int accuracy = target.Accuracy(hit); // Calculate how accurate the tap was based on the size of the TimerRing
		float destructDelay = target.decaySpeed <= 2 ? target.decaySpeed : 0.5f;

		// Spawn the indicator text that shows the player how accurate they were with their tap.
		GameObject accuracyIndicatorScript = Instantiate(AccuracyIndicatorPrefab, target.transform.position, Quaternion.identity);
		accuracyIndicatorScript.GetComponent<AccuracyIndicator> ().SpawnSetup (accuracy, destructDelay);

		// Then run animations and other functions based on that accuracy.
		if (accuracy >= 95) {
			Debug.Log ("Perfect!");
		} else if (accuracy >= 85) {
			Debug.Log ("Great!");
		} else if (accuracy >= 75) {
			Debug.Log ("Good!");
		} else if (accuracy >= 65) {
			Debug.Log ("Close!");
		} else {
			Debug.Log ("Ouch!");
		}

	}


	void SpawnTarget(float nowtime) {
		// Scaling the decay speed logarithmically. Quick start to the curve, and a trailing increase that stays sane. 
		nextTargetDecaySpeedMod = Mathf.Log(difficultyModifier + 1, 4);

		// Update lastSpawnTime to match the currently spawning target
		lastSpawnTime = nowtime;

		// Get the location to spawn the new target
		nextSpawnPosition = new Vector3 (Random.Range (-6f, 7f), Random.Range (-2.5f, 4f), 1f);

		// Instantiate a new target at the given location and get a reference to its TapTarget component
		TapTarget nextTarget = Instantiate (TapTargetPrefab, nextSpawnPosition, Quaternion.identity).GetComponent<TapTarget>();

		// Set the speed of the new target
		nextTarget.decaySpeed = Random.Range (nextTargetDecaySpeedMod + 0.5f, nextTargetDecaySpeedMod + 1.2f);

		// Set the length of the pause on overlap before destruction.
		nextTarget.delayTime = 0.05f / nextTarget.decaySpeed;

		// As decaySpeed scales upward, the lifetime of a target decreases. The faster the targets, the faster a new one should spawn. 
		nextSpawnDelay = (1f / nextTarget.decaySpeed);
	}
}

enum CombatState {
	Ready,
	Active,
	Victory,
	Defeat
}
