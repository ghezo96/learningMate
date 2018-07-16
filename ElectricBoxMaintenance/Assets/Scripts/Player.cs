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

    private AudioSource AudioSource;
    

    // Use this for initialization
    void Start ()
    {
        Debug.Log("In Start");

        AudioSource = gameObject.GetComponent<AudioSource>();

        // create holographic buttons to get started with
        mainMenuContainer.ButtonClicked += OnButtonClicked;
        homeButton.Clicked += HomeButton_Clicked;
        liveInfo.Clicked += liveInfo_Clicked;

        if (homeButton.isActiveAndEnabled)
        {
            homeButton.setActiveStatus(false);
        }
    }

    public void liveInfo_Clicked(GameObject button)
    {
        windowManager.SetActive(false);
        mainMenuContainer.SetActiveStatus(false);
        button.SetActive(true); 
        theBox.GetComponent<ObjectDecomposition>().MoveObjectsForwards();
    
    }
    // HomeButton click event handler
    private void HomeButton_Clicked(GameObject button)
    {
        windowManager.SetActive(false);
        mainMenuContainer.SetActiveStatus(true);
        // Hide home button
        button.SetActive(false);
        theBox.GetComponent<ObjectDecomposition>().MoveObjectsBackwards();
    }

    private void OnButtonClicked(GameObject button)
    {
        //Debug.Log(button.name + "menu buttons");
        mainMenuContainer.SetActiveStatus(false);
        windowManager.SetActive(true);
        homeButton.setActiveStatus(true);
        AudioSource.PlayOneShot(AudioSource.clip);

    }

    // Update is called once per frame
    void Update () {
       
    }

}
