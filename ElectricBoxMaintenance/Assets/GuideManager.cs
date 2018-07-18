using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class GuideManager : MonoBehaviour{

    public GameObject[] Steps;
    public FloatingButton Next;
    public FloatingButton Previous;
    public MainMenuContainer InteractiveMenuContainer;


    //bool clicked = false;
    //bool isActive = false;

    int StepPointer = 0;



	// Use this for initialization
	void Start () {

        //Next.Clicked += NextStep();

        Steps = new GameObject[transform.childCount -1];
        PopulateArray();

        Steps[0].SetActive(true);
        InteractiveMenuContainer.ButtonClicked += OnButtonClicked;
        Next.Clicked += NextStep;
        Previous.Clicked += PreviousStep;


	}

    public void setActiveStatus(bool status)
    {
        gameObject.SetActive(status);
    }
	
	// Update is called once per frame
	void Update () {

        
    }

    void PopulateArray()
    {
        for (int i = 0; i < Steps.Length; i++)
        {
            Steps[i] = transform.GetChild(i).gameObject;
        }
    }

    public void NextStep(GameObject button)
    {

        if(StepPointer < Steps.Length - 1)
        {
            Steps[StepPointer].SetActive(false);
            StepPointer++;
            Steps[StepPointer].SetActive(true);
        }

    }

    public void PreviousStep(GameObject button)
    {
        if (StepPointer > 0)
        {
            Steps[StepPointer].SetActive(false);
            StepPointer--;
            Steps[StepPointer].SetActive(true);
        }
    }

   public void OnButtonClicked (GameObject button)
    {
        //Debug.Log(button.name);
    }


    //void NextStep()
    //{
    //    for (int i = 0; i < Steps.Length; i++)
    //    {
    //        if (clicked && isActive)
    //        {
    //            Steps[i].SetActive(false);
    //            Steps[i + 1].SetActive(true);
    //        }

    //    }
    //}
}
