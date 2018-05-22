using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapChartNode {

    /* TapChartNode is a class that stores the elements needed to
     * spawn new TapTargets within a charted scene. Charted scenes
     * involve building up 'charts' of nodes that store key information
     * about when to spawn those nodes and how fast those nodes should be 
     * shrinking to their final position. 
    */

    float spawnTime = 0.0f;

    float lifetime = 0.0f;

    Vector3 spawnPosition = Vector3.zero;

    public TapChartNode()
    {

    }
}
