using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidTravel : MonoBehaviour {

    // Use this for initialization
    public static Vector3 startPos, endPos;
    public GameObject ObjectToHit;
    public static bool SceneLoadComplete = false, StartTraveling = false;
    float currentTime = 0f;
    public float timeToMove = 2f;
	void Start () {
        startPos = transform.localPosition;
        endPos = new Vector3(0, 0, 3);
        //Debug.Log("ASteroid start pos: " + startPos);
        //Debug.Log("ASteroid end pos: " + endPos);
	}
	
	// Update is called once per frame
	void Update () {
        if (StartTraveling)
        {
            if(currentTime <= timeToMove)
            {
                currentTime += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(startPos, endPos, currentTime / timeToMove);
                //Debug.Log("Asteroid position: " + transform.localPosition);
            }
        }
        transform.Rotate(new Vector3(1f, 1f, 1f), 0.5f);
        //Debug.Log("ASteroid pos: " + transform.localPosition);
    }
    private void OnDestroy()
    {
        StartTraveling = false;
    }
}
