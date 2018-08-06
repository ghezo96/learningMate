using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;
using HoloToolkit.Unity.InputModule;

public class VertxObjectHandler : MonoBehaviour {

    public string[,] ComponentArray;

    public static List<GameObject> ObjectComparisonList = new List<GameObject>();

    public static int GameObjectCounter;

    GameObject CreatedComponent;

    // Use this for initialization
    void Start (){


        //x-position
        float switchXPosition = -0.08f;
        float connectorXPosition = -0.04f;

        ComponentArray = new string[,]
        {
            {"SWITCH_1", "a00e3fa6-babb-498a-a559-e9ae05cf9c31"},
            {"SWITCH_2", "a00e3fa6-babb-498a-a559-e9ae05cf9c31"},
            {"SWITCH_3", "a00e3fa6-babb-498a-a559-e9ae05cf9c31"},
            {"CONNECTOR_1", "65257fc0-b39a-459d-93d3-25bce8d6bfd7"},
            {"CONNECTOR_2", "65257fc0-b39a-459d-93d3-25bce8d6bfd7"},
            {"BOX", "35296c54-8ae5-430a-b6eb-32048c883606" }
        };

        //spawn vertx objects
        for (int i = 0; i < (ComponentArray.Length/2); i++)
        {
            //POSITIONAL CHANGES
            if(ComponentArray[i, 0].Substring(0,3) == "SWI")
            {
                CreatedComponent = CreateNode(ComponentArray[i, 0], ComponentArray[i, 1], new Vector3(switchXPosition, 0f, 0f));
                switchXPosition += 0.08f;
                CreatedComponent.AddComponent<CreateWires>();
                CreatedComponent.AddComponent<HandDraggable>();
            }

            if(ComponentArray[i, 0].Substring(0,3) == "CON")
            {
                CreatedComponent = CreateNode(ComponentArray[i, 0], ComponentArray[i, 1], new Vector3(connectorXPosition, -0.1f, 0f));
                connectorXPosition += 0.08f;
                CreatedComponent.AddComponent<CreateWires>();
                CreatedComponent.AddComponent<HandDraggable>();
            }

            if(ComponentArray[i, 0] == "BOX")
            {
                CreatedComponent = CreateNode(ComponentArray[i, 0], ComponentArray[i, 1], new Vector3(0.14f, -0.3f, 0f));
            }

            CreatedComponent.AddComponent<VERTXMeshCollider>();
        }
    }

    // Method to create and return Vertex Node Link Game object 
    private GameObject CreateNode(string name, string id, Vector3 position)
    {
        var vertxThing = SceneLink.Instance.CreateNode
            (name,
            position,
            Quaternion.identity,
            Vector3.one,
            id,
            null,
            "DefaultNodeLink");
        return vertxThing.gameObject;
    }
}
