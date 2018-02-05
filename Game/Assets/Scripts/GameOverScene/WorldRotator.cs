using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRotator : MonoBehaviour {
    public float rotationSpeed = 2;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed, Space.World);
    }
}
