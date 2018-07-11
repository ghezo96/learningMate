using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {

    public GameObject recordWindow;
    public GameObject propertyWindow;
    public GameObject statusWindow;


   

    // Use this for initialization
    void Start () {
        recordWindow.SetActive(true);
        propertyWindow.SetActive(true);
        statusWindow.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
