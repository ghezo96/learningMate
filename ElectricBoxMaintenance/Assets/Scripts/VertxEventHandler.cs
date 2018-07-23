using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VertxEventHandler : MonoBehaviour {

    public class Message
    {
        public string name { get; set; }
        public string state { get; set; }
    }



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnUpdate(object message)
    {
        Message _message = JsonConvert.DeserializeObject<Message>(message.ToString());
        Debug.Log("OnUpdate: " + _message.name + " status => " + _message.state);
    }
}
