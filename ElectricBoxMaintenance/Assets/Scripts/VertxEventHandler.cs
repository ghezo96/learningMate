using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;


public class VertxEventHandler : MonoBehaviour {


    private Dictionary<string, string> AnimationDictionary = new Dictionary<string, string>();
    GameObject currentGameObject;


    // Use this for initialization
    void Start()
    {
        InitaliseAnimations();
    }

    // DICTIONARY WITH ANIMATIONS
    void InitaliseAnimations()
    {
        AnimationDictionary.Add("KEY_ANIMATION", "62fe3789-6dc0-4be8-8de4-daf6be186bed");
        AnimationDictionary.Add("SWITCH_ONE", "59d89c08-85a6-4e17-9edb-a2b648d0503e");
        AnimationDictionary.Add("SWITCH_TWO", "3fe496cd-7cf7-44d8-8388-74ac49e14986");
        AnimationDictionary.Add("SWITCH_THREE", "9d0db931-1698-47af-8080-3106cfede727");
        AnimationDictionary.Add("SWITCH_FOUR", "a3418e0a-a9a3-4ee2-ada9-5eb0da5a0d29");
        AnimationDictionary.Add("SWITCH_FIVE", "30fc82a8-7b3b-4cab-afb9-c8304a6c756d");
        AnimationDictionary.Add("SWITCH_SIX", "d81a4656-da1e-44ce-b367-54856aa31127");
    }


	// Update is called once per frame
	void Update () {
        if (currentGameObject)
        {
            GameObject box = GameObject.FindGameObjectWithTag("Box");
            currentGameObject.transform.position = box.transform.position;
            currentGameObject.transform.rotation = box.transform.rotation;
        }
    }



    public void OnUpdate(object message)
    {
        // Deserialize the message received by IOT
        Message _message = JsonConvert.DeserializeObject<Message>(message.ToString());

        //Validate message received from IoT and then Start next set of instruction base on message
        ValidateUserAction(_message);
    }

    // Validate IoT message and start the next set of instruction
    private void ValidateUserAction(Message message)
    {

        string componentNameWithState = message.name + message.state;

        switch (componentNameWithState)
        {
            case "KEY_ANIMATION1":
                {
                    StartNextInstruction("SWITCH_ONE", AnimationDictionary["SWITCH_ONE"], message);
                    break;
                }
            case "SWITCH_ONE0":
                {
                    StartNextInstruction("SWITCH_TWO", AnimationDictionary["SWITCH_TWO"], message);
                   
                    break;
                }
            case "SWITCH_ONE1":
                {
                    StartNextInstruction("KEY_ANIMATION", AnimationDictionary["KEY_ANIMATION"], message);
                    
                    break;
                }
            case "SWITCH_TWO0":
                {
                    StartNextInstruction("SWITCH_ONE", AnimationDictionary["SWITCH_ONE"], message);
                    break;
                }
            case "SWITCH_TWO1":
                {
                    StartNextInstruction("SWITCH_THREE", AnimationDictionary["SWITCH_THREE"], message);
                    break;
                }
            case "SWITCH_THREE0":
                {
                    StartNextInstruction("SWITCH_TWO", AnimationDictionary["SWITCH_TWO"], message );
                    break;
                }

            case "SWITCH_THREE1":
                {
                    StartNextInstruction("BATTERY", AnimationDictionary["BATTERY"], message);
                    break;
                }

            case "BATTERY0":
                {
                    StartNextInstruction("SWITCH_THREE", AnimationDictionary["SWITCH_THREE"], message);
                    break;
                }

            case "BATTERY1":
                {
                    StartNextInstruction("NEXT", AnimationDictionary["NEXT"], message);
                    break;
                }

                // BATTERY
        }

    }

    // Start next instruction
    private void StartNextInstruction(string name, string id, Message message)
    { 
        DestroyObject(currentGameObject);
        currentGameObject = CreateNode(name, id);
        currentGameObject.AddComponent<KeyAnimEventHandler>();

        Debug.Log("Found object " + GameObject.FindGameObjectWithTag("Finish"));
    }


    public void InitKeyAnimation()
    {
        Debug.Log("Init key animation :");
        currentGameObject = CreateNode("KEY_ANIMATION", "62fe3789-6dc0-4be8-8de4-daf6be186bed");
        currentGameObject.AddComponent<KeyAnimEventHandler>();
    }

    // Method to create and return Vertex Node Link Game object 
    private GameObject CreateNode(string name, string id)
    {
        GameObject box = GameObject.FindGameObjectWithTag("Box");
        var vertxObject = SceneLink.Instance.transform.Find(name);
        GameObject vertxThing;
        if (vertxObject == null)
        {
            vertxThing = SceneLink.Instance.CreateNode(name,
                box.transform.position,
                box.transform.rotation,
                Vector3.one,
                id
           );
        }
        else
        {
            vertxThing = vertxObject.gameObject;
            Debug.Log("node already exists");

        }
        return vertxThing;

    }


}


//
//GameObject parentCopy = new GameObject();
//parentCopy.transform.parent = SceneLink.Instance.transform.transform;
//parentCopy.transform.position = box.transform.position;
//parentCopy.transform.rotation = box.transform.rotation;

//parentCopy.transform.parent = null;

//Vector3 targetPos = parentCopy.transform.localPosition;
//Quaternion targetRot = parentCopy.transform.localRotation;



//vertxThing.transform.parent = parentCopy.transform;
//vertxThing.transform.localPosition = Vector3.zero;
//vertxThing.transform.localRotation = Quaternion.identity;
//vertxThing.transform.parent = SceneLink.Instance.transform;
//Destroy(parentCopy);