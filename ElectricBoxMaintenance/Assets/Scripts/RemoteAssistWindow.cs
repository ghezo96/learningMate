using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteAssistWindow : MonoBehaviour {
    

	// Use this for initialization
	void Start () {

        foreach(FloatingButton button in gameObject.GetComponentsInChildren<FloatingButton>())
        {
            button.Clicked += Button_Clicked;
        }

	}

    private void Button_Clicked(GameObject button)
    {
       if(button.name == "ContactButton1")
        {

        } else if(button.name == "ContactButton2")
        {
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
