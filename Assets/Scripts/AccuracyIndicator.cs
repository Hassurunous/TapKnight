using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyIndicator : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public Sprite[] accuracySprites;

	public void SpawnSetup(int accuracy, float delay) {
		if (accuracy >= 95) {
			spriteRenderer.sprite = accuracySprites [0];
		} else if (accuracy >= 85) {
			spriteRenderer.sprite = accuracySprites [1];
		} else if (accuracy >= 75) {
			spriteRenderer.sprite = accuracySprites [2];
		} else if (accuracy >= 65) {
			spriteRenderer.sprite = accuracySprites [3];
		} else {
			spriteRenderer.sprite = accuracySprites [4];
		}
		Destroy (this.gameObject, delay);
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
