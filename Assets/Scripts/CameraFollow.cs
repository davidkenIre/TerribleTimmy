using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public GameObject targetObject;
    // This is used to offset the camera such that it keeps ahead of the  
    // mouse, else the mouse will just move to the center of the screen
    private float distanceToTarget;   

    // Use this for initialization
    void Start () {
        // Calculate the amout of offset the camera 
        distanceToTarget = transform.position.x - targetObject.transform.position.x;

    }
	
	// Update is called once per frame
	void Update () {

        float targetObjectX = targetObject.transform.position.x;

        Vector3 newCameraPosition = transform.position;
        newCameraPosition.x = targetObjectX + distanceToTarget;
        transform.position = newCameraPosition;

    }
}
