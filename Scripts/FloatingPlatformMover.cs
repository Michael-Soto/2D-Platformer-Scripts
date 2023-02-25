using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatformMover : MonoBehaviour {

    private float origin;
    private int distance;

	// Use this for initialization
	void Start () {
        origin = transform.position.x; // set starting point
        distance = 6; // set how far the platform should go
	}
	
	// Update is called once per frame
    // Moves the platform back and forth
	void Update () {
        transform.position = new Vector3(origin + Mathf.PingPong(Time.time, distance), transform.position.y, 0);
	}
}
