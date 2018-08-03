﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.UX.Buttons;
using VertexUnityPlayer;

public class Player : MonoBehaviour
{
    public GameObject windowManager;
    public MainMenuContainer mainMenuContainer;
    public FloatingButton homeButton;
    public FloatingButton StartButton;
    public GameObject WholeBox;
    public FloatingButton Reset;
    public GameObject Camera;
    public GameObject MainBox;
    public GameObject MainBoxDoor;
    public GameObject MainBoxPanel;
    public GameObject BoundingBox;
    public GameObject SpatialMesh;
    public GameObject sceneLink;
    bool boxStatus = true;
    bool inDecomp = false;



    // Use this for initialization
    void Start()
    {
        Debug.Log("In Start");

        // create holographic buttons to get started with
        mainMenuContainer.ButtonClicked += OnButtonClicked;
        homeButton.Clicked += HomeButton_Clicked;
        StartButton.Clicked += Start_Clicked;
        Reset.Clicked += Reset_Clicked;

        if (homeButton.isActiveAndEnabled)
        {
            homeButton.setActiveStatus(false);
        }
        SceneLink.Instance.OnStateChange += SceneLink_OnStateChange;
    }

    // On scene connect, Handler is set up
    private void SceneLink_OnStateChange(SceneLinkStatus oldState, SceneLinkStatus newState)
    {
        Debug.Log("SceneLink_OnStateChange - VERTX connected : " + newState);
        if (newState == SceneLinkStatus.Connected)
        {
            Debug.Log("SceneLink_OnStateChange - VERTX connected : ");
            StartCoroutine(ResetVertxEventHandler());
        }

    }
    // Start button click handler
    public void Start_Clicked(GameObject button)
    {
        StartButton.setActiveStatus(false);
        Reset.setActiveStatus(true);
        SpatialMesh.SetActive(false);
        mainMenuContainer.SetActiveStatus(true);
        MainBox.GetComponent<Movement>().enabled = false;
        BoundingBox.SetActive(false);
        WholeBox.SetActive(false);
    }
    // Reset button click handler
    public void Reset_Clicked(GameObject button)
    {
        Camera.GetComponent<RaycastPositioningV1>().enabled = true;
        MainBox.SetActive(false);
        BoundingBox.SetActive(true);
        Reset.setActiveStatus(false);
        StartButton.setActiveStatus(true);
        mainMenuContainer.SetActiveStatus(false);
        SpatialMesh.SetActive(true);

        SetVertxEventHandlerState(false);
    }

    // HomeButton click event handler
    private void HomeButton_Clicked(GameObject button)
    {
        button.SetActive(false);
        if(inDecomp)
        {
            StartCoroutine(GoToHome());
        }
        else
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

            WholeBox.SetActive(false);
            Reset.setActiveStatus(true);

            SetVertxEventHandlerState(false);
            // REmove CollabVertxObjectHAndler 
            SceneLink.Instance.GetComponent<FuseBoxStateManager>().RemoveCollabVertxObjectHandler();
            sceneLink.GetComponent<SwitchAndConnectorNode>().enabled = false;
           MainBox.GetComponent<BoxCollider>().enabled = true;
        }

    }

    IEnumerator GoToHome()
    {
        WholeBox.GetComponent<ObjectDecomposition>().MoveObjectsBackwards();
        yield return new WaitForSeconds(1.0f);

        if (boxStatus)
        {
            boxStatus = false;
            MainBoxDoor.SetActive(true);
            MainBoxPanel.SetActive(true);
        }
        windowManager.SetActive(false);
        mainMenuContainer.SetActiveStatus(true);
        // Hide home button
        WholeBox.SetActive(false);
        Reset.setActiveStatus(true);

        SetVertxEventHandlerState(false);
        inDecomp = false;
        sceneLink.GetComponent<SwitchAndConnectorNode>().enabled = true;
    }

    // Menu container button click event handler
    private void OnButtonClicked(GameObject button)
    {

        if (button.name == "LiveInformation")
        {

            WholeBox.SetActive(true);
            mainMenuContainer.SetActiveStatus(false);
            windowManager.SetActive(true);
            homeButton.setActiveStatus(true);
            Reset.setActiveStatus(false);
            BoundingBox.SetActive(false);

            boxStatus = true;
            MainBoxDoor.SetActive(false);
            MainBoxPanel.SetActive(false);

            WholeBox.GetComponent<ObjectDecomposition>().MoveObjectsForwards();
            inDecomp = true;
        }
        else if (button.name == "InteractiveGuide")
        {
            WholeBox.SetActive(false);
            StartCoroutine(LoadKeyAnimation());
            mainMenuContainer.SetActiveStatus(false);
            windowManager.SetActive(false);
            Reset.setActiveStatus(false);
            homeButton.setActiveStatus(true);
            
        }
        else if(button.name == "Collab")
        {
            StartCoroutine(EnableIoTListeners(false));
            WholeBox.SetActive(false);
            mainMenuContainer.SetActiveStatus(false);
            windowManager.SetActive(false);
            Reset.setActiveStatus(false);
            homeButton.setActiveStatus(true);
            //StartCoroutine(StartCollaberation());
            // Attach CollabVertxObjectHAndler
            SceneLink.Instance.GetComponent<FuseBoxStateManager>().CreateCollabVertxObjectHandler();
            sceneLink.GetComponent<SwitchAndConnectorNode>().enabled = true;
            MainBox.GetComponent<BoxCollider>().enabled = false;
        }
    }

    // Coroutine to start loading assets from vertx

    IEnumerator StartCollaberation()
    {
        yield return new WaitForSeconds(0.5f);
        // Disable IoT component attached to the SceneLink
       
    }

    // Coroutine to load first key animation
    IEnumerator LoadKeyAnimation()
    {
        Debug.Log("Load Key Animation Co routine ");

        yield return new WaitForSeconds(1.0f);
        //EnableIoTListeners(true);
        if (SceneLink.Instance.GetComponent<FuseBoxStateManager>() != null)
        {
            SceneLink.Instance.GetComponent<FuseBoxStateManager>().enabled = true;
        }
        yield return new WaitForSeconds(1.0f);
        if (SceneLink.Instance.GetComponentInChildren<VertxEventHandler>() != null)
        {

           // SceneLink.Instance.GetComponentInChildren<VertxEventHandler>().gameObject.SetActive(true);
            SceneLink.Instance.GetComponentInChildren<VertxEventHandler>().enabled = true;
            SceneLink.Instance.GetComponentInChildren<VertxEventHandler>().setIoTEnabled(true);
        }
        if (SceneLink.Instance.GetComponentInChildren<VertxEventHandler>() != null)
        {
            SceneLink.Instance.GetComponentInChildren<VertxEventHandler>().InitKeyAnimation();
        }
    }

    // Coroutine to reset the vertx event manager
    IEnumerator ResetVertxEventHandler()
    {
        EnableIoTListeners(false);
        foreach (NodeLink a in SceneLink.Instance.GetComponentsInChildren<NodeLink>())
        {
            if(a.name != "IoTVertxEventManager")
            {
                Debug.Log("Destroying object :" + a.name);
                Destroy(a.gameObject);
               
            }

        }
        yield return null;
    }


    // Enable / Disable VertxEventHandler
    private void SetVertxEventHandlerState(bool state)
    {
        StartCoroutine(EnableIoTListeners(state));
        if (!state)
        {
            foreach (AnimEventHandler a in SceneLink.Instance.GetComponentsInChildren<AnimEventHandler>())
            {
                Destroy(a.gameObject);
            }
        }
    }

    IEnumerator EnableIoTListeners(bool isEnabled)
    {
        if (SceneLink.Instance.GetComponent<FuseBoxStateManager>() != null)
        {
            SceneLink.Instance.GetComponent<FuseBoxStateManager>().enabled = isEnabled;
        }
        yield return new WaitForSeconds(1.0f);
        if (SceneLink.Instance.GetComponentInChildren<VertxEventHandler>() != null)
        {
            //SceneLink.Instance.GetComponentInChildren<VertxEventHandler>().gameObject.SetActive(isEnabled);
            //VertxEventHandler.IoTEnabled
            SceneLink.Instance.GetComponentInChildren<VertxEventHandler>().setIoTEnabled(isEnabled);
            SceneLink.Instance.GetComponentInChildren<VertxEventHandler>().enabled = isEnabled;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
