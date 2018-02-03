using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
	private Vector3 position;
	private Vector3 screenToWorldPointPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/*
    mouseData->x = nowX / (float)ofGetWidth();
    mouseData->y = 1.0 - nowY / (float)ofGetHeight();
		*/

		if (Input.GetMouseButton (0)) {
			position = Input.mousePosition;
			position.z = 10.0f;
			screenToWorldPointPosition = Camera.main.ScreenToWorldPoint (position);
			Vector3 screenPos = Camera.main.WorldToViewportPoint (screenToWorldPointPosition);
			Debug.Log (screenPos);
		}

	}
}
