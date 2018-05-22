using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioVisualizer;

public class BeatSpawnController : MonoBehaviour {

    // Store for spawning TapTargets
    [SerializeField]
    GameObject TapTargetPrefab;

    // Store for Accuracy Indicator prefab
    [SerializeField]
    GameObject AccuracyIndicatorPrefab;

    // Store a reference to the AudioEventListener so we can read public variables
    [SerializeField]
    AudioEventListener ael;

    void OnEnable()
    {
        TapTargetEventManager.OnTapped += OnTapTargetTapped;
        AudioEventListener.OnBeatRecognized += SpawnTapTarget;
    }


    void OnDisable()
    {
        TapTargetEventManager.OnTapped -= OnTapTargetTapped;
        AudioEventListener.OnBeatRecognized -= SpawnTapTarget;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTapTargetTapped(Vector3 position,  float accuracy)
    {
        print("Target tapped. Accuracy = " + (int)(accuracy));
    }

    //called in an AuidioFileEventListener.OnBeat event, 'preBeatOffset'seconds before the beat happens
    //spawn a TapTarget at a random location with a lifetime equal to the 'preBeatOffset'
    void SpawnTapTarget(Beat beat)
    {
        // Get the location to spawn the new target
        Vector3 nextSpawnPosition = new Vector3(Random.Range(-6f, 7f), Random.Range(-2.5f, 3.25f), 1f);

        // Instantiate a new target at the given location and get a reference to its TapTarget component
        GameObject newTarget = Instantiate(TapTargetPrefab, nextSpawnPosition, Quaternion.identity);

        // Assign the lifetime to the new target. Log error if this fails for any reason.
        try
        {
            newTarget.GetComponent<NewTapTarget>().TapTargetSetup(ael.preBeatOffset);
        }
        catch
        {
            Debug.LogError("TapTarget component not found in newTarget object.");
        }
    }
}
