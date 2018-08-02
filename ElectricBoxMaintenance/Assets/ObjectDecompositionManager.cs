using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

public class ObjectDecompositionManager : MonoBehaviour {

    public GameObject ElectricBox;

	// Use this for initialization
	void Start () {

        ElectricBox = CreateNode("ElectricBox", "863b5c9a-7b83-4a88-b4d6-41a33bdba80e");
        ElectricBox.AddComponent<LoadElectricBox>();
	}

    // Method to create and return Vertex Node Link Game object 
    private GameObject CreateNode(string name, string id)
    {
        var vertxObject = SceneLink.Instance.transform.Find(name);
        GameObject vertxThing;

        if (vertxObject == null)
        {
            vertxThing = SceneLink.Instance.CreateNode(name,
                new Vector3(0f, 0f, 0f),
                Quaternion.identity,
                Vector3.one,
                id
           );
        }
        else
        {
            Debug.Log("Node: " + name + " \n already exists");
            vertxThing = vertxObject.gameObject;

        }

        return vertxThing;
    }

}
