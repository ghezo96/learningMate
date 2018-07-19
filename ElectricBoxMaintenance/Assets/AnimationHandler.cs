using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour {


    public GameObject DoubleArrow;
    public GameObject HandWithKey;

    enum Steps
    {
        Step_1 = 0,
        Step_2 = 1,
        Step_3 = 2,
        Step_4 = 3,
        Step_5 = 4,
        Step_6 = 5,
        Step_7 = 6,
        Step_8 = 7,
        Step_9 = 8,
        Step_10 = 9,
        Step_11 = 10,
        Step_12 = 11,
        Step_13 = 12

    }

    void Step_1()
    {
        //Get Object references
        DoubleArrow = transform.GetChild((int)Steps.Step_1).Find("DoubleArrow").gameObject;
        HandWithKey = transform.GetChild((int)Steps.Step_1).Find("HandWithKey").gameObject;


        //turn on step1
        transform.GetChild((int)Steps.Step_1).gameObject.SetActive(true);

    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Movement(GameObject chosenObject,  float animationTime)
    {
        float startTime = Time.time;
        while(startTime - Time.time <= animationTime)
        {

        }

        yield return null;
    }

}
