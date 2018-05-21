using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSpawnController : MonoBehaviour {

    // Store for spawning TapTargets
    [SerializeField]
    GameObject TapTargetPrefab;

    // Store for Accuracy Indicator prefab
    [SerializeField]
    GameObject AccuracyIndicatorPrefab;

    void OnEnable()
    {
        TapTargetEventManager.OnTapped += OnTapTargetTapped;
    }


    void OnDisable()
    {
        TapTargetEventManager.OnTapped -= OnTapTargetTapped;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTapTargetTapped(Vector3 position,  float accuracy)
    {
        print("Target tapped. Accuracy = " + (int)(accuracy * 100));
    }
}
