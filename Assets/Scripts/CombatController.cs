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

    // True if it is time to spawn a new target; False if other targets are already being handled.
    bool safeToSpawnTargets = false;

	// What is the current difficulty rating? 
	public float difficultyModifier = 0f;

    // What is the current speed modifier (based on difficulty rating)
    float speedModifier = 1.0f;

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

    // Quick reference to active TapTargets, so we don't need to search for all of them every time.
    List<GameObject> activeTargets = new List<GameObject>();

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

	IEnumerator StartCombat() {
		inGameUI.SetActive (true);
		DisplayCharacters ();

        yield return new WaitForSeconds(3);

        safeToSpawnTargets = true;

        yield break;
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
        difficultyModifier = 0f;
        UpdateSpeedModifier();
		GameController.Score = 0;
		ClearUI ();
		ClearCombatants ();
		StartCoroutine(StartCombat ());
		currState = CombatState.Active;
	}

	void ClearUI() {
		winScreen.SetActive (false);
		loseScreen.SetActive (false);
		readyScreen.SetActive (false);
	}

	void Update () {
		if (currState == CombatState.Active) {
			CheckForCombatEnd ();
			if (safeToSpawnTargets) {
                StartCoroutine(SpawnTargets ());
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
                DestroyAllTapTargets();
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
                DestroyAllTapTargets();
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
		float attackSpeedMod = speedModifier + 1 < 3 ? speedModifier + 1 : 3;

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

        // Remove from activeTargets
        activeTargets.Remove(target.gameObject);
	}


    IEnumerator SpawnTargets(int amount = 1) {

        safeToSpawnTargets = false;

        // Set the lifetime of the new target(s)
        float newLifetime = Random.Range(1.0f / (speedModifier * 2f), 1.0f / speedModifier);
        float nextSpawnDelay = Random.Range(0.1f, 0.75f);

        // Store a list of lifetimes for the targets that we are going to instantiate
        List<float> lifetimes = new List<float>();

        // Store a list of delays to use in the spawning process
        List<float> delays = new List<float>();

        for (int targets = 0; targets < amount; targets++)
        {
            // create `amount` lifetimes and store them so we can calculate and instantiate targets

            // calculate the spawn delay for the next target in the chain and store that in the delays list

            // Get the location to spawn the new target
            Vector3 nextSpawnPosition = new Vector3(Random.Range(-6f, 7f), Random.Range(-2.5f, 3.25f), 1f);

            // Set the layer order of the new target to prevent targets from being double-pressed

            // Instantiate a new target at the given location and get a reference to its TapTarget component
            GameObject newTarget = Instantiate(TapTargetPrefab, nextSpawnPosition, Quaternion.identity);
            activeTargets.Add(newTarget);

            // Assign the lifetime to the new target. Log error if this fails for any reason.
            try
            {
                activeTargets[activeTargets.Count - 1].GetComponent<TapTarget>().TapTargetSetup(newLifetime);
            }
            catch
            {
                Debug.LogError("TapTarget component not found in activeTargets[targets] object.");
            }

            yield return new WaitForSeconds(nextSpawnDelay);
        }

        while (activeTargets.Count > 0)
        {
            yield return null;
        }

        safeToSpawnTargets = true;

        yield break;
	}

    void DestroyAllTapTargets()
    {
        GameObject[] tapTargets = GameObject.FindGameObjectsWithTag("TapTarget");

        for (var i = 0; i < tapTargets.Length; i++)
        {
            Destroy(tapTargets[i]);
        }
    }

    void IncreaseDifficultyLevel() {
		difficultyModifier += 0.1f;
        UpdateSpeedModifier();
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

    void UpdateSpeedModifier()
    {
        // Scaling the speed logarithmically. Quick start to the curve, and a trailing increase that stays sane. 
        speedModifier = Mathf.Log((difficultyModifier + 1.5f), 2.5f);
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
        StartCoroutine(StartCombat());
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
