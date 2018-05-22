using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour {

	// Store for spawning TapTargets
	[SerializeField]
	GameObject TapTargetPrefab;

	// Store for Accuracy Indicator prefab
	[SerializeField]
	GameObject AccuracyIndicatorPrefab;

	[SerializeField]
	GameObject loseScreen;

	[SerializeField]
	GameObject winScreen;

	[SerializeField]
	GameObject readyScreen;

	[SerializeField]
	GameObject inGameUI;

	[SerializeField]
	Text inGameScoreText;

	[SerializeField]
	Text afterGameScoreText;

    // TODO: Animation curve could be used to manage the difficulty without resorting to 
    // creating a complex equation by hand. 
    // Using animation curves to control tap target duration
    //[SerializeField]
    //AnimationCurve TapTargetDuration;

	// Keep track of the active target
	TapTarget activeTarget;


	// Keep track of the last time we spawned a target
	float lastSpawnTime = 0.0f;

	// Delay between the death of current target and spawn of the next
	float nextSpawnDelay;

	// Position to spawn the next target. 
	Vector3 nextSpawnPosition;

	// What is the current difficulty rating? 
	public float difficultyModifier = 1f;

	// This controls how fast the next target is. It is calculated in update for each target.
	float activeTargetDecaySpeedMod = 1f;

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

	// We'll start in the "Ready" state, then switch to the "Active" state once the player presses "Ready".
	CombatState currState = CombatState.Ready;

	// For our initial tests, we're going to use an endless mode, then implement the map, upgrades...etc. later in the Progressive game mode.
	GameType gameType = GameType.Endless;

	// Singleton initialization of GameController static
	private static CombatController _Instance = null;
	public static CombatController Instance {
		get {
			return _Instance;
		}
	}

	void Awake()
	{
		//Check if instance already exists
		if (_Instance == null) {
			//if not, set instance to this
			_Instance = this;
		} else if (_Instance != this) {
			Destroy (this.gameObject);
		}
	}
		
	// Check the hitpoints of both characters to see if combat is over
	void CheckForCombatEnd() {
		if (playerCharacter.hitpoints <= 0) {
			currState = CombatState.Defeat;
		} else if (enemyCharacter.hitpoints <= 0) {
			currState = CombatState.Victory;
			GameController.Score += (int)(difficultyModifier * 10);
		}
	}

	void DisplayCharacters() {
//		Debug.Log ("Spawning: " + CharacterManager.instance.GetCharacterByName ("knight1"));
		if (currState == CombatState.Ready) {
			GameObject player = Instantiate (CharacterManager.instance.GetCharacterByName ("knight1"), playerCharacterObject.transform);
			playerCharacter = player.GetComponent<Character> ();
			playerCharacter.Setup ();

			GameObject enemy = Instantiate (CharacterManager.instance.GetCharacterByName ("ork1"), enemyCharacterObject.transform);
			enemyCharacter = enemy.GetComponent<Character> ();
			enemyCharacter.Setup ();
		} else if (currState == CombatState.Active) {
			Destroy (enemyCharacter.gameObject);
			GameObject enemy = Instantiate (CharacterManager.instance.GetCharacterByName ("ork1"), enemyCharacterObject.transform);
			enemyCharacter = enemy.GetComponent<Character> ();
			enemyCharacter.Setup ();
		}


	}

	void Start () {
	}

	void StartCombat() {
		inGameUI.SetActive (true);
		DisplayCharacters ();

		nextSpawnDelay = Random.Range (1f / Mathf.Pow(2f, difficultyModifier), 5f / Mathf.Pow(2f, difficultyModifier));
		nextSpawnPosition = new Vector3 (Random.Range (-6f, 7f), Random.Range (-2.5f, 4.25f), 1f);
	}

	void ClearCombatants() {
		if (playerCharacter) {
			Destroy (playerCharacter.gameObject);
		}
		if (enemyCharacter) {
			Destroy (enemyCharacter.gameObject);
		}
	}

	public void ReadyOrRetry() {
		difficultyModifier = 0.0f;
		GameController.Score = 0;
		ClearUI ();
		ClearCombatants ();
		StartCombat ();
		currState = CombatState.Active;
	}

	void ClearUI() {
		winScreen.SetActive (false);
		loseScreen.SetActive (false);
		readyScreen.SetActive (false);
	}

	// 
	void Update () {
		if (currState == CombatState.Active) {
			CheckForCombatEnd ();
			currTime = Time.time;
			if (currTime >= lastSpawnTime + nextSpawnDelay) {
				SpawnTargets (currTime);
			}

			#if UNITY_EDITOR
			if (Input.GetButtonDown ("Fire1")) {
                print("Fire1 pressed in editor.");
				RaycastHit2D raycastHit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
				if (raycastHit) {
					if (raycastHit.collider.CompareTag ("TapTarget")) {
						TapTarget targetHit = raycastHit.collider.GetComponent<TapTarget> ();
						TargetOutcome (targetHit, true);
					}
				}
			}

			#elif UNITY_IOS || UNITY_ANDROID
			if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began)) {
				RaycastHit2D raycastHit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
				if (raycastHit) {
					if (raycastHit.collider.CompareTag("TapTarget")) {
						TapTarget targetHit = raycastHit.collider.GetComponent<TapTarget>();
						TargetOutcome (targetHit, true);
					}
				}
			}

			#endif
		} else if (gameType == GameType.Endless) {
			if (currState == CombatState.Victory || currState == CombatState.Defeat) {
				if (activeTarget != null) {
					Destroy (activeTarget.gameObject);
				}
				if (currState == CombatState.Victory) {
                    // Hooray! You won! Do things.
                    //					winScreen.SetActive (true);
                    currState = CombatState.Between;
                    StartCoroutine(EnemySpawnDelay());
				} else {
					// Darn! You lost! Do things.
					inGameUI.SetActive(false);
					loseScreen.SetActive (true);
					afterGameScoreText.text = "Score: " + GameController.Score;
					currState = CombatState.Ready;
				}

			}
		} else if (gameType == GameType.Progressive) {
			if (currState == CombatState.Victory || currState == CombatState.Defeat) {
				if (activeTarget != null) {
					Destroy (activeTarget.gameObject);
				}
				if (currState == CombatState.Victory) {
					// Hooray! You won! Do things.
					winScreen.SetActive(true);
				} else {
					// Darn! You lost! Do things.
					loseScreen.SetActive(true);
				}
				currState = CombatState.Ready;
			} 
		} else {
			// We're in the ready state. Wait for input.

		}
	}

	public void TargetOutcome(TapTarget target, bool hit) {
		int accuracy = target.Accuracy(hit); // Calculate how accurate the tap was based on the size of the TimerRing
		float destructDelay = Mathf.Clamp(target.lifetime, 0.5f, 1.5f);

		// Spawn the indicator text that shows the player how accurate they were with their tap.
		GameObject accuracyIndicatorScript = Instantiate(AccuracyIndicatorPrefab, target.transform.position, Quaternion.identity);
		accuracyIndicatorScript.GetComponent<AccuracyIndicator> ().SpawnSetup (accuracy, destructDelay);

		// Get the modifier for calculating animation speed.
		float attackSpeedMod = activeTargetDecaySpeedMod + 1 < 3 ? activeTargetDecaySpeedMod + 1 : 3;

		// Storing damage outside the if statements so we can update the Score after calculating. 
		int damage = 0;

		// Then run animations and other functions based on that accuracy.
		// Player gains a small amount of HP back on perfect presses.
		if (accuracy >= 95) {
			damage = playerCharacter.attackPower * 2;
			enemyCharacter.TakeDamage (false, damage);
			playerCharacter.Attack (attackSpeedMod);
			playerCharacter.GainHP (5);
			damage += 5;
		} else if (accuracy >= 85) {
			damage = (int)playerCharacter.attackPower;
			enemyCharacter.TakeDamage (false, damage);
			playerCharacter.Attack (attackSpeedMod);
		} else if (accuracy >= 75) {
			damage = playerCharacter.attackPower / 2;
			enemyCharacter.TakeDamage (false, damage);
			playerCharacter.Attack (attackSpeedMod);
		} else if (accuracy >= 55) {
            //damage = playerCharacter.attackPower / 2;
            playerCharacter.Attack(attackSpeedMod, true);
            enemyCharacter.Attack(attackSpeedMod);
        } else {
			playerCharacter.TakeDamage (false, enemyCharacter.attackPower);
			enemyCharacter.Attack (attackSpeedMod);
		}


		// Update the score. 
		GameController.Score += damage;
		inGameScoreText.text = "Score: " + GameController.Score;
	}


	void SpawnTargets(float nowtime, int amount=1) {

		// Update lastSpawnTime to match the currently spawning target
		lastSpawnTime = nowtime;

        // Scaling the decay speed logarithmically. Quick start to the curve, and a trailing increase that stays sane. 
        activeTargetDecaySpeedMod = Mathf.Log((difficultyModifier + 1.5f), 2.5f);

        // Get the location to spawn the new target
        nextSpawnPosition = new Vector3 (Random.Range (-6f, 7f), Random.Range (-2.5f, 3.25f), 1f);

		// Instantiate a new target at the given location and get a reference to its TapTarget component
		activeTarget = Instantiate (TapTargetPrefab, nextSpawnPosition, Quaternion.identity).GetComponent<TapTarget>();

        // Set the lifetime of the new target and returns the full lifetime with included delay for target alignment (See: TapTarget.cs for more about delayTime)
        float newLifetime = Random.Range(1.0f / (activeTargetDecaySpeedMod * 2f), 1.0f / activeTargetDecaySpeedMod);
        nextSpawnDelay =  activeTarget.TapTargetSetup(newLifetime);
	}

	void IncreaseDifficultyLevel() {
		difficultyModifier += 0.1f;
	}

	// Increase the power of the enemy based on the difficultyModifier
	void IncreaseEnemyPower(Character enemy) {
		// Scale = 5 * difficultyMod, but we want to make sure it's always at least 1.
		int newAttackPower = (15 * difficultyModifier) > 10 ? (int)(15 * difficultyModifier) : 10;
		enemy.UpdateAttack (newAttackPower);

		// Update the size of the enemy to reflect this increase in power level
		float newSize = 1 + (difficultyModifier / 10f);
		enemy.transform.localScale = new Vector2 (-newSize, newSize);

		// Increase their HP to reflect the new difficulty as well.
		int newHP = (10 * difficultyModifier) > 10 ? (int)(10 * difficultyModifier) : 10;
		enemy.UpdateMaxHP(newHP);
	}

	public void MainMenu() {
		ClearCombatants ();
		GameController.instance.currentGameState = GameState.Menu;
		loseScreen.SetActive (false);
		winScreen.SetActive (false);
		readyScreen.SetActive (true);
	}

    IEnumerator EnemySpawnDelay() {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("EnemySpawnDelay activated.");
        currState = CombatState.Active;
        IncreaseDifficultyLevel();
        StartCombat();
        IncreaseEnemyPower(enemyCharacter);
        yield break;
    }

    // TODO: move the enemy onto the screen and delay the target spawning until they make it to their space.
    IEnumerator EnemyRunIn() {
        yield break;
    }
}

enum CombatState {
	Ready,
	Active,
	Victory,
	Defeat,
    Between
}

enum GameType {
	Endless,
	Progressive
}
