using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.UX.Buttons;
using VertexUnityPlayer;

public class Player : VertexSingleton<Player> {



    public GameObject windowManager;
    public MainMenuContainer mainMenuContainer;
    PanelWindow[] allWindows;
    string inputTextName;
    string inputTitleText;
    string inputDescriptionText;
    public FloatingButton homeButton;
    public FloatingButton liveInfo;
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

    /////////////////////////////////////////////////////////////
    public FloatingButton ValidateButton;


    public delegate void OnValidateButtonClicked();
    public event OnValidateButtonClicked OnValidateClicked;

    // Use this for initialization
    void Start ()
    {
        Debug.Log("In Start");

        // create holographic buttons to get started with
        mainMenuContainer.ButtonClicked += OnButtonClicked;
        homeButton.Clicked += HomeButton_Clicked;
        liveInfo.Clicked += liveInfo_Clicked;
        StartButton.Clicked += Start_Clicked;
        ValidateButton.Clicked += Validate_Clicked;

        Reset.Clicked += Reset_Clicked;


        if (homeButton.isActiveAndEnabled)
        {
            homeButton.setActiveStatus(false);
        }

        //Renderer Render = GetComponent<Renderer>();
        //Render.material
    }

    public void Validate_Clicked(GameObject button)
    {
        if(CreateWires.CorrectWireCount == 5 && CreateWires.IncorrectWireCount == 0)
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
            Reset.setActiveStatus(true);
        }
        else
        {
            if(OnValidateClicked != null)
            {
                OnValidateClicked.Invoke();
                StopCoroutine(ShowBadWires());
                StartCoroutine(ShowBadWires());
            }
        }

    }

    IEnumerator ShowBadWires()
    {
        while (true)
        {
            for (int i = 5; i < CreateWires.IncorrectWireCount + 5; i++)
            {
                if (CreateWires.ConnectionArray[i, 3] == "1")
                {
                    GameObject badWire;
                    string name = CreateWires.ConnectionArray[i, 0] + CreateWires.ConnectionArray[i, 1];
                    string nameReverse = CreateWires.ConnectionArray[i, 1] + CreateWires.ConnectionArray[i, 0];
                    if (SceneLink.Instance.transform.Find(name))
                    {
                        badWire = SceneLink.Instance.transform.Find(name).gameObject;
                        RecurrsionSearch(badWire);
                    }
                    else if (SceneLink.Instance.transform.Find(nameReverse))
                    {
                        badWire = SceneLink.Instance.transform.Find(nameReverse).gameObject;
                        RecurrsionSearch(badWire);
                    }
                    else
                    {
                        Debug.Log("bad wire not found");
                    }
                }
            }
            yield return null;
        }
    }

    void RecurrsionSearch(GameObject gameObject)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject childObject = gameObject.transform.GetChild(i).gameObject;
            if (childObject.name == "Primitive")
            {
                Renderer renderer = childObject.GetComponent<Renderer>();
                Material mat = renderer.material;
                float emision = Mathf.PingPong(Time.time * 0.25f, 0.5f);
                Color baseColour = Color.red;
                Color finalColour = baseColour * Mathf.LinearToGammaSpace(emision);
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", finalColour);
            }
            else
            {
                RecurrsionSearch(childObject);
            }
        }
    }

        /////////////////////////////////////////////////////////////

    public void Start_Clicked(GameObject button)
    {
        StartButton.setActiveStatus(false);
        Reset.setActiveStatus(true);
        SpatialMesh.SetActive(false);
        mainMenuContainer.SetActiveStatus(true);
        MainBox.GetComponent<Movement>().enabled = false;
        BoundingBox.SetActive(false);
        BoxModel.SetActive(true);
        ValidateButton.setActiveStatus(false);
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
        ValidateButton.setActiveStatus(false);
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
        ValidateButton.setActiveStatus(false);

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
        ValidateButton.setActiveStatus(false);
    }

    private void OnButtonClicked(GameObject button)
    {
        //Debug.Log(button.name + "menu buttons");
        mainMenuContainer.SetActiveStatus(false);
        windowManager.SetActive(true);
        homeButton.setActiveStatus(true);
        Reset.setActiveStatus(false);
        BoundingBox.SetActive(false);
        ValidateButton.setActiveStatus(false);
    }

    // Update is called once per frame
    void Update () {
       
    }

}
