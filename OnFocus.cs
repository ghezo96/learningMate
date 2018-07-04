using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

public class OnFocus : MonoBehaviour, IFocusable
{

    Vector3 Close;
    Vector3 Far = new Vector3(0, 0, 6);

    bool closeUp = false;

    public void OnFocusEnter()
    {
        Close = transform.position - CameraCache.Main.transform.position;
        Debug.Log("Focus entered with " + gameObject);
        transform.position = Close;
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    public void OnFocusExit()
    {
        Debug.Log("Focus exited with " + gameObject);
        InputManager.Instance.PopModalInputHandler();
        transform.position = Far;
    }


    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(0, 0, 6);
    }

    // Update is called once per frame
    void Update()
    {
        //cubeOutward = false;
    }


}
