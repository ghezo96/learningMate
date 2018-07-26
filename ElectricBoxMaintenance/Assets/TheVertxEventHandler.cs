using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

public class TheVertxEventHandler : MonoBehaviour {

    public string[,,] AnimationArray;

    GameObject PreviousAnimationNode;

    int PreviousStep;
    int CurrentStep;

    // Use this for initialization
    void Start () {
        AnimationArray = new string[,,]
        {
            {
                {"KEY_ANIMATION","62fe3789-6dc0-4be8-8de4-daf6be186bed", "1"},
                {"SWITCH_ONE","59d89c08-85a6-4e17-9edb-a2b648d0503e", "0" },
                {"SWITCH_TWO","3fe496cd-7cf7-44d8-8388-74ac49e14986", "0"},
                {"SWITCH_THREE","9d0db931-1698-47af-8080-3106cfede727", "0"},
                {"SWITCH_FOUR","a3418e0a-a9a3-4ee2-ada9-5eb0da5a0d29", "0"},
                {"SWITCH_FIVE","30fc82a8-7b3b-4cab-afb9-c8304a6c756d", "0"},
                {"SWITCH_SIX","d81a4656-da1e-44ce-b367-54856aa31127,", "0"}
            }
        };

        // Start at step 1
        CurrentStep = 0;

        // Create key animation node if it doesn't exist 
        CreateNode("KEY_ANIMATION", "62fe3789-6dc0-4be8-8de4-daf6be186bed");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnUpdate(object message)
    {
        Debug.Log("<OnUpdate>");

        // Deserialize the message received by IOT
        Message _message = JsonConvert.DeserializeObject<Message>(message.ToString());

        Debug.Log("Component => " + _message.name + "Status => " + _message.state);

        string componentName = _message.name;
        string componentState = _message.state.ToString();

        // Update array
        UpdateComponentArray(componentName, componentState);

        // Play animations
        //ExecuteAnimation(componentName, componentState);
        

        if(CurrentStep == PreviousStep)
        {
            DestroyObject(PreviousAnimationNode);
        }
        else
        {
            PreviousStep = CurrentStep;
            CurrentStep++;
        }
    }

    void UpdateComponentArray(string _ComponentName, string _ComponentStatus)
    {
        for (int i = 1; i < (AnimationArray.Length / 3); i++)
        {
            if (_ComponentName == AnimationArray[0, i, 0])
            {
                AnimationArray[0, i, 2] = _ComponentStatus;
                PreviousAnimationNode = SceneLink.Instance.transform.Find(AnimationArray[0, (i - 1), 2]).gameObject;
                break;
            }
        }
    }

    void ExecuteAnimation(string _ComponentName, string _ComponentStatus)
    {
        //Debug.Log("<ExecuteAnimation>");
        for (int i = 1; i < (AnimationArray.Length / 3); i++)
        {
            if (AnimationArray[0,i,0] == _ComponentName)
            {
                if (AnimationArray[0, i, 2] == "0")
                {
                    //Debug.Log("Creating node " + _ComponentName);
                    CreateNode(AnimationArray[0, i, 0], AnimationArray[0, i, 1]);
                }
                else if (AnimationArray[0, i, 2] == "1")
                {
                    //Debug.Log("Deleting node " + _ComponentName);
                    DestroyObject(SceneLink.Instance.transform.Find(_ComponentName).gameObject);
                }
                else
                {
                    Debug.Log("<"+ _ComponentName + ">"  + "<Null Status>");
                }
                break;
            }
        }
    }

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
            Debug.Log(name + " node already exists");

        }
        return vertxThing;

    }
}
