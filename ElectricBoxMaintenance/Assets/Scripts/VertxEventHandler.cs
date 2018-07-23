using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;


public class VertxEventHandler : MonoBehaviour {

  

    public class Component
    {
        public string name { get; set; }
        public int status { get; set; }
        public int completed { get; set; }
    }

    public class ComponentsStatus
    {
        public List<Component> Components { get; set; }
    }

    private Dictionary<string, string> AnimationDictionary = new Dictionary<string, string>();

    string componentsWithStatus = "{\"Components\":[{\"name\":\"switch1\",\"status\":0,\"completed\":0},{\"name\":\"switch2\",\"status\":0,\"completed\":0},{\"name\":\"switch3\",\"status\":0,\"completed\":0},{\"name\":\"door\",\"status\":0,\"completed\":0},{\"name\":\"key\",\"status\":0,\"completed\":0}]}";
    // List to hold all the component with their sssname and status
    List<Component> componentList;
    GameObject currentGameObject;


    // Use this for initialization
    void Start()
    {
        Debug.Log("STart");
        

        // Get the components status
        ComponentsStatus componentsStatusObject = JsonConvert.DeserializeObject<ComponentsStatus>(componentsWithStatus);
        componentList = componentsStatusObject.Components;
        InitaliseAnimations();
        //
        GameObject keyAnimationObject = CreateNode("KeyAnimation", "ac100581-430a-4817-b2c8-9978144b521f");
        keyAnimationObject.AddComponent<KeyAnimEventHandler>();
        currentGameObject = keyAnimationObject;
    }

    void InitaliseAnimations()
    {
        AnimationDictionary.Add("keyAnimation", "ac100581-430a-4817-b2c8-9978144b521f");
        AnimationDictionary.Add("Anim_Switch1", "36748b64-8251-4b76-8672-d67d5522dfb1");
        AnimationDictionary.Add("Anim_Switch2", "c74cf1eb-da54-448e-b5c2-a423d16064a4");
        AnimationDictionary.Add("Anim_Switch3", "ec53c75e-ec10-4014-8d96-f73f2e27ca82");
    }


	// Update is called once per frame
	void Update () {
		
	}


    public void OnUpdate(object message)
    {
        Debug.Log("OnUpdate: " + message);
        Message _message = JsonConvert.DeserializeObject<Message>(message.ToString());

        Debug.Log("OnUpdate: " + _message.name + " status => " + _message.state);

        string animationName = _message.name;

        // Update component 
        UpdateComponentStatus(_message);

        currentGameObject.GetComponent<IComponent>().OnNotify(_message);

        // Start next set of instruction base on message received by IOT

        if(_message.name == "keyAnimation" &&  _message.state == 1)
        {
            StartNextInstruction("Anim_Switch1", "66d507fc-e459-4ba0-883c-654643d0b28a");
            
            
        }
        if (_message.name == "switchOne" && _message.state == 1)
        {
            StartNextInstruction("Anim_Switch2", "e5a2f9c6-46fe-4fd4-8e70-d175c8179a7c");
            
        }
        if (_message.name == "switchTwo" && _message.state == 1)
        {
            StartNextInstruction("Anim_Switch3", "a635a8ae-3107-49b0-a103-7340d65e51e4");
            
        }
        

    }

    private void StartNextInstruction(string name, string id)
    {
        currentGameObject.GetComponent<KeyAnimEventHandler>().DestroyIt();
        GameObject keyAnimationObject = CreateNode(name, id);
        keyAnimationObject.AddComponent<KeyAnimEventHandler>();
        currentGameObject = keyAnimationObject;
    }

    private void UpdateComponentStatus(Message message)
    {
        foreach (Component component in componentList)
        {
            if (component.name == name && component.status != message.state)
            {
                // Update components status 
                component.status = message.state;
                // send message to component to update itself
                currentGameObject.GetComponent<IComponent>().OnNotify(message);
            }
        }
    }

    // Method to create and return Vertex Node Link Game object 
    private GameObject CreateNode(string name, string id)
    {

        Debug.Log("Node: " + name + " id : " + id);
        var vertxObject = SceneLink.Instance.transform.Find(name);
        GameObject vertxThing;

        if (vertxObject == null)
        {
            vertxThing = SceneLink.Instance.CreateNode(name,
                new Vector3(0f, 0f, 0f),
                Quaternion.identity,
                Vector3.one,
                id
           );
        }
        else
        {
            vertxThing = vertxObject.gameObject;

        }
        return vertxThing;

    }


}
