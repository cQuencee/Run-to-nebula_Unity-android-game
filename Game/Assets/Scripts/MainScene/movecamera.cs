using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movecamera : MonoBehaviour {

    // Use this for initialization
    public static float moveSpeed = 8.5f;
    public GameObject TravelingAsteroid, Runner;
    Vector3 offset;
    public static bool Done = false;
    public static bool startCamPositioning = false;
    public Vector3 startCamRot;
    public Vector3 startCamPos;
    public Vector3 endCamRot;
	void Start () {
        offset = transform.localPosition - TravelingAsteroid.transform.localPosition;
    }
    private void OnDestroy()
    {
        Done = false;
        startCamPositioning = false;
    }
    // Update is called once per frame
    void Update () {
        if (moveplayer.StartGame)
        {
            Camera.main.transform.Translate(new Vector3(0, 0, moveSpeed) * Time.deltaTime, Space.World);
        }
        if (startCamPositioning)
        {
            startCamPositioning = false;
            var playerPos = Runner.transform.localPosition;
            playerPos.z = -1f;
            playerPos.y = 5.52f;
            playerPos.x = 0;
            startCamPos = transform.localPosition;
            startCamRot = transform.localEulerAngles;
            endCamRot = Runner.transform.localEulerAngles;
            endCamRot.x = 25f;
            StartCoroutine(camPositioning(transform, startCamPos, playerPos, startCamRot, endCamRot, Time.time));
        }
        
    }
    private void LateUpdate()
    {
        if (AsteroidTravel.StartTraveling && !Done)
        {
            transform.localPosition = TravelingAsteroid.transform.localPosition + offset;
        }
    }
    public IEnumerator camPositioning(Transform thisTransform, Vector3 startPos, Vector3 endPos, Vector3 startRot, Vector3 endRot, float time)
    {
        float currTime = 0;
        float TimeToMove = 2f;
        while(currTime <= TimeToMove)
        {
            currTime += Time.deltaTime;
            transform.localEulerAngles = new Vector3(Mathf.LerpAngle(startRot.x, endRot.x, currTime/TimeToMove), Mathf.LerpAngle(startRot.y, endRot.y, currTime / TimeToMove), Mathf.LerpAngle(startRot.z, endRot.z, currTime / TimeToMove));
            thisTransform.localPosition = cubeBezier(startPos, new Vector3(10, 5, 10), new Vector3(8, 6, -3), endPos, currTime / TimeToMove);
            yield return null;
        }
        if(thisTransform.localPosition != endPos)
        {
            thisTransform.localPosition = endPos;
        }
    }
    public static Vector3 cubeBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float time)
    {
        return (((-p0 + 3*( p1-p2 ) + p3) * time + (3* ( p0 + p2 )- 6*p1 )) * time + 3 * ( p1-p0 )) *time + p0;
    }
}
