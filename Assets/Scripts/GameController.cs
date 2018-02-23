using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject TapTargetPrefab;

	float lastSpawnTime = 0.0f;

	float nextSpawnDelay;

	Vector3 nextSpawnPosition;

	// Use this for initialization
	void Start () {
		nextSpawnDelay = Random.Range (1f, 5f);
		nextSpawnPosition = new Vector3 (Random.Range (-6f, 7f), Random.Range (-2.5f, 4.25f), 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= lastSpawnTime + nextSpawnDelay) {
			lastSpawnTime = Time.time;

			TapTarget nextTarget = Instantiate (TapTargetPrefab, nextSpawnPosition, Quaternion.identity).GetComponent<TapTarget>();
			nextTarget.decaySpeed = Random.Range (0.2f, 2f);
			nextSpawnPosition = new Vector3 (Random.Range (-6f, 7f), Random.Range (-2.5f, 4f), 1f);
		}
	}
}
