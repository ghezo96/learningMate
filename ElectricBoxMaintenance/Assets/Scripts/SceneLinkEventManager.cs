using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

[RequireComponent(typeof(SceneLink))]
public class SceneLinkEventManager : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //GameObject HandlerNode = CreateNode("VertxEventManager", null);
        //HandlerNode.AddComponent<VertxEventHandler>();
        StartCoroutine(AttachManagers());

    }

    IEnumerator AttachManagers()
    {

        yield return new WaitForSeconds(3f);
        LoadVertxEventManager();
        LoadLiveInformationManager();
    }

    //private void Instance_OnStateChange(SceneLinkStatus oldState, SceneLinkStatus newState)
    //{
    //    if (newState == SceneLinkStatus.Connected)
    //    {
    //        / dothings
    //    }
    //}

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
        return vertxThing;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadVertxEventManager()
    {
        GameObject HandlerNode = CreateNode("VertxEventManager", null);
        HandlerNode.AddComponent<VertxEventHandler>();
    }

    public void LoadLiveInformationManager()
    {
        GameObject VERTXobjectDecompositionHandlerNode = CreateNode("VertxObjectDecompositionHandler", null);
        VERTXobjectDecompositionHandlerNode.AddComponent<ObjectDecompositionManager>();
    }

    //void Collaboration()
    //{
    //    GameObject VertxObjectHandlerNode = CreateNode("VertxObjectHandler", null);
    //    VertxObjectHandlerNode.AddComponent<VertxObjectHandler>();
    //}

}
