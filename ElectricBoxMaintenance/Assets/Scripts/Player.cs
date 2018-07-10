using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.UX.Buttons;


public class Player : MonoBehaviour {

    public PanelWindow propertyWindow;
    public PanelWindow recordWindow;
    public PanelWindow statusWindow;

    public MainMenuContainer menuContainer;



    public int numberOfObjects = 10;

    private PanelWindow [] panelWindows;

    private 

    // Use this for initialization
    void Start () {

        Debug.Log("In Start");
        // create holographic buttons to get started with

        // Instantialte the panel window

        panelWindows = gameObject.GetComponentsInChildren<PanelWindow>();

        // setting title and description of each and every panel window

        Debug.Log("(panelWindows.Length => " + panelWindows.Length );

        foreach (PanelWindow window in panelWindows)
        {
            window.TitleText = "This is window title ";
            window.DescriptionText = "This is description text ";
        }

        // Get the floating buttons and listen for click event
        FloatingButton[] buttons = gameObject.GetComponentsInChildren<FloatingButton>();
        foreach (FloatingButton button in buttons)
        {
            button.Clicked += OnButtonClicked;
        }

        menuContainer =  gameObject.GetComponentInChildren<MainMenuContainer>();

        //menuContainer.SetActive(true);
    }

    private void OnButtonClicked(GameObject button)
    {
        Debug.Log(button.name + "menu buttons");

        menuContainer.SetActiveStatus(false);

        //if (button.name == "PropertyButton")
        //{
        //    propertyWindow.Show();
        //    propertyWindow.TitleText = "Property title window";
        //    propertyWindow.DescriptionText = "This is properlty window description";

        //} else if(button.name == "RecordButton")
        //{
        //    recordWindow.Show();
        //    recordWindow.TitleText = "Record title window";
        //    recordWindow.DescriptionText = "This is Record window description";

        //} else  if(button.name == "StatusButton")
        //{
        //    statusWindow.Show();
        //    statusWindow.TitleText = "Status title window";
        //    statusWindow.DescriptionText = "This is Status window description";
        //}
    }


    // Update is called once per frame
    void Update () {
       
    }

    private void OnDestroy()
    {
        FloatingButton[] buttons = gameObject.GetComponentsInChildren<FloatingButton>();
        foreach (FloatingButton button in buttons)
        {
            button.Clicked -= OnButtonClicked;
        }
    }
}
