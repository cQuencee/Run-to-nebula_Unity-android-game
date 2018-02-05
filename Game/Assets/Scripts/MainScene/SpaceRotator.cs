using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SpaceRotator : MonoBehaviour {

    public float rotationSpeed = 2;

    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
        
        transform.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed, Space.World);
    }
    /*private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        Debug.Log("Scene loaded");
        AsteroidTravel.SceneLoadComplete = true;
    }*/
}
