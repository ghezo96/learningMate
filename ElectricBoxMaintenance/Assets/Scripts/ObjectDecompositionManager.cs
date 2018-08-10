using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

public class ObjectDecompositionManager : MonoBehaviour {

    GameObject UXHandler;
    GameObject Box;
    //GameObject WholeBox;
    GameObject ElectricBox;

    List<GameObject> objectList = new List<GameObject>();

    public string[,] DecompositionComponents = new string[,]
    {
        {"BoxLayer","3ee7a6a3-5d8d-4a17-bd62-5ea6cd790ce6"},
        {"WireLayer","3f9d7ae0-bac4-4f94-9e8f-e3b13435d932"},
        {"FuseLayer","f444f89c-d4aa-428b-91c3-de7ddd5f76e2"},
        {"SwitchLayer","8bc9bf0b-20c7-4233-9a96-c934e74e2e19"}
    };

	// Use this for initialization
	void Start () {

        UXHandler = GameObject.Find("UXHandler");
        Box = UXHandler.transform.Find("Box").gameObject;
        
    }

    private void Update()
    {
            foreach (GameObject layer in objectList)
            {
                layer.transform.position = Box.transform.position;
                layer.transform.rotation = Box.transform.rotation;
            }
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
            Vector3 spawnPosition;


            if (Box == null)
            {
                spawnPosition = new Vector3(0f, 0f, 0f);
                Debug.Log("Box not found");
            }
            else
            {
                spawnPosition = Box.transform.position;
            }


            // Honestly Siege, I'm making this up at this point, if it works it's a miracle, if it doesn't it's Jamie's fault

            vertxThing = SceneLink.Instance.CreateNode(name,
                spawnPosition,
                Quaternion.Inverse(Box.transform.rotation),
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

        if (!objectList.Contains(vertxThing))
        {
            objectList.Add(vertxThing);
        }

        return vertxThing;
    }

    public void VertxDecomposeStart()
    {
        Debug.Log("VertxDecomposeStart : ");
        ElectricBox = CreateNode("ElectricBox", null);
        LoadComponents();
    }

    public void VertxComposition()
    {
        ElectricBox.GetComponent<ObjectDecompositionVERTX>().ComposeVERTX();
    }

    public void RemoveBox()
    {
        Destroy(ElectricBox);
    }

}
