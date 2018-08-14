using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;


public class VertxEventHandlerV2 : MonoBehaviour
{

    string incorrectComponent;
    string incorrectState;
    string correctState;

    public Dictionary<string, string> WrongAnimations = new Dictionary<string, string>();


    GameObject currentGameObject;
    public string[,] AnimationArray;

    GameObject PreviousAnimationNode;
    int currentStep;

    public bool IoTEnabled { get; set; }


    // Use this for initialization
    void Start()
    {
        currentStep = 0;
        AnimationArray = new string[,]
        {
                {"KEY_ANIMATION","62fe3789-6dc0-4be8-8de4-daf6be186bed", "1"},
                {"DOOR_ANIMATION","fe72fbeb-7341-40ec-91a9-7d4387bf9ff6", "1"},
                {"SWITCH_ONE","c85807c4-867d-41f8-8d11-09715c52d349", "0" },
                {"SWITCH_TWO","1b6364fb-fb42-4d4a-9427-91bf8ca35c91", "1"},
                {"SWITCH_THREE","f8501707-987c-4a18-ba1c-ffc6e99dd58f", "0"},
                {"FUSE_ANIMATION","55436d56-7f92-44ad-9d57-4c27d2a60b05", "0" },
                {"FUSE_ANIMATION_CLOSE","e6b3428a-e40c-402e-adc9-419b459b137c", "1" },
                {"SWITCH_FOUR","311eab37-eec5-4caa-880c-00b511fb26a5", "1"},
                {"SWITCH_FIVE","ec08686a-4d1e-49a1-ad7c-0c994a366929", "0"},
                {"SWITCH_SIX","8c0daae5-5c91-49d6-a479-711a82d1bbe2", "1"},
                {"DOOR_FINISH","b4f004f9-4f04-4e74-9603-a0a695778f7d", "0"},
                {"KEY_ANIMATION_FINISH","813d6861-2370-419d-951d-9fb71312c492","0"}
        };

    }



    // Update is called once per frame
    void Update()
    {
        if (currentGameObject)
        {
            GameObject box = GameObject.FindGameObjectWithTag("Box");
            currentGameObject.transform.position = box.transform.position;
            currentGameObject.transform.rotation = box.transform.rotation;
        }
    }

    public void setIoTEnabled(bool enabled)
    {
        IoTEnabled = enabled;
    }

    public void OnUpdate(object message)
    {
        // Deserialize the message received by IOT
        Message _message = JsonConvert.DeserializeObject<Message>(message.ToString());

        if (!IoTEnabled)
        {
            return;
        }

        if (PreviousAnimationNode)
        {
            DestroyImmediate(PreviousAnimationNode);
        }

        string componentName = _message.name;
        string componentState = _message.state.ToString();

        if (currentStep > 5)
        {
            switch (componentName)
            {
                case "SWITCH_THREE":
                    componentName = "SWITCH_FOUR";
                    break;
                case "SWITCH_TWO":
                    componentName = "SWITCH_FIVE";
                    break;
                case "SWITCH_ONE":
                    componentName = "SWITCH_SIX";
                    break;
                case "DOOR_ANIMATION":
                    componentName = "DOOR_FINISH";
                    break;
                case "KEY_ANIMATION":
                    componentName = "KEY_ANIMATION_FINISH";
                    break;
                case "FUSE_ANIMATION":
                    componentName = "FUSE_ANIMATION_CLOSE";
                    break;
            }
        }


        if (!isComplete())
        {
            if (!WrongAnimations.ContainsKey(componentName))
            {
                if (AnimationArray[currentStep, 0] != componentName && AnimationArray[currentStep, 2] != componentState)
                {
                    incorrectComponent = componentName;
                    incorrectState = componentState;
                    WrongAnimations.Add(componentName, componentState);
                    CreateNode(componentName, AnimationArray[findIndex(incorrectComponent), 2]);
                }
                else
                {
                    ExecuteAnimation(componentName, componentState);
                }
            }
            else
            {
                ExecuteIdiotAnimation(componentName, componentState);
            }
        }
        else
        {
            PreviousAnimationNode = CreateNode("MAINTANENCE_MODEL", "2359e967-bcd4-44ab-b58f-6f7a8ca391e4");
        }


        // Deserialize the message received by IOT
        //Debug.Log("Component name => " + componentName);
        //Debug.Log("Component state => " + componentState);


    }

    //maintenence completed when all animations have been played
    bool isComplete()
    {
        bool maintenenceComplete;
        if (currentStep == 11)
        {
            maintenenceComplete = true;
        }
        else
        {
            maintenenceComplete = false;
        }
        return maintenenceComplete;
    }

    void ExecuteAnimation(string _ComponentName, string _ComponentStatus)
    {
        if(AnimationArray[currentStep, 0] == _ComponentName && AnimationArray[currentStep, 2] == _ComponentStatus)
        {
            CreateNode(AnimationArray[currentStep + 1, 0], AnimationArray[currentStep + 1, 1]);
            currentStep++;
        }
    }

    void ExecuteIdiotAnimation(string receievedComponent, string receievedState)
    {
        int count = 0;

        if (receievedComponent == incorrectComponent && receievedState == correctState)
        {
            WrongAnimations.Remove(incorrectComponent);
            if(WrongAnimations.Count == 0)
            {
                CreateNode(AnimationArray[currentStep + 1, 0], AnimationArray[currentStep + 1, 1]);
                currentStep++;
            }
            else
            {
                foreach (var item in WrongAnimations)
                {
                    if (count == WrongAnimations.Count)
                    {
                        incorrectComponent = item.Key;
                        switch (item.Value)
                        {
                            case "0":
                                correctState = "1";
                                break;
                            case "1":
                                correctState = "0";
                                break;
                        }
                    }
                    count++;
                }

                CreateNode(incorrectComponent, AnimationArray[findIndex(incorrectComponent),2]);
            }
        }
        else
        {
            CreateNode(incorrectComponent, AnimationArray[findIndex(incorrectComponent), 2]);
        }
    }

    int findIndex(string componentName)
    {
        int index = 0;

        for (int i = 0; i < AnimationArray.Length/3; i++)
        {
            if(AnimationArray[i, 0] == componentName)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    public void InitKeyAnimation()
    {
        Debug.Log("Init key animation :");
        currentGameObject = CreateNode("KEY_ANIMATION", "62fe3789-6dc0-4be8-8de4-daf6be186bed");
        currentGameObject.AddComponent<AnimEventHandler>();
        PreviousAnimationNode = currentGameObject;
        currentStep = 0;
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
        currentGameObject = vertxThing;
        currentGameObject.AddComponent<AnimEventHandler>();
        PreviousAnimationNode = SceneLink.Instance.transform.Find(name).gameObject;
        return vertxThing;

    }


}