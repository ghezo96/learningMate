﻿using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

public class TheVertxEventHandler : MonoBehaviour {

    public string[,,] AnimationArray;

    GameObject PreviousAnimationNode;

    //DOOR_CLOSE ANIMATION : b4f004f9-4f04-4e74-9603-a0a695778f7d

    int currentIndex;
    bool isComplete;
    int numberOfCompletedSteps;
    bool flipSwitches;

    int debugCounter = 0;
    int switchReverseCounter = 0;

    // Use this for initialization
    void Start () {

        isComplete = false;
        flipSwitches = false;

        AnimationArray = new string[,,]
        {
            {
                {"KEY_ANIMATION","62fe3789-6dc0-4be8-8de4-daf6be186bed", "0"},
                {"DOOR_ANIMATION","fe72fbeb-7341-40ec-91a9-7d4387bf9ff6", "0"},
                {"SWITCH_ONE","59d89c08-85a6-4e17-9edb-a2b648d0503e", "0" },
                {"SWITCH_TWO","3fe496cd-7cf7-44d8-8388-74ac49e14986", "0"},
                {"SWITCH_THREE","9d0db931-1698-47af-8080-3106cfede727", "0"},
                //SAME SWITCHES USED
                {"SWITCH_FOUR","a3418e0a-a9a3-4ee2-ada9-5eb0da5a0d29", "0"},
                {"SWITCH_FIVE","30fc82a8-7b3b-4cab-afb9-c8304a6c756d", "0"},
                {"SWITCH_SIX","3930543b-229a-4f82-804a-90cd09eca5a5", "0"},
            }
        };

        // Create key animation node if it doesn't exist
        if (!(SceneLink.Instance.transform.Find("KEY_ANIMATION")))
        {
            CreateNode("KEY_ANIMATION", "62fe3789-6dc0-4be8-8de4-daf6be186bed");
        }
        else
        {
            //Debug.Log("Starting key animation already exists");
            PreviousAnimationNode = SceneLink.Instance.transform.Find("KEY_ANIMATION").gameObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnUpdate(object message)
    {
        debugCounter++;




        //Destory previous animation
        DestroyImmediate(PreviousAnimationNode);

        // Deserialize the message received by IOT
        Message _message = JsonConvert.DeserializeObject<Message>(message.ToString());


        string componentName = _message.name;
        string componentState = _message.state.ToString();




        if (componentName == "SWITCH_ONE" && AnimationArray[0, 2, 2] == "1" && AnimationArray[0, 3, 2] == "1" && AnimationArray[0, 4, 2] == "1")
        {
            AnimationArray[0, 2, 2] = "0";
            AnimationArray[0, 3, 2] = "0";
            AnimationArray[0, 4, 2] = "0";

            flipSwitches = true;
        }

        if((componentName == "SWITCH_ONE" || componentName == "SWITCH_TWO" || componentName == "SWITCH_THREE") && flipSwitches)
        {
            if(AnimationArray[0, 2, 2] == "1" && AnimationArray[0, 3, 2] == "1" && AnimationArray[0, 4, 2] == "1")
            {
                flipSwitches = false;
            }
            else
            {
                //switch (componentState)
                //{
                //    case "0":
                //        componentState = "1";
                //        break;
                //    case "1":
                //        componentState = "0";
                //        break;
                //}
            }
        }





        // Update array
        UpdateComponentArray(componentState);

        //if (componentName == "SWITCH_ONE" || componentName == "SWITCH_TWO" || componentName == "SWITCH_THREE"
        //&& AnimationArray[0, 0, 0] == "1" && AnimationArray[0, 1, 0] == "1" && AnimationArray[0, 2, 0] == "1"
        //&& AnimationArray[0, 3, 0] == "1" && AnimationArray[0, 4, 0] == "1")
        //{
        //    if (switchReverseCounter != 3)
        //    {
        //        switch (componentState)
        //        {
        //            case "0":
        //                componentState = "1";
        //                break;
        //            case "1":
        //                componentState = "0";
        //                break;
        //        }

        //        switchReverseCounter++;
        //    }
        //    else
        //    {
        //        switchReverseCounter = 0;
        //    }

        //}


        // index for which animation to play
        currentIndex = FindNewIndex(componentName, componentState, "CurrentStep");

        CheckLastStepCompleted(componentName, componentState);


        if(!isComplete)
        {

            //Debug.Log("StartStep => " + currentIndex);

            //print table before update
            Debug.Log("<== BEFORE UPDATE ==>");
            FindNewIndex(componentName, componentState, "Table");

            //bring table after update
            Debug.Log("<== AFTER UPDATE ==>");
            FindNewIndex(componentName, componentState, "Table");

            // Play animations
            ExecuteAnimation(componentName, componentState);
            //Debug.Log("FinishStep => " + currentIndex);
        }
        else
        {
            PreviousAnimationNode = CreateNode("MAINTANENCE_MODEL", "2359e967-bcd4-44ab-b58f-6f7a8ca391e4");
        }

    }

 
    //void UpdateComponentArray(string _ComponentName, string _ComponentStatus)
    //{
    //    for (int i = 0; i < (AnimationArray.Length / 3) - 1; i++)
    //    {
    //        if (_ComponentName == AnimationArray[0, i, 0])
    //        {
    //            AnimationArray[0, i, 2] = _ComponentStatus;
    //            break;
    //        }
    //    }
    //}

    //void ExecuteAnimation(string _ComponentName, string _ComponentStatus)
    //{
    //    for (int i = 0; i < (AnimationArray.Length / 3) - 1; i++)
    //    {
    //        if (AnimationArray[0,i,0] == _ComponentName)
    //        {

    //            if (AnimationArray[0, i, 2] == "0")
    //            {
    //                Debug.Log("DEBUG");
    //                if(_ComponentName == "KEY_ANIMATION")
    //                {
    //                    Debug.Log("Trying to create "  + _ComponentName);
    //                    CreateNode("KEY_ANIMATION", "62fe3789-6dc0-4be8-8de4-daf6be186bed");
    //                }
    //                else
    //                {
    //                    PreviousAnimationNode = CreateNode(AnimationArray[0, i, 0], AnimationArray[0, i, 1]);
    //                    CurrentStep--;
    //                }
    //                break;
    //            }
    //            if(AnimationArray[0, i, 2] == "1")
    //            {
    //                PreviousAnimationNode = CreateNode(AnimationArray[0, i + 1, 0], AnimationArray[0, i + 1, 1]);
    //                CurrentStep++;
    //            }
    //            break;
    //        }
    //    }
    //}


    void CheckLastStepCompleted(string _ComponentName, string _ComponentStatus)
    {
        int numberOfCompletedSteps = 0;

        //last step has index of 7
        if (currentIndex == 7 && AnimationArray[0, currentIndex, 2] == "1")
        {
            numberOfCompletedSteps = FindNewIndex(_ComponentName, _ComponentStatus, "State1");
        }

        switch(numberOfCompletedSteps)
        {
            case 7:
                isComplete = true;
                break;
            default:
                currentIndex = FindNewIndex(_ComponentName, _ComponentStatus, "CurrentStep");
                break;
        }
    }

    void UpdateComponentArray(string _ComponentStatus)
    {
        //Debug.Log("CurrentStep => " + currentIndex);
        AnimationArray[0, currentIndex, 2] = _ComponentStatus;
    }


    //decides what animation to run
    void ExecuteAnimation(string _ComponentName, string _ComponentStatus)
    {
        //Debug.Log("CurrentStep => " + currentIndex);
        //if key animation (DOOR ANIMATION) component is not called
        if (_ComponentName != "DOOR_CLOSE")
        {
            if(_ComponentStatus == "0")
            {
                currentIndex = FindNewIndex(_ComponentName, _ComponentStatus, "Name");
            }

            //switch (_ComponentStatus)
            //{
            //    //next animation depends on what component is switched to incorrect state
            //    case "0":
            //        currentIndex = FindNewIndex(_ComponentName, _ComponentStatus, "Name");
            //        break;
            //    //next animation depends on what status in our array is still incorrect
            //    default:
            //        break;
            //}

            //creates node for that index
            CreateNode(AnimationArray[0, currentIndex, 0], AnimationArray[0, currentIndex, 1]);
        }
        //section for key animation (DOOR ANIMATION) component being changed 
        else
        {
            //DOOR ANIMATION PLAYED ALWAYS IF CLOSED
            if (_ComponentStatus == "0")
            {
                CreateNode("DOOR_OPEN", "b4f004f9-4f04-4e74-9603-a0a695778f7d");
            }
            else
            {

                CreateNode(AnimationArray[0, currentIndex, 0], AnimationArray[0, currentIndex, 1]);

                //recalculate index value to start from where we left

                //// currentIndex == STEP NUMBER if stepnumbe = 0 then outcome is step
                //switch (currentIndex)
                //{
                //    case 0:
                //        CreateNode(AnimationArray[0, currentIndex + 1, 0], AnimationArray[0, currentIndex + 1, 1]);
                //        break;

                //    default:
                //        CreateNode(AnimationArray[0, currentIndex, 0], AnimationArray[0, currentIndex, 1]);
                //        break;
                //}
                //currentIndex = FindNewIndex(_ComponentName, _ComponentStatus, "Status");
            }

        }

    }

    int FindNewIndex(string _ComponentName, string _ComponentStatus, string searchBy)
    {
        int index;

        for (index = 0; index < (AnimationArray.Length / 3); index++)
        {
            switch (searchBy)
            {
                case "Name":
                    if (AnimationArray[0, index, 0] == _ComponentName)
                    {
                        return index;
                    }
                    break;

                case "CurrentStep":
                    if (AnimationArray[0, index, 2] == "0")
                    {
                        return index;
                    }
                    break;
                case "State1":
                    if(AnimationArray[0, index, 2] == "1")
                    {
                        numberOfCompletedSteps++;
                    }
                    break;
                case "Table":
                    {
                        Debug.Log(
                            debugCounter + " " +
                            AnimationArray[0, index, 0] + " " + 
                            AnimationArray[0, index, 1] + " " +
                            AnimationArray[0, index, 2]
                            );
                        break;
                    }
            }
        }

        if(searchBy == "State1")
        {
            return numberOfCompletedSteps;
        }
        else
        {
            return index;
        }
    }

    private GameObject CreateNode(string name, string id)
    {
        var vertxObject = SceneLink.Instance.transform.Find(name);
        GameObject vertxThing;

        if (vertxObject == null)
        {
            //Debug.Log(name + " node created");
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
            //Debug.Log(name + " node already exists");

        }

        PreviousAnimationNode = SceneLink.Instance.transform.Find(name).gameObject;
        return vertxThing;

    }
}
