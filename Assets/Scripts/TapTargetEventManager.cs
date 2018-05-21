using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTargetEventManager : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Public Functions

    public delegate void TapAction(Vector3 position, float accuracy);
    public static event TapAction OnTapped;

    #endregion
}
