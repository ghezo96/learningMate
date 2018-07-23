using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

[RequireComponent(typeof(SceneLink))]
public class FuseBoxStateManager : MonoBehaviour
{

    public SceneLink sceneLink;

    public GameObject FuseboxNode;
    public GameObject handKeyAnimation;



    // Use this for initialization
    void Start()
    {
        sceneLink = GetComponent<SceneLink>();
        sceneLink.OnStateChange += SceneLink_OnStateChange;
    }

    private void SceneLink_OnStateChange(SceneLinkStatus oldState, SceneLinkStatus newState)
    {
        if (newState == SceneLinkStatus.Connected)
        {
            Debug.Log("VERTX connected");
            StartCoroutine(SetUpEventHandler());
        }

    }

    IEnumerator SetUpEventHandler()
    {
        yield return new WaitForSeconds(2.0f);

        handKeyAnimation = CreateNode("vertx-event-handler", "");
        handKeyAnimation.AddComponent<VertxEventHandler>();

        //CreateNode("Vertx-electrical-box", "863b5c9a-7b83-4a88-b4d6-41a33bdba80e");

    }

    // Method to create and return Vertex Node Link Game object 
    private GameObject CreateNode(string name, string id)
    {
        var vertxObject = sceneLink.transform.Find(name);
        GameObject vertxThing;

        if (vertxObject == null)
        {
            vertxThing = SceneLink.Instance.CreateNode(name, 
                new Vector3(0f, 0f, 0f),
                Quaternion.identity,
                Vector3.one,
                id
           );

            //vertxThing = vertxObject.gameObject;
        }
        else
        {
            Debug.Log("Node: " + name + " \n could not be created");
            vertxThing = vertxObject.gameObject;

        }

        return vertxThing;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
