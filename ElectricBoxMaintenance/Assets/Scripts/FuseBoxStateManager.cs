using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

[RequireComponent(typeof(SceneLink))]
public class FuseBoxStateManager : MonoBehaviour
{

    public SceneLink sceneLink;



    // Use this for initialization
    void Start()
    {
        sceneLink = GetComponent<SceneLink>();
        sceneLink.OnStateChange += SceneLink_OnStateChange;
    }

    // On scene connect, Handler is set up
    private void SceneLink_OnStateChange(SceneLinkStatus oldState, SceneLinkStatus newState)
    {
        if (newState == SceneLinkStatus.Connected)
        {
            Debug.Log("SceneLink_OnStateChange - VERTX connected : ");
            StartCoroutine(SetUpEventHandler());
        }

    }

    // Co-routine instantiated Vertx-Event-Manager if not already in scene 
    IEnumerator SetUpEventHandler()
    {
        yield return new WaitForSeconds(2.0f);

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
            Debug.Log("Node: " + name + " \n already exists");
            vertxThing = vertxObject.gameObject;

        }

        return vertxThing;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
