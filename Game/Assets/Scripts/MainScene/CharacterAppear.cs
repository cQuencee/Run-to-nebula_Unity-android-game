using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterAppear : MonoBehaviour {

    public GameObject Character, LeftBtn, RightBtn, Destroyer;
    public ParticleSystem exploslsion;
    bool charShown = false;
    public Text CountDownText;
    int counter = 5;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(exploslsion.isPlaying);
        if (exploslsion.time >= 0.60f && !charShown)
        {
            Character.SetActive(true);
            StartCoroutine(StartCountDown());
            charShown = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "asteroidTransition")
        {
            exploslsion.Play();
            movecamera.Done = true;
            Destroy(other.gameObject);
        }
    }

    public IEnumerator StartCountDown()
    {
        LeftBtn.SetActive(true);
        RightBtn.SetActive(true);
        movecamera.startCamPositioning = true;
        CountDownText.gameObject.SetActive(true);
        while(counter > 0)
        {
            CountDownText.text = ""+counter;
            counter--;
            yield return new WaitForSeconds(1f);
        }
        Destroyer.SetActive(true);
        CountDownText.gameObject.SetActive(false);
        moveplayer.Anim.SetBool("StartGame", true);
        moveplayer.StartGame = true;
        
    }
    
}
