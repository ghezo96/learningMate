using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;


public class VertxEventHandler : MonoBehaviour
{
    string newValue;

    bool correctStep = true;

    int incorrectStep;


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
                {"KEY_ANIMATION","62fe3789-6dc0-4be8-8de4-daf6be186bed", "1"}, //
                {"DOOR_ANIMATION","fe72fbeb-7341-40ec-91a9-7d4387bf9ff6", "1"}, //
                {"SWITCH_ONE","c85807c4-867d-41f8-8d11-09715c52d349", "0" }, //
                {"SWITCH_TWO","1b6364fb-fb42-4d4a-9427-91bf8ca35c91", "1"}, //
                {"SWITCH_THREE","f8501707-987c-4a18-ba1c-ffc6e99dd58f", "0"}, //
                {"FUSE_ANIMATION","67d64a7a-097b-4eb1-ba29-64af9c8537b9", "0" },
                {"FUSE_ANIMATION_CLOSE","33a4426a-cb3d-4618-9f5e-cd0fb4314c2e", "1" },
                {"SWITCH_FOUR","311eab37-eec5-4caa-880c-00b511fb26a5", "1"}, // 3
                {"SWITCH_FIVE","ec08686a-4d1e-49a1-ad7c-0c994a366929", "0"}, // 2
                {"SWITCH_SIX","8c0daae5-5c91-49d6-a479-711a82d1bbe2", "1"}, // 1
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

        string componentName = _message.name;
        string componentState = _message.state.ToString();

        if (PreviousAnimationNode)
        {
            DestroyImmediate(PreviousAnimationNode);
        }

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

        if (correctStep)
        {
            // Deserialize the message received by IOT

            //Debug.Log("Component name => " + componentName);
            //Debug.Log("Component state => " + componentState);

            //SAME SWITCHES USED

            if (!isComplete())
            {
                ExecuteAnimation(componentName, componentState);
            }
            else
            {
                PreviousAnimationNode = CreateNode("MAINTANENCE_MODEL", "2359e967-bcd4-44ab-b58f-6f7a8ca391e4");
            } 
        }
        else
        {
            if (componentName == AnimationArray[incorrectStep, 0] && componentState == AnimationArray[incorrectStep, 2])
            {
                switch (componentState)
                {
                    case "1":
                        newValue = "0";
                        break;
                    case "0":
                        newValue = "1";
                        break;
                }

                AnimationArray[incorrectStep, 2] = newValue;
                CreateNode(AnimationArray[currentStep, 0], AnimationArray[currentStep, 1]);
                correctStep = true;
            }
            else
            {
                CreateNode(AnimationArray[incorrectStep, 0], AnimationArray[incorrectStep, 1]);
            }
        }

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
        if (_ComponentName == AnimationArray[currentStep, 0] && _ComponentStatus == AnimationArray[currentStep, 2])
        {
            CreateNode(AnimationArray[currentStep + 1, 0], AnimationArray[currentStep + 1, 1]);
            currentStep++;
        }
        else
        {
            correctStep = false;

            for (int i = 0; i < AnimationArray.Length/3; i++)
            {
                if(AnimationArray[i,0] == _ComponentName)
                {

                    Debug.Log(AnimationArray[i, 0]);


                    switch (_ComponentStatus)
                    {
                        case "1":
                            newValue = "0";
                            break;
                        case "0":
                            newValue = "1";
                            break;
                    }

                    switch (_ComponentName)
                    {
                        case "SWITCH_ONE":
                            if(currentStep >= 3)
                            {
                                incorrectStep = 2;
                                CreateNode(AnimationArray[2, 0], AnimationArray[2, 1]);
                                Debug.Log(AnimationArray[2, 0]);
                            }
                            else
                            {
                                incorrectStep = 9;
                                CreateNode(AnimationArray[9, 0], AnimationArray[9, 1]);
                                Debug.Log(AnimationArray[9, 0]);
                            }
                            break;
                        case "SWITCH_TWO":
                            if(currentStep >= 4)
                            {
                                incorrectStep = 3;
                                CreateNode(AnimationArray[3, 0], AnimationArray[3, 1]);
                                Debug.Log(AnimationArray[3, 0]);
                            }
                            else
                            {
                                incorrectStep = 8;
                                CreateNode(AnimationArray[8, 0], AnimationArray[8, 1]);
                                Debug.Log(AnimationArray[8, 0]);

                            }
                            break;
                        case "SWITCH_THREE":
                            if(currentStep >= 5)
                            {
                                incorrectStep = 4;
                                CreateNode(AnimationArray[4, 0], AnimationArray[4, 1]);
                                Debug.Log(AnimationArray[4, 0]);
                            }
                            else
                            {
                                incorrectStep = 7;
                                CreateNode(AnimationArray[7, 0], AnimationArray[7, 1]);
                                Debug.Log(AnimationArray[7, 0]);
                            }
                            break;
                        case "SWITCH_FOUR":
                            if(currentStep >= 8)
                            {
                                incorrectStep = 7;
                                CreateNode(AnimationArray[7, 0], AnimationArray[7, 1]);
                                Debug.Log(AnimationArray[7, 0]);
                            }
                            else
                            {
                                incorrectStep = 4;
                                CreateNode(AnimationArray[4, 0], AnimationArray[4, 1]);
                                Debug.Log(AnimationArray[4, 0]);
                            }
                            break;
                        case "SWITCH_FIVE":
                            if(currentStep >= 9)
                            {
                                incorrectStep = 6;
                                CreateNode(AnimationArray[6, 0], AnimationArray[6, 1]);
                                Debug.Log(AnimationArray[6, 0]);
                            }
                            else
                            {
                                incorrectStep = 3;
                                CreateNode(AnimationArray[3, 0], AnimationArray[3, 1]);
                                Debug.Log(AnimationArray[3, 0]);
                            }
                            break;
                        case "SWITCH_SIX":
                            if(currentStep >= 10)
                            {
                                incorrectStep = 9;
                                CreateNode(AnimationArray[9, 0], AnimationArray[9, 1]);
                                Debug.Log(AnimationArray[9, 0]);
                            }
                            else
                            {
                                incorrectStep = 2;
                                CreateNode(AnimationArray[2, 0], AnimationArray[2, 1]);
                                Debug.Log(AnimationArray[2, 0]);
                            }
                            break;
                        default:
                            incorrectStep = i;
                            CreateNode(AnimationArray[i, 0], AnimationArray[i, 1]);
                            Debug.Log("not a switch");
                            break;
                    }


                    AnimationArray[incorrectStep, 2] = newValue;
                    break;
                }
            }
        }

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