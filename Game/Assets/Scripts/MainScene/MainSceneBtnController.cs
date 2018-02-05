using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainSceneBtnController : MonoBehaviour {

    public GameObject StartBtn, HeaderTxt;
    Button btn;
    // Use this for initialization
    void Start () {
        btn = StartBtn.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.S))
        {
            btn.onClick.Invoke();
        }
	}
    public void StartGameBtn()
    {
        AsteroidTravel.StartTraveling = true;
        HeaderTxt.SetActive(false);
        StartBtn.SetActive(false);
    }
}
