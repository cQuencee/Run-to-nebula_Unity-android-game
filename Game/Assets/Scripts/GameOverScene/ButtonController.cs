using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	public void PlayAgain()
    {
        movecamera.moveSpeed = 8.5f;
        moveplayer.moveSpeed = 8.5f;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
