using System.Collections;
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
    public GameObject theBox;
    public FloatingButton Reset;
    public GameObject Camera;
    public GameObject MainBox;
    public GameObject MainBoxDoor;
    public GameObject MainBoxPanel;
    public GameObject BoundingBox;
    public GameObject SpatialMesh;
    public GameObject BoxModel;
    bool boxStatus = true;



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
        BoxModel.SetActive(true);
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
        BoxModel.SetActive(false);

        SetVertxEventHandlerState(false);
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

        SetVertxEventHandlerState(false);
    }

    // Menu container button click event handler
    private void OnButtonClicked(GameObject button)
    {

        if (button.name == "LiveInformation")
        {
            mainMenuContainer.SetActiveStatus(false);
            windowManager.SetActive(true);
            homeButton.setActiveStatus(true);
            Reset.setActiveStatus(false);
            BoundingBox.SetActive(false);

            boxStatus = true;

            MainBoxDoor.SetActive(false);
            MainBoxPanel.SetActive(false);
            theBox.GetComponent<ObjectDecomposition>().MoveObjectsForwards();
        }
        else if (button.name == "InteractiveGuide")
        {
            StartCoroutine(LoadKeyAnimation());
            mainMenuContainer.SetActiveStatus(false);
            windowManager.SetActive(false);
            Reset.setActiveStatus(false);
            homeButton.setActiveStatus(true);
        }
    }

    // Coroutine to load first key animation
    IEnumerator LoadKeyAnimation()
    {
        Debug.Log("Load Key Animation Co routine ");

        yield return new WaitForSeconds(0.5f);
        if (SceneLink.Instance.GetComponent<FuseBoxStateManager>() != null)
        {
            SceneLink.Instance.GetComponent<FuseBoxStateManager>().enabled = true;
        }
        yield return new WaitForSeconds(0.5f);
        if (SceneLink.Instance.GetComponentInChildren<VertxEventHandler>() != null)
        {
            SceneLink.Instance.GetComponentInChildren<VertxEventHandler>().enabled = true;
        }
        if (SceneLink.Instance.GetComponentInChildren<VertxEventHandler>() != null)
        {
            SceneLink.Instance.GetComponentInChildren<VertxEventHandler>().InitKeyAnimation();
        }
    }

    // Coroutine to reset the vertx event manager
    IEnumerator ResetVertxEventHandler()
    {
        if (SceneLink.Instance.GetComponent<FuseBoxStateManager>() != null)
        {
            SceneLink.Instance.GetComponent<FuseBoxStateManager>().enabled = false;
        }
        if (SceneLink.Instance.GetComponentInChildren<VertxEventHandler>() != null)
        {
            SceneLink.Instance.GetComponentInChildren<VertxEventHandler>().enabled = false;
        }
        foreach (NodeLink a in SceneLink.Instance.GetComponentsInChildren<NodeLink>())
        {
            if(a.name != "VertxEventManager")
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
        if(SceneLink.Instance.GetComponent<FuseBoxStateManager>() != null)
        {
            SceneLink.Instance.GetComponent<FuseBoxStateManager>().enabled = state;
        }
        if(SceneLink.Instance.GetComponentInChildren<VertxEventHandler>() != null)
        {
            SceneLink.Instance.GetComponentInChildren<VertxEventHandler>().enabled = state;
        }
        if (!state)
        {
            foreach (KeyAnimEventHandler a in SceneLink.Instance.GetComponentsInChildren<KeyAnimEventHandler>())
            {
                Destroy(a.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
