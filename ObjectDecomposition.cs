using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ObjectDecomposition : MonoBehaviour, IInputHandler {

    public Transform[] ObjectList;
    public float[] ObjectXValues;

    [Header("Timing variables")]
    public float LerpDuration = 1f;
    [Header("Spatial variables")]
    public float MaxDistance = 1f;
    public AnimationCurve MovementCurve;

    private bool ExtendedObjects = false;

    public void OnInputDown(InputEventData eventData)
    {
        if(!ExtendedObjects)
        {
            MoveObjectsForwards();
        }
        else
        {
            MoveObjectsBackwards();
        }
        eventData.Use();

    }

    public void OnInputUp(InputEventData eventData)
    {
        eventData.Use();
    }

    void MoveObjectsForwards()
    {
        StopAllCoroutines();
        for (int i = 0; i < ObjectList.Length; i++)
        {
            float offset = MaxDistance * i / (float)ObjectList.Length;
            StartCoroutine(MoveObject(ObjectList[i], ObjectList[i].localPosition, offset, false));
        }
    }
    
    void MoveObjectsBackwards()
    {
        StopAllCoroutines();
        for (int i = 0; i < ObjectList.Length; i++)
        {
            StartCoroutine(MoveObject(ObjectList[i], ObjectList[i].localPosition, ObjectXValues[i], true));
        }
    }


    IEnumerator MoveObject(Transform movingObject, Vector3 startPosition, float endXValue, bool reverse)
    {
        float startTime = Time.time;
        while(Time.time - startTime <= LerpDuration)
        {
            float percentageComplete = (Time.time - startTime)/ LerpDuration;
            float curveValue = MovementCurve.Evaluate(percentageComplete);
        
            float targetXValue = 0f;
            if (reverse)
            {
                targetXValue = (1f - curveValue) * (endXValue + startPosition.x);
            }
            else
            {
                targetXValue = curveValue * (endXValue + startPosition.x);
            }
            
        
            Vector3 targetPosition = startPosition;
            targetPosition.x = targetXValue;
        
            movingObject.localPosition = targetPosition;
            yield return null;
        }

        Vector3 endPosition = startPosition;
        endPosition.x = endXValue;
        movingObject.localPosition = endPosition;
       
        if (reverse)
        {
            ExtendedObjects = false;
        }
        else
        {
            ExtendedObjects = true;
        }
        yield return null;
    }

    void InitialiseArrays()
    {
        ObjectList = new Transform[transform.childCount];
        ObjectXValues = new float[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            ObjectList[i] = child;
            ObjectXValues[i] = child.position.x;
            i++;
        }
    }

    void Start()
    {
        InitialiseArrays();
    }


}
