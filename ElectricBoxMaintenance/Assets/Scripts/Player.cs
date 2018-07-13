using System.Collections;
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
    

    // Use this for initialization
    void Start ()
    {
        if (homeButton.isActiveAndEnabled)
        {
            homeButton.setActiveStatus(false);
        }
       
        Debug.Log("In Start");

        // create holographic buttons to get started with
        mainMenuContainer.ButtonClicked += OnButtonClicked;
        homeButton.Clicked += HomeButton_Clicked;
        liveInfo.Clicked += liveInfo_Clicked;
    }

    public void liveInfo_Clicked(GameObject button)
    {
        windowManager.SetActive(false);
        mainMenuContainer.SetActiveStatus(false);
        button.SetActive(true); 
        theBox.GetComponent<ObjectDecomposition>().ElectricBoxMovement();
    
    }
    // HomeButton click event handler
    private void HomeButton_Clicked(GameObject button)
    {
        windowManager.SetActive(false);
        mainMenuContainer.SetActiveStatus(true);
        // Hide home button
        button.SetActive(false);
    }

    private void OnButtonClicked(GameObject button)
    {
        //Debug.Log(button.name + "menu buttons");
        mainMenuContainer.SetActiveStatus(false);
        windowManager.SetActive(true);
        homeButton.setActiveStatus(true);

    }

    // Update is called once per frame
    void Update () {
       
    }

}
