﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelWindow : MonoBehaviour {

    //close button
    private HoloToolkit.Unity.InputModule.Tests.TestButton closeButton;

    private PanelDescription panelDescription;
    // var to hold the panel width
    public float PanelWidth;
    // var to hold the panel height
    public float PanelHeight;

    [SerializeField]
    private string titleText;

    [SerializeField]
    private string descriptionText;


    void Start () {

        //Get the panel description container and set the description
        panelDescription = gameObject.GetComponentInChildren<PanelDescription>();
        panelDescription.setTitle(TitleText);
        panelDescription.setDescription(DescriptionText);

        // Get the close button and listen for close events
        closeButton = gameObject.GetComponentInChildren<HoloToolkit.Unity.InputModule.Tests.TestButton>();
        closeButton.Activated += CloseButton_Activated;

    }

    // panel close button handler
    private void CloseButton_Activated(HoloToolkit.Unity.InputModule.Tests.TestButton source)
    {
        Hide();
    }

    // hide panel windows
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // show panel window
    public void Show()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public string DescriptionText
    {
        get
        {
            return descriptionText;
        }

        set
        {
            descriptionText = value;
            
           
        }
    }

    public string TitleText
    {
        get
        {
            return titleText;
        }

        set
        {
            titleText = value;
           
        }
    }

}