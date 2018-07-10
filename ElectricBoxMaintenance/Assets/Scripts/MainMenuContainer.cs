using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuContainer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   public void SetActiveStatus(bool status)
    {
        gameObject.SetActive(status);
    }
}
