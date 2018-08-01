using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using VertexUnityPlayer;

public class ObjectGameEvent : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION");
        if(collision.transform.name == "Snapping")
        {
            Debug.Log("SnappingRIGHTNOW");
            gameObject.transform.position = collision.transform.position;
        }
    }
}
