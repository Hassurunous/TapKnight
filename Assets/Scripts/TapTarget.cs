using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTarget : MonoBehaviour {

	// How fast will this target decay?
	public float decaySpeed = 5.0f;

	// How big is the outer ring (TimerRing) currently, compared to the size of the Target Ring
	float timerRingScale = 3.0f;

	// Reference for the TimerRing
	public GameObject TimerRing;

	// Reference for the TargetRing
	public GameObject TargetRing;

	// Interpolation parameter tells the percentage of the transformation that has taken place in the Lerp function
	float tParam = 0f;

	// We want to delay slightly after the rings align to destroy the ring to give users a bit of a chance to get the 100% click
	bool aligned = false;
	float alignTime = 0.0f;
	float delayTime;

	void Update()
	{
		// Scale the TimerRing down over time
		if (tParam < 1) {
			tParam += Time.deltaTime * decaySpeed; //This will increment tParam based on Time.deltaTime multiplied by a speed multiplier
			timerRingScale = Mathf.Lerp (3f, 1f, tParam);
			TimerRing.transform.localScale = new Vector3 (timerRingScale, timerRingScale, 1f);
		} else if (aligned == false) {
			alignTime = Time.time;
			aligned = true;
		} else if (aligned && Time.time >= alignTime + delayTime) {
			GameController.TargetOutcome(this, false);
		}

	}

	// Use this for initialization
	void Start () {
		delayTime = 0.1f / decaySpeed;
	}

	void Awake() {
	}

	public int Accuracy(bool hit) {
		Kill ();
		if (hit) {
			return (int) (100.0f / Mathf.Pow (2, timerRingScale - 1));
		} else {
			return 0;
		}
	}

	void Kill() {
		Destroy (this.gameObject);
	}
}
