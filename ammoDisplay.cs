using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ammoDisplay : MonoBehaviour {

    public static int ammo;
    Text ammoCount;
   
	// Use this for initialization
	void Start ()
    {

        ammoCount = GetComponent<Text>();
        ammo = 10;
        

    }
	
	//Update is called once per frame
	void Update ()
    {
        ammoCount.text = "Ammo: " + ammo;
    }
}
