using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	// Store for spawning TapTargets
	public GameObject TapTargetPrefab;

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

	int Score = 0;

	// Currently, the only thing to do is start spawning targets. 
	void Start () {
		nextSpawnDelay = Random.Range (1f / Mathf.Pow(2f, difficultyModifier), 5f / Mathf.Pow(2f, difficultyModifier));
		nextSpawnPosition = new Vector3 (Random.Range (-6f, 7f), Random.Range (-2.5f, 4.25f), 1f);
	}
	
	// 
	void Update () {
		currTime = Time.time;
		if (currTime >= lastSpawnTime + nextSpawnDelay) {
			SpawnTarget (currTime);
		}

		#if UNITY_EDITOR && !UNITY_IOS
		if (Input.GetButtonDown("Fire1")) {
			RaycastHit2D raycastHit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (raycastHit) {
				if (raycastHit.collider.CompareTag("TapTarget")) {
					TapTarget targetHit = raycastHit.collider.GetComponent<TapTarget>();
					TargetOutcome(targetHit, true);
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

	public static void TargetOutcome(TapTarget target, bool hit) {
		int accuracy = target.Accuracy(hit); // Calculate how accurate the tap was based on the size of the TimerRing
		// Then run animations and other functions based on that accuracy.
		if (accuracy >= 98) {
			Debug.Log ("Perfect!");
		} else if (accuracy >= 85) {
			Debug.Log ("Great!");
		} else if (accuracy >= 70) {
			Debug.Log ("Good!");
		} else if (accuracy >= 50) {
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

		// As decaySpeed scales upward, the lifetime of a target decreases. The faster the targets, the faster a new one should spawn. 
		nextSpawnDelay = (1f / nextTarget.decaySpeed);
	}
}
