using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;


public class VertxEventHandler : MonoBehaviour
{

    GameObject currentGameObject;
    public string[,,] AnimationArray;

    GameObject PreviousAnimationNode;
    int currentStep;

    public bool IoTEnabled { get; set; }


    // Use this for initialization
    void Start()
    {
        currentStep = 0;
        AnimationArray = new string[,,]
        {
            {
                {"KEY_ANIMATION","62fe3789-6dc0-4be8-8de4-daf6be186bed", "1"},
                {"DOOR_ANIMATION","fe72fbeb-7341-40ec-91a9-7d4387bf9ff6", "1"},
                {"SWITCH_ONE","59d89c08-85a6-4e17-9edb-a2b648d0503e", "0" },
                {"SWITCH_TWO","3fe496cd-7cf7-44d8-8388-74ac49e14986", "1"},
                {"SWITCH_THREE","b9ed5bee-0f83-44ad-b9c8-943cfccfbbef", "0"},
                {"FUSE_ANIMATION","55436d56-7f92-44ad-9d57-4c27d2a60b05", "0" },
                {"FUSE_ANIMATION_CLOSE","e6b3428a-e40c-402e-adc9-419b459b137c", "1" },
                {"SWITCH_FOUR","2fa79861-9ec8-423f-a113-e80a2c04739e", "1"},
                {"SWITCH_FIVE","30fc82a8-7b3b-4cab-afb9-c8304a6c756d", "0"},
                {"SWITCH_SIX","3930543b-229a-4f82-804a-90cd09eca5a5", "1"},
                {"DOOR_FINISH","b4f004f9-4f04-4e74-9603-a0a695778f7d", "0"},
                {"KEY_ANIMATION_FINISH","813d6861-2370-419d-951d-9fb71312c492","0"}
            }
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

        string componentName = _message.name;
        string componentState = _message.state.ToString();
        Debug.Log("componentName : " + componentName);
        if (!IoTEnabled)
        {
            return;
        }
        // Deserialize the message received by IOT
        if (PreviousAnimationNode)
        {
            DestroyImmediate(PreviousAnimationNode);
        }

        Debug.Log("Component name => " + componentName);
        Debug.Log("Component state => " + componentState);

        //SAME SWITCHES USED
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
            ExecuteAnimation(componentName, componentState);
        }
        else
        {
            PreviousAnimationNode = CreateNode("MAINTANENCE_MODEL", "2359e967-bcd4-44ab-b58f-6f7a8ca391e4");
        }

        //if (componentName == "DOOR_ANIMATION")
        //{
        //    Debug.Log("Found door!");
        //    GameObject.Find("UXHandler").transform.FindChild("Box").transform.FindChild("ElectricityWires").gameObject.SetActive(true);
        //}
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
        if (_ComponentName == AnimationArray[0, currentStep, 0] && _ComponentStatus == AnimationArray[0, currentStep, 2])
        {
            CreateNode(AnimationArray[0, currentStep + 1, 0], AnimationArray[0, currentStep + 1, 1]);
            currentStep++;
        }
        else
        {
            CreateNode(AnimationArray[0, currentStep, 0], AnimationArray[0, currentStep, 1]);
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