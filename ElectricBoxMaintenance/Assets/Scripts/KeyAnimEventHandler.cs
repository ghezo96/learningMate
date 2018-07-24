﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAnimEventHandler : MonoBehaviour, IComponent {


    public void OnNotify(Message message)
    {
        Debug.Log("OnNotify : state " + message.state + " name : " + message.name );

        if(message.state == 1)
        {
            Debug.Log("Message state: " + message.state);
            DestroyObject(gameObject);
        }
       
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DestroyIt()
    {
        Destroy(gameObject);
    }

}
