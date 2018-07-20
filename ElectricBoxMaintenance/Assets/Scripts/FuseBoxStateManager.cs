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
            StartCoroutine(SetupFusebox());
        }

    }

    IEnumerator SetupFusebox()
    {
        yield return new WaitForSeconds(2.0f);

        //ElectricBox
        //var box = sceneLink.transform.Find("fusebox-vertx");
        //if (box == null)
        //    FuseboxNode = SceneLink.Instance.CreateNode(
        //        "fusebox-vertx",
        //        new Vector3(0, 0, 0),
        //        Quaternion.identity,
        //        Vector3.one,
        //        "863b5c9a-7b83-4a88-b4d6-41a33bdba80e"
        //        );
        //else
        //    FuseboxNode = box.gameObject;


        handKeyAnimation = CreateNodeByNameAndId("vertx-event-handler", "");
        handKeyAnimation.AddComponent<VertxEventHandler>();

        //var guid = handKeyAnimation.GetComponent<NodeLink>().Guid;
        //Debug.Log(guid);

        //yield return new WaitForSeconds(5f);
        ////DestroyObject(handKeyAnimation);

        //GameObject switchOne = CreateNodeByNameAndId("arrow-switch-1", "36748b64-8251-4b76-8672-d67d5522dfb1");

        //yield return new WaitForSeconds(5f);
        ////DestroyObject(switchOne);

        //GameObject switchTwo = CreateNodeByNameAndId("arrow-switch-2", "c74cf1eb-da54-448e-b5c2-a423d16064a4");



        //yield return new WaitForSeconds(5f);
        ////DestroyObject(switchTwo);

        //GameObject switchThree = CreateNodeByNameAndId("arrow-switch-3", "ec53c75e-ec10-4014-8d96-f73f2e27ca82");


        //yield return new WaitForSeconds(5f);
        //DestroyObject(switchThree);


    }

    // Method to create and return Vertex Node Link Game object 
    private GameObject CreateNodeByNameAndId(string name, string id)
    {
        var vertxObject = SceneLink.Instance.transform.Find(name);
        GameObject nodeGameObject;
        if (vertxObject == null)
        {
            nodeGameObject = SceneLink.Instance.CreateNode(name, 
                new Vector3(0f, 0f, 0f),
                Quaternion.identity,
                Vector3.one,
                id
           );
        }
        else
        {
            nodeGameObject = vertxObject.gameObject;
        }
        return nodeGameObject;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
