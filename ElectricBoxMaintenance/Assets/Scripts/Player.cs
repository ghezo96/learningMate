using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.UX.Buttons;


public class Player : MonoBehaviour {

    
    public GameObject windowManager;
    public MainMenuContainer mainMenuContainer;
    public ObjectDecomposition boxDecomp;


    // Use this for initialization
    void Start () {

        Debug.Log("In Start");
        // create holographic buttons to get started with
        mainMenuContainer.ButtonClicked += OnButtonClicked;


    }

    private void OnButtonClicked(GameObject button)
    {
        Debug.Log(button.name + "menu buttons");

        mainMenuContainer.SetActiveStatus(false);
        windowManager.SetActive(true);
        // play the decomposition 
        boxDecomp.ElectricBoxMovement();
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
