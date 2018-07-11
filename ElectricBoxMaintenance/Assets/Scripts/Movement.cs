using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class Movement : MonoBehaviour, IInputHandler, IFocusable
{
    Vector3 initialPosition;
    Vector3 pointerPosition;
    Vector3 positionX;
    Vector3 positionY;
    Vector3 positionZ;

    bool focused = false;
    bool clicked = false;
    IInputSource inputSource;
    uint sourceID;

    public void OnFocusEnter()
    {
        focused = true;       
    }

    public void OnFocusExit()
    {
        focused = false;        
    }
     
    public void OnInputDown(InputEventData eventData)
    {
        clicked = true;
        sourceID = eventData.SourceId;        
        inputSource = eventData.InputSource;       
        initialPosition = gameObject.transform.position;
    }

    public void OnInputUp(InputEventData eventData)
    {
        clicked = false;
        inputSource = null;
    }

    // Use this for initialization
    void Start()
    {
        positionZ.z = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    Vector3 previousPosition;

    public void Move()
    {
        if (inputSource != null)
        {
            if (gameObject.tag == "Player" &&  clicked && inputSource.TryGetGripPosition(sourceID, out pointerPosition))
            {
                pointerPosition = new Vector3(pointerPosition.x, pointerPosition.y, positionZ.z);
                previousPosition = new Vector3(previousPosition.x, previousPosition.y, positionZ.z);
                Vector3 handMovementDirection = pointerPosition - previousPosition;
                previousPosition = pointerPosition;
                gameObject.transform.position += handMovementDirection;
               
            }
        }
    }
}
