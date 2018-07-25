﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.UX.Buttons;
using VertexUnityPlayer;

public class Player : MonoBehaviour {

    
    public GameObject windowManager;
    public MainMenuContainer mainMenuContainer;
    PanelWindow[] allWindows;
    string inputTextName;
    string inputTitleText;
    string inputDescriptionText;
    public FloatingButton homeButton;
    public FloatingButton StartButton;
    public GameObject theBox;
    public FloatingButton Reset;
    public GameObject Camera;
    public GameObject MainBox;
    public GameObject MainBoxDoor;
    public GameObject MainBoxPanel;
    public GameObject BoundingBox;
    public GameObject SpatialMesh;
    public GameObject BoxModel;
    public GameObject SceneLinkScriptForGuide;
    bool boxStatus = true;
    private List<string> listOfAnimationsInGuide = new List<string> { "KeyAnimation", "SWITCH_ONE", "SWITCH_TWO", "SWITCH_THREE" };
   
    

    // Use this for initialization
    void Start ()
    {
        Debug.Log("In Start");

        // create holographic buttons to get started with
        mainMenuContainer.ButtonClicked += OnButtonClicked;
        homeButton.Clicked += HomeButton_Clicked;
        StartButton.Clicked += Start_Clicked;
        SpatialMesh.SetActive(true);
        Reset.Clicked += Reset_Clicked;

        if (homeButton.isActiveAndEnabled)
        {
            homeButton.setActiveStatus(false);
        }

    }

    public void Start_Clicked(GameObject button)
    {
        StartButton.setActiveStatus(false);
        Reset.setActiveStatus(true);
        SpatialMesh.SetActive(false);
        mainMenuContainer.SetActiveStatus(true);
        MainBox.GetComponent<Movement>().enabled = false;
        BoundingBox.SetActive(false);
        BoxModel.SetActive(true);
    }

    public void Reset_Clicked (GameObject button)
    {
        Camera.GetComponent<RaycastPositioningV1>().enabled = true;
        MainBox.SetActive(false);
        BoundingBox.SetActive(true);
        Reset.setActiveStatus(false);
        StartButton.setActiveStatus(true);
        mainMenuContainer.SetActiveStatus(false);
        SpatialMesh.SetActive(true);
        BoxModel.SetActive(false);
    }

    // HomeButton click event handler
    private void HomeButton_Clicked(GameObject button)
    {
        if (boxStatus)
        {
            boxStatus = false;
            MainBoxDoor.SetActive(true);
            MainBoxPanel.SetActive(true);
        }

        windowManager.SetActive(false);
        mainMenuContainer.SetActiveStatus(true);
        // Hide home button
        button.SetActive(false);
        theBox.GetComponent<ObjectDecomposition>().MoveObjectsBackwards();
        Reset.setActiveStatus(true);
        
        //foreach (NodeLink a in SceneLink.Instance.GetComponentsInChildren<NodeLink>())
        //{
        //    if (listOfAnimationsInGuide.Contains(a.name))
        //    {
        //        //Debug.Log("a guid is: " + a.Guid);
        //        //a.gameObject.SetActive(false);
        //        //Destroy(a.gameObject);
        //        //a.gameObject.GetComponent<KeyAnimEventHandler>().DestroyIt();
        //        Destroy(a.gameObject);
        //        //a.gameObject.GetComponent<KeyAnimEventHandler>().DestroyIt();
        //        Debug.Log("Destroyed: " + a.name);
        //    }
        //    //if (a.name == "VertxEventManager")
        //    //{
        //    //    //Destroy(a.gameObject);
        //    //    a.gameObject.SetActive(false);
        //    //}

        //}
       // SceneLinkScriptForGuide.GetComponent<FuseBoxStateManager>().enabled = false;

    }

    private void OnButtonClicked(GameObject button)
    {
        //Debug.Log(button.name + "menu buttons");

        if(button.name == "LiveInformation")
        {
            mainMenuContainer.SetActiveStatus(false);
            windowManager.SetActive(true);
            homeButton.setActiveStatus(true);
            Reset.setActiveStatus(false);
            BoundingBox.SetActive(false);
            boxStatus = true;
            MainBoxDoor.SetActive(false);
            MainBoxPanel.SetActive(false);
            BoundingBox.SetActive(false);
            theBox.GetComponent<ObjectDecomposition>().MoveObjectsForwards();
        }
        else if(button.name == "InteractiveGuide")
        {
            mainMenuContainer.SetActiveStatus(false);
            windowManager.SetActive(false);
            Reset.setActiveStatus(false);
            homeButton.setActiveStatus(true);
        }
      

    }

    // Update is called once per frame
    void Update () {
       
    }

}
