using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharRotation : MonoBehaviour {

    // Use this for initialization
    public float rotationSpeed = 10, xAngle = 0.1f, yAngle = 0.2f, zAngle = 0.3f;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(xAngle, yAngle, zAngle), Time.deltaTime * rotationSpeed, Space.Self);
        
	}
}
