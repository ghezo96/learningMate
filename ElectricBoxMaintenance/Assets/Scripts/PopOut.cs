﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;

public class PopOut : MonoBehaviour , IFocusable, IInputHandler
{
    Vector3 initialScale;
    Vector3 targetScale;
    bool focused = false;
    Animator anim;

   // public GameObject Camera;
    public void OnFocusEnter()
    {
        focused = true;
        //Camera.GetComponent<RaycastPositioning>().enabled = false;
        //transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        //initialScale = transform.localScale;
        targetScale = initialScale*1.2f;
        anim.SetBool("AnimFocus", true);
    }

    public void OnFocusExit()
    {
        focused = false;
        //Camera.GetComponent<RaycastPositioning>().enabled = true;
        //transform.localScale = Vector3.one;
        anim.SetBool("AnimFocus", false);

    }   

    // Use this for initialization
    void Start () {
        initialScale = gameObject.transform.localScale;
        anim = gameObject.GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (focused)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 5f);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, initialScale, Time.deltaTime * 5f);
        }
    }

    public void OnInputDown(InputEventData eventData)
    {
        transform.localScale = transform.localScale*0.8f;
    }

    public void OnInputUp(InputEventData eventData)
    {
        
    }
}
