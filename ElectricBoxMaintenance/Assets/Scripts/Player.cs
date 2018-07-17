﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.UX.Buttons;

public class Player : MonoBehaviour {

    
    public GameObject windowManager;
    public MainMenuContainer mainMenuContainer;
    PanelWindow[] allWindows;
    string inputTextName;
    string inputTitleText;
    string inputDescriptionText;
    public FloatingButton homeButton;
    public FloatingButton liveInfo;
    public GameObject theBox;
    public FloatingButton Reset;
    public GameObject Camera;
    public GameObject MainBox;
    public GameObject MainBoxDoor;
    public GameObject MainBoxPanel;
    public GameObject BoundingBox;
    bool boxStatus = true;
   
    

    // Use this for initialization
    void Start ()
    {
        Debug.Log("In Start");

        // create holographic buttons to get started with
        mainMenuContainer.ButtonClicked += OnButtonClicked;
        homeButton.Clicked += HomeButton_Clicked;
        liveInfo.Clicked += liveInfo_Clicked;

        Reset.Clicked += Reset_Clicked;


        if (homeButton.isActiveAndEnabled)
        {
            homeButton.setActiveStatus(false);
        }

    }

    public void Reset_Clicked (GameObject button)
    {
        Camera.GetComponent<RaycastPositioningV1>().enabled = true;
        MainBox.SetActive(false);
        BoundingBox.SetActive(true);
        Reset.setActiveStatus(false);
        homeButton.setActiveStatus(true);
        mainMenuContainer.SetActiveStatus(false);
    }
    public void liveInfo_Clicked(GameObject button)
    {
        boxStatus = true;
       
            MainBoxDoor.SetActive(false);
            MainBoxPanel.SetActive(false);
      
        
        windowManager.SetActive(false);
        mainMenuContainer.SetActiveStatus(false);
        button.SetActive(true);
        BoundingBox.SetActive(false);
        theBox.GetComponent<ObjectDecomposition>().MoveObjectsForwards();
        
    
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
        MainBox.GetComponent<Movement>().enabled = false;
        Reset.setActiveStatus(true);
    }

    private void OnButtonClicked(GameObject button)
    {
        //Debug.Log(button.name + "menu buttons");
        mainMenuContainer.SetActiveStatus(false);
        windowManager.SetActive(true);
        homeButton.setActiveStatus(true);
        Reset.setActiveStatus(false);
        BoundingBox.SetActive(false);

    }

    // Update is called once per frame
    void Update () {
       
    }

}
