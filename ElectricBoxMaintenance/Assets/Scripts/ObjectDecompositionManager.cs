using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

public class ObjectDecompositionManager : MonoBehaviour {

    GameObject wholeBox;
    GameObject ElectricBox;

    public string[,] DecompositionComponents = new string[,]
    {
        {"BoxLayer","3ee7a6a3-5d8d-4a17-bd62-5ea6cd790ce6"},
        {"WireLayer","3f9d7ae0-bac4-4f94-9e8f-e3b13435d932"},
        {"FuseLayer","f444f89c-d4aa-428b-91c3-de7ddd5f76e2"},
        {"SwitchLayer","8bc9bf0b-20c7-4233-9a96-c934e74e2e19"}
    };

	// Use this for initialization
	void Start () {

        //ElectricBox = CreateNode("ElectricBox", "863b5c9a-7b83-4a88-b4d6-41a33bdba80e");
        //ElectricBox.AddComponent<LoadElectricBox>();
	}

    void LoadComponents()
    {
        for (int i = 0; i < DecompositionComponents.Length/2; i++)
        {
            CreateNode(DecompositionComponents[i,0], DecompositionComponents[i, 1]);
        }
        ElectricBox.AddComponent<ObjectDecompositionVERTX>();
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

    public void VertxDecomposeStart()
    {

        // Get the whole box and align electric box with it
        //GameObject wholeBox = GameObject.FindGameObjectWithTag("WholeBox");
        ElectricBox = CreateNode("ElectricBox", null);
        //ElectricBox.transform.position = wholeBox.transform.position;
        //ElectricBox.transform.rotation = wholeBox.transform.rot
        LoadComponents();
        ElectricBox.transform.rotation *= Quaternion.Euler(wholeBox.transform.rotation.x, 180, wholeBox.transform.rotation.z);

    }

    private void Update()
    {
        GameObject wholeBox = GameObject.FindGameObjectWithTag("WholeBox");
        if (ElectricBox)
        {
            ElectricBox.transform.position = wholeBox.transform.position;
            

        }
    }
}
