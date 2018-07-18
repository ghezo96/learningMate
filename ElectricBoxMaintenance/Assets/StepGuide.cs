using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;


public class StepGuide : MonoBehaviour, IFocusable, IInputClickHandler  {

    public void OnFocusEnter()
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    public void OnFocusExit()
    {
        InputManager.Instance.PopModalInputHandler();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
