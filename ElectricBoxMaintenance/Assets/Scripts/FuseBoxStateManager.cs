using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

[RequireComponent(typeof(SceneLink))]
public class FuseBoxStateManager : MonoBehaviour
{

    public SceneLink sceneLink;

    private GameObject FuseboxNode;
    private GameObject handKeyAnimation;



    // Use this for initialization
    void Start()
    {
        sceneLink = GetComponent<SceneLink>();
        sceneLink.OnStateChange += SceneLink_OnStateChange;
    }

    private void SceneLink_OnStateChange(SceneLinkStatus oldState, SceneLinkStatus newState)
    {
        //Debug.Log("SceneLink_OnStateChange - VERTX connected : newstate " + newState );
        if (newState == SceneLinkStatus.Connected)
        {
            Debug.Log("SceneLink_OnStateChange - VERTX connected : ");
            StartCoroutine(SetUpEventHandler());
        }

    }

    IEnumerator SetUpEventHandler()
    {
        yield return new WaitForSeconds(2.0f);

        //CreateNode("Vertx-electrical-box", "863b5c9a-7b83-4a88-b4d6-41a33bdba80e");
        //GameObject HandlerNode = CreateNodeWithPrefab("HandlerNode", "", "EventHandlerNodeLink");
        GameObject HandlerNode = CreateNode("VertxEventManager", null);
        HandlerNode.AddComponent<VertxEventHandler>();

    }

    // Method to create and return Vertex Node Link Game object 
    private GameObject CreateNode(string name, string id)
    {
        var vertxObject = sceneLink.transform.Find(name);
        GameObject vertxThing;

        if (vertxObject == null)
        {
            vertxThing = sceneLink.CreateNode(name,
                new Vector3(0f, 0f, 0f),
                Quaternion.identity,
                Vector3.one,
                id
           );
        }
        else
        {
            Debug.Log("Node: " + name + " \n could not be created");
            vertxThing = vertxObject.gameObject;

        }

        return vertxThing;

    }

    //private GameObject CreateNodeWithPrefab(string name, string id, string prefabId)
    //{

    //    Debug.Log("Node: " + name + " id : " + id);
    //    var vertxObject = sceneLink.transform.Find(name);
    //    GameObject vertxThing;

    //    if (vertxObject == null)
    //    {
    //        vertxThing = SceneLink.Instance.CreateNode(name,
    //            new Vector3(0f, 0f, 0f),
    //            Quaternion.identity,
    //            Vector3.one,
    //            id,
    //            null,
    //            prefabId
    //       );
    //        //vertxThing = vertxObject.gameObject;
    //    }
    //    else
    //    {
    //        vertxThing = vertxObject.gameObject;

    //    }
    //    return vertxThing;

    //}


    // Update is called once per frame
    void Update()
    {

    }
}
