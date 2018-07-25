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
        // Get the components status
        ComponentsStatus componentsStatusObject = JsonConvert.DeserializeObject<ComponentsStatus>(componentsWithStatus);
        componentList = componentsStatusObject.Components;
        InitaliseAnimations();
        //
        GameObject keyAnimationObject = CreateNode("KEY_ANIMATION", "ac100581-430a-4817-b2c8-9978144b521f");

        keyAnimationObject.AddComponent<AnimationHandler>();

        currentGameObject = keyAnimationObject;
    }

    void InitaliseAnimations()
    {
        AnimationDictionary.Add("KEY_ANIMATION", "ac100581-430a-4817-b2c8-9978144b521f");
        AnimationDictionary.Add("SWITCH_ONE", "66d507fc-e459-4ba0-883c-654643d0b28a");
        AnimationDictionary.Add("SWITCH_TWO", "e5a2f9c6-46fe-4fd4-8e70-d175c8179a7c");
        AnimationDictionary.Add("SWITCH_THREE", "a635a8ae-3107-49b0-a103-7340d65e51e4");
    }


	// Update is called once per frame
	void Update () {
		
	}



    public void OnUpdate(object message)
    {
        // Deserialize the message received by IOT
        Message _message = JsonConvert.DeserializeObject<Message>(message.ToString());
        Debug.Log("OnUpdate: \nName => " + _message.name + " status => " + _message.state);
        
        // Update component 
        // UpdateComponentStatus(_message);

        //Validate message received from IoT and then Start next set of instruction base on message
        ValidateUserAction(_message);
    }

    // Validate IoT message and start the next set of instruction
    private void ValidateUserAction(Message message)
    {
        //if(currentGameObject != null)
        //{
        //    currentGameObject.GetComponent<KeyAnimEventHandler>().DestroyIt();
        //}

        //if (message.name == "KEY_ANIMATION")
        //{
        //    currentGameObject = (message.state == 0)
        //        ? StartNextInstruction("KEY_ANIMATION", "ac100581-430a-4817-b2c8-9978144b521f")
        //        : StartNextInstruction("SWITCH_ONE", "66d507fc-e459-4ba0-883c-654643d0b28a");
        //}
        //if (message.name == "SWITCH_ONE" && message.state == 1)
        //{
        //    currentGameObject = (message.state == 0)
        //        ? StartNextInstruction("SWITCH_ONE", "66d507fc-e459-4ba0-883c-654643d0b28a")
        //        : StartNextInstruction("SWITCH_TWO", "e5a2f9c6-46fe-4fd4-8e70-d175c8179a7c");

        //}
        //if (message.name == "SWITCH_TWO" && message.state == 1)
        //{

        //    currentGameObject = (message.state == 0)
        //        ? StartNextInstruction("SWITCH_TWO", "e5a2f9c6-46fe-4fd4-8e70-d175c8179a7c")
        //        : StartNextInstruction("SWITCH_THREE", "a635a8ae-3107-49b0-a103-7340d65e51e4");

        //}

        if (message.name == "KEY_ANIMATION")
        {
            currentGameObject = (message.state == 0)
                ? StartNextInstruction("KEY_ANIMATION", "ac100581-430a-4817-b2c8-9978144b521f")
                : StartNextInstruction("SWITCH_ONE", "66d507fc-e459-4ba0-883c-654643d0b28a");
        }
        if (message.name == "SWITCH_ONE")
        {
            currentGameObject = (message.state == 0)
                ? StartNextInstruction("SWITCH_ONE", "66d507fc-e459-4ba0-883c-654643d0b28a")
                : StartNextInstruction("SWITCH_TWO", "e5a2f9c6-46fe-4fd4-8e70-d175c8179a7c");

        }
        if (message.name == "SWITCH_TWO")
        {

            currentGameObject = (message.state == 0)
                ? StartNextInstruction("SWITCH_TWO", "e5a2f9c6-46fe-4fd4-8e70-d175c8179a7c")
                : StartNextInstruction("SWITCH_THREE", "a635a8ae-3107-49b0-a103-7340d65e51e4");

            currentGameObject.GetComponent<IComponent>().OnNotify(message);
        }
    }

    // Start next instruction
    private GameObject StartNextInstruction(string name, string id)
    {
        GameObject vertxObject = CreateNode(name, id);
        vertxObject.AddComponent<AnimationHandler>();
        return vertxObject;
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
