using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

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
            VertexAuthentication.LaunchRemoteAssist("5a9db979-83ef-430b-bb01-733d27feafcd");

        }
        else if(button.name == "ContactButton2")
        {
            VertexAuthentication.LaunchRemoteAssist("669469a6-488c-464b-90b0-69d937ebdf05");
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
