using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

public class ObjectDecompositionManager : MonoBehaviour {


    GameObject ElectricBox;

    public string[,] DecompositionComponents = new string[,]
    {
        {"BoxLayer","3ee7a6a3-5d8d-4a17-bd62-5ea6cd790ce6"},
        {"WireLayer","3f9d7ae0-bac4-4f94-9e8f-e3b13435d932"},
        {"FuseLayer","f444f89c-d4aa-428b-91c3-de7ddd5f76e2"},
        {"SwitchLayer","9189d159-cd45-4785-889f-6b2313f154d3"}
    };

	// Use this for initialization
	void Start () {

        //ElectricBox = CreateNode("ElectricBox", "863b5c9a-7b83-4a88-b4d6-41a33bdba80e");
        //ElectricBox.AddComponent<LoadElectricBox>();
        ElectricBox = CreateNode("ElectricBox", null);
        ElectricBox.AddComponent<ObjectDecompositionVERTX>();
        LoadComponents();
	}

    void LoadComponents()
    {
        for (int i = 0; i < DecompositionComponents.Length/2; i++)
        {
            CreateNode(DecompositionComponents[i,0], DecompositionComponents[i, 1]);
        }
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

        if(name != "ElectricBox")
        {
            vertxThing.transform.SetParent(ElectricBox.transform);
        }

        return vertxThing;
    }

}
