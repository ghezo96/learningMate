using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;
using HoloToolkit.Unity;
using VertexDataTypes;

public class SnapToThis : MonoBehaviour {

    void NodeLink_Loaded()
    {
        RecurrsionSearch(gameObject);
    }

    void RecurrsionSearch(GameObject gameObject)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject childObject = gameObject.transform.GetChild(i).gameObject;
            if (childObject.name == "Primitive")
            {
                MeshCollider meshCollider = childObject.GetComponent<MeshCollider>();
                meshCollider.skinWidth = 0.0001f;
                MeshRenderer meshRenderer = childObject.GetComponent<MeshRenderer>();
                //meshRenderer.enabled = false;
                Rigidbody rb = childObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                RecurrsionSearch(childObject);
            }
        }
    }
}
