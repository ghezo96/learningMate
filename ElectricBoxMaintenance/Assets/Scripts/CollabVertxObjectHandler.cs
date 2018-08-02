using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;
using HoloToolkit.Unity.InputModule;

public class CollabVertxObjectHandler : MonoBehaviour {

    public string[,] ComponentArray;
    bool doItOnce = true;

    GameObject VertxBoxComponent;
    List<GameObject> vertxGameObjects;

    // Use this for initialization
    void Start () {

        vertxGameObjects = new List<GameObject>();
      
        ComponentArray = new string[,]
        {
            {"SWITCH", "a00e3fa6-babb-498a-a559-e9ae05cf9c31"},
            {"SWITCH", "a00e3fa6-babb-498a-a559-e9ae05cf9c31"},
            {"SWITCH", "a00e3fa6-babb-498a-a559-e9ae05cf9c31"},
            {"CONNECTOR", "65257fc0-b39a-459d-93d3-25bce8d6bfd7"},
            {"CONNECTOR", "65257fc0-b39a-459d-93d3-25bce8d6bfd7"},
            {"BOX", "f75f4a1f-1988-4498-be3a-f8f7a8149625" }
        };

        //spawn vertx objects
        for (int i = 0; i < (ComponentArray.Length/2); i++)
        {
            //POSITIONAL CHANGES
           
            if(ComponentArray[i, 0] == "CONNECTOR")
            {
                GameObject connectorComponent = CreateNode(ComponentArray[i, 0], ComponentArray[i, 1], new Vector3(0, 0, 0));
                vertxGameObjects.Add(connectorComponent);
            }
            if(ComponentArray[i, 0] == "BOX")
            {
                VertxBoxComponent = CreateNode(ComponentArray[i, 0], ComponentArray[i, 1], new Vector3(0, 0, 0));
                VertxBoxComponent.AddComponent<ModifiedStart>();
            }
            if (ComponentArray[i, 0] == "SWITCH")
            {
                GameObject switchComponent = CreateNode(ComponentArray[i, 0], ComponentArray[i, 1], new Vector3(0,0, 0));
                vertxGameObjects.Add(switchComponent);
            }
        }

        // Attach listener to the switches and connections
        foreach(GameObject gameObj in vertxGameObjects)
        {
            gameObj.AddComponent<MoveAndSnap2>();
        }
    }



    // Method to create and return Vertex Node Link Game object 
    private GameObject CreateNode(string name, string id, Vector3 position)
    {
        var vertxThing = SceneLink.Instance.CreateNode(name,
            position,
            Quaternion.identity,
            Vector3.one,
            id,
            null,
            "DefaultNodeLink"
        );
        return vertxThing.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (VertxBoxComponent)
        {
            GameObject box = GameObject.FindGameObjectWithTag("Box");
            VertxBoxComponent.transform.position = box.transform.position;
            VertxBoxComponent.transform.rotation = box.transform.rotation;

            var xOffset = box.transform.position.x;
            var offset = box.transform.position.x;
            var zPos = box.transform.position.z;
            var yPos = box.transform.position.y;

            float distance = -0.2f;
            if (doItOnce)
            {
                for (int i = 0; i < vertxGameObjects.Count; i++)
                {

                    vertxGameObjects[i].transform.position = new Vector3(box.transform.position.x - xOffset, yPos, zPos);
                    xOffset -= 0.1f;
                    vertxGameObjects[i].transform.rotation = box.transform.rotation;

                    if (vertxGameObjects[i].name.Contains("CONNECTOR"))
                    {
                        offset -= 0.1f;
                        vertxGameObjects[i].transform.position = new Vector3(box.transform.position.x - offset, yPos - 0.1f, zPos);
                        distance += offset;
                    }
                }
                doItOnce = false;
            }
        }
    }
}
