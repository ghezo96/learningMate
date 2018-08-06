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

    IEnumerator SpawnShit()
    {
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
        for (int i = 0; i < (ComponentArray.Length / 2); i++)
        {
            //POSITIONAL CHANGES

            if (ComponentArray[i, 0] == "CONNECTOR")
            {
                GameObject connectorComponent = CreateNode(ComponentArray[i, 0], ComponentArray[i, 1]);
                vertxGameObjects.Add(connectorComponent);
            }
            if (ComponentArray[i, 0] == "BOX")
            {
                VertxBoxComponent = CreateNode(ComponentArray[i, 0], ComponentArray[i, 1]);
                VertxBoxComponent.AddComponent<ModifiedStart>();
                GameObject box = GameObject.FindGameObjectWithTag("Box");
                VertxBoxComponent.transform.position = box.transform.localPosition;
                VertxBoxComponent.transform.rotation = box.transform.rotation;
                VertxBoxComponent.tag = "THEBOX";
            }
            if (ComponentArray[i, 0] == "SWITCH")
            {
                GameObject switchComponent = CreateNode(ComponentArray[i, 0], ComponentArray[i, 1]);
                vertxGameObjects.Add(switchComponent);
            }
            yield return new WaitForSeconds(0.2f);
        }

        // Attach listener to the switches and connections
        foreach (GameObject gameObj in vertxGameObjects)
        {
            gameObj.AddComponent<MoveAndSnap>();
            gameObj.AddComponent<Rigidbody>();
            gameObj.GetComponent<Rigidbody>().useGravity = false;
            gameObj.GetComponent<Rigidbody>().isKinematic = true;
            //gameObj.GetComponent<BoxCollider>().isTrigger = false;

        }
        yield break;
    }
    // Use this for initialization
    void Start () {

        StartCoroutine(SpawnShit());
    }



    // Method to create and return Vertex Node Link Game object 
    private GameObject CreateNode(string name, string id)
    {
        GameObject box = GameObject.FindGameObjectWithTag("Box");
        var vertxThing = SceneLink.Instance.CreateNode(name,
            box.transform.position,
            box.transform.rotation,
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

            var xOffset = 0.4f;
            var offset = 0.4f;
            var xPos = box.transform.position.x;
            var zPos = box.transform.position.z;
            var yPos = box.transform.position.y;


            if (doItOnce)
            {
                Debug.Log("Box position: " + box.transform.localPosition);
                for (int i = 0; i < vertxGameObjects.Count; i++)
                {

                    vertxGameObjects[i].transform.position = new Vector3(xPos - xOffset, yPos + 0.3f, zPos);
                    xOffset -= 0.1f;
                    vertxGameObjects[i].transform.rotation = box.transform.rotation;
                    
                   
                   

                    if (vertxGameObjects[i].name.Contains("CONNECTOR"))
                    {
                        vertxGameObjects[i].transform.position = new Vector3(xPos - offset, yPos + 0.2f, zPos);
                        offset -=0.1f;
                    }
                    Debug.Log("Switch position: " + vertxGameObjects[i].transform.position);
                }
                doItOnce = false;
            }
        }
    }
}
