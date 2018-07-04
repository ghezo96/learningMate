using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

public class OnFocus : MonoBehaviour, IFocusable
{
    //CAMERA ONFOCUS RANGE ~ 4.5m



    bool moveCube = false;


    Vector3 currentCubePos;
    Vector3 restCubePos;
    Vector3 targetVector;

    public Vector3 cubeOffset = new Vector3(1f, 1f, 1f);

    //Vector3 closeCubePos = new Vector3(0f, 0f, 10f);


    float lerpSpeed = 5f;

    bool completedLerp = false;

    //Vector3 Close;
    //Vector3 Far = new Vector3(0, 0, 6);

    //bool closeUp = false;

    public void OnFocusEnter()
    {
        InputManager.Instance.PushModalInputHandler(gameObject);

        moveCube = true;

        //Close = transform.position - CameraCache.Main.transform.position;
        Debug.Log("Focusing on " + gameObject);
        //transform.position = Close;
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    public void OnFocusExit()
    {
        InputManager.Instance.PopModalInputHandler();

        moveCube = false;


        Debug.Log("Lost foucs on  " + gameObject);
        InputManager.Instance.PopModalInputHandler();
        //transform.position = Far;
    }


    void Start()
    {
        //transform.position = new Vector3(0, 0, 6);
        restCubePos = transform.position;

    }

    void Update()
    {
        //cubeOutward = false;
        whileInFocus();
    }

    void whileInFocus()
    {
        currentCubePos = transform.position;
        Vector3 targetVector = CameraCache.Main.transform.position + cubeOffset;

        if (moveCube)
        {
            Debug.Log("Moving object");
            if(transform.position != targetVector)
            {
                transform.position = Vector3.Lerp(currentCubePos, targetVector, lerpSpeed * Time.deltaTime);
            }
            else
            {
                moveCube = false;
            }

        }
        else
        {
            Debug.Log("Moving object back");
            transform.position = Vector3.Lerp(currentCubePos, restCubePos, lerpSpeed * Time.deltaTime);
        }
    }

    IEnumerator CubeSelection()
    {

        yield return new WaitForSeconds(2);
    }
}
