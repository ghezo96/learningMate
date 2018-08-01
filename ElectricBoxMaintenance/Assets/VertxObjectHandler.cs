using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;
using HoloToolkit.Unity.InputModule;

public class VertxObjectHandler : MonoBehaviour {

    public string[,] ComponentArray;

    GameObject CreatedComponent;

    // Use this for initialization
    void Start () {

        //x-position
        float switchXPosition = -0.08f;
        float connectorXPosition = -0.04f;

        ComponentArray = new string[,]
        {
            {"SWITCH", "a00e3fa6-babb-498a-a559-e9ae05cf9c31"},
            {"SWITCH", "a00e3fa6-babb-498a-a559-e9ae05cf9c31"},
            {"SWITCH", "a00e3fa6-babb-498a-a559-e9ae05cf9c31"},
            {"CONNECTOR", "65257fc0-b39a-459d-93d3-25bce8d6bfd7"},
            {"CONNECTOR", "65257fc0-b39a-459d-93d3-25bce8d6bfd7"},
            {"BOX", "35296c54-8ae5-430a-b6eb-32048c883606" }
        };

        //spawn vertx objects
        for (int i = 0; i < (ComponentArray.Length/2); i++)
        {
            //POSITIONAL CHANGES
            if(ComponentArray[i, 0] == "SWITCH")
            {
                CreatedComponent = CreateNode(ComponentArray[i, 0], ComponentArray[i, 1], new Vector3(switchXPosition, 0f, 0f));
                switchXPosition += 0.08f;
            }
            if(ComponentArray[i, 0] == "CONNECTOR")
            {
                CreatedComponent = CreateNode(ComponentArray[i, 0], ComponentArray[i, 1], new Vector3(connectorXPosition, -0.1f, 0f));
                connectorXPosition += 0.08f;
            }
            CreatedComponent.AddComponent<HandDraggable>();
            CreatedComponent.AddComponent<Snapping>();

            //CreatedComponent.AddComponent<ObjectGameEvent>();
            if(ComponentArray[i, 0] == "BOX")
            {
                CreatedComponent = CreateNode(ComponentArray[i, 0], ComponentArray[i, 1], new Vector3(0.14f, -0.3f, 0f));
            }
            CreatedComponent.AddComponent<ModifiedStart>();
        }
        CreatedComponent = CreateNode("SNAPPING", "a00e3fa6-babb-498a-a559-e9ae05cf9c31", new Vector3(0f, 0.1f, 0f));
        CreatedComponent.AddComponent<SnapToThis>();
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
