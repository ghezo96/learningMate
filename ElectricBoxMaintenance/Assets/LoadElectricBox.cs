using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;
using VertexDataTypes;

public class LoadElectricBox : MonoBehaviour {

    public GameObject returnedObject;

    void NodeLink_Loaded()
    {
        Debug.Log("Finding Object");
        RecurrsionSearch(gameObject);
        ObjectDecompositionVERTX.boxObject = returnedObject;
    }

    void RecurrsionSearch(GameObject gameObject)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject childObject = gameObject.transform.GetChild(i).gameObject;
            if(childObject.name == "RootNode")
            {
                returnedObject = childObject;
                childObject.AddComponent<ObjectDecompositionVERTX>();
            }
            else
            {
                RecurrsionSearch(childObject);
            }
        }
    }
}
