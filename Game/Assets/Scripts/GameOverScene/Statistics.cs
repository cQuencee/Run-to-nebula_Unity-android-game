using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Statistics : MonoBehaviour {

    public Text Coins, TravelDistance;
    public static int CoinsCollected;
	// Use this for initialization
	void Start () {
        Coins.text = "You've collected " + moveplayer.coinCollected + " coins";
        TravelDistance.text = "You've traveled " + System.String.Format("{0:0.##}", moveplayer.DistanceTravled) + " light years";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
