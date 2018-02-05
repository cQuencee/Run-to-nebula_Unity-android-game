using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteriodScript : MonoBehaviour {

    // Use this for initialization
    float thrust = 1.5f;
    public bool Explode = false;
    public Rigidbody rb;
    Vector3 forceDirection;
    void Start () {
        rb = GetComponent<Rigidbody>();
        if (gameObject.name == "forward")
        {
            forceDirection = new Vector3(-1f, 0.1f, 1);
        }
        else if (gameObject.name == "left")
        {
            forceDirection = new Vector3(-1f, 0.1f, 1f);
        }
        else if (gameObject.name == "forward_left")
        {
            forceDirection = new Vector3(-1f, 0.1f, 1f);
        }
        else if (gameObject.name == "forward_right")
        {
            forceDirection = new Vector3(1f, 0.1f, 1f);
        }
        else if (gameObject.name == "forward_up")
        {
            forceDirection = new Vector3(0.1f, 1f, 1f);
        }
        else if (gameObject.name == "forward_right_up")
        {
            forceDirection = new Vector3(1f, 0.3f, 1f);
        }
        else if (gameObject.name == "forward_right_1")
        {
            forceDirection = new Vector3(1f, 0.1f, 1f);
        }
        else if (gameObject.name == "forward_up_1")
        {
            forceDirection = new Vector3(-0.5f, 1f, 1f);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Explode)
        {
            rb.AddRelativeForce(forceDirection * thrust, ForceMode.Impulse);
        }
    }
}
