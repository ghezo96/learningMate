using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class OnFocusResize : MonoBehaviour, IFocusable
{
    //Vector3 initialScale;

    float LerpDuration = 2f;
    float ScaleFactor = 1.5f;

    bool isResized = false;
    bool isResizing = false;
    bool resizeComponent = false;

    Vector3 originalScale;



    public void OnFocusEnter()
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
        resizeComponent = true;


        //transform.parent.transform.localScale = originalScale * ScaleFactor;
    }

    public void OnFocusExit()
    {
        InputManager.Instance.PopModalInputHandler();
        resizeComponent = false;






        //transform.parent.transform.localScale = originalScale;
    }


    void Start () {

        originalScale = transform.parent.transform.localScale;






        //initialScale = transform.localScale;
	}
	

	void Update () {

        Vector3 currentScale = transform.parent.transform.localScale;

        if (resizeComponent)
        {
            Vector3.Lerp(currentScale, originalScale, LerpDuration * Time.deltaTime);
        }
        else if(!resizeComponent)
        {
            Vector3.Lerp(currentScale, originalScale * ScaleFactor, LerpDuration * Time.deltaTime);
        }

    }

}
