using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDecompositionV2 : MonoBehaviour {

    public GameObject ElectricBox;

    public bool Extend = false;
    public Transform[] ObjectList;
    public Vector3[] InitialPositions;

    private bool MovingPhase;
    public int MaxDistance;


    public float LerpDuration = 1f;
    public AnimationCurve MovementCurve;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitialiseArrays()
    {
        ObjectList = new Transform[ElectricBox.transform.childCount];
        InitialPositions = new Vector3[ElectricBox.transform.childCount];
        int i = 0;
        foreach (Transform child in ElectricBox.transform)
        {
            ObjectList[i] = child;
            InitialPositions[i] = child.position;
            i++;
        }
    }

    public void Extension()
    {
        if (!Extend)
        {
            Extend = true;
        }
        else
        {
            Extend = false;
        }
    }

    void MoveObjectsForwards()
    {
        StopAllCoroutines();
        for (int i = 0; i < ObjectList.Length; i++)
        {
            float offset = MaxDistance * i / ((float)ObjectList.Length - 1);
            Debug.Log(Vector3.right * offset);
            StartCoroutine(MoveObject(ObjectList[i], InitialPositions[i], InitialPositions[i] + Vector3.right * offset, false));
        }
    }

    IEnumerator MoveObject(Transform movingObject, Vector3 startPosition, Vector3 endPosition, bool reverse)
    {
        MovingPhase = true;
        float startTime = Time.time;
        while (Time.time - startTime <= LerpDuration)
        {
            float percentageComplete = (Time.time - startTime) / LerpDuration;
            float curveValue = MovementCurve.Evaluate(percentageComplete);

            float targetZvalue = 0f;

            if (reverse)
            {
                targetZvalue = curveValue * (endPosition.z - startPosition.z);
            }
            else
            {
                targetZvalue = curveValue * (endPosition.z);
            }

            Vector3 targetPosition = startPosition + transform.forward * targetZvalue;
            movingObject.position = targetPosition;
            yield return null;
        }
        MovingPhase = false;
        yield return null;
    }
}
