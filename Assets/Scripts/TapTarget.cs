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
//		print (tParam);
		// Scale the TimerRing down over time
		if (tParam < 1) {
			tParam += Time.deltaTime * decaySpeed; //This will increment tParam based on Time.deltaTime multiplied by a speed multiplier
			timerRingScale = Mathf.Lerp (3f, 1f, tParam);
			TimerRing.transform.localScale = new Vector3 (timerRingScale, timerRingScale, 1f);
		} else if (aligned == false) {
//			Debug.Log( "Last chance!");
			alignTime = Time.time;
//			Debug.Log (alignTime);
			aligned = true;
		} else if (aligned && Time.time >= alignTime + delayTime) {
//			Debug.Log ("Too slow!");
			Destroy (this.gameObject);
		}

		#if UNITY_EDITOR && !UNITY_IOS
		if (Input.GetButtonDown("Fire1")) {
//			Debug.Log ("Touch detected.");
			RaycastHit2D raycastHit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (raycastHit) {
				if (raycastHit.collider.CompareTag("TapTarget")) {
					Debug.Log("TapTarget clicked");
				}
			}
		}

		#elif UNITY_IOS
		if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began)) {
//			Debug.Log ("Touch detected.");
			RaycastHit2D raycastHit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
			if (raycastHit) {
				if (raycastHit.collider.CompareTag("TapTarget")) {
//					Debug.Log("TapTarget clicked");
				}
			}
		}

		#endif

	}

	// Use this for initialization
	void Start () {
		delayTime = 0.1f / decaySpeed;
//		print ("DelayTime = " + delayTime);
	}
}
