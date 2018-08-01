using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;
using VertexDataTypes;

public class ModifiedStart : MonoBehaviour {

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