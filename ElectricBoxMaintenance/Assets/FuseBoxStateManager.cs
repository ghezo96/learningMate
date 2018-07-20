using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

[RequireComponent(typeof(SceneLink))]
public class FuseBoxStateManager : MonoBehaviour {

    public SceneLink sceneLink;

    public GameObject FuseboxNode;
    public GameObject handKeyAnimation;

    // Use this for initialization
    void Start () {
        sceneLink = GetComponent<SceneLink>();
        sceneLink.OnStateChange += SceneLink_OnStateChange;
	}

    private void SceneLink_OnStateChange(SceneLinkStatus oldState, SceneLinkStatus newState)
    {
        if(newState == SceneLinkStatus.Connected)
        {
            StartCoroutine(SetupFusebox());
        }

        if(newState == SceneLinkStatus.Disconnected)
        {
            sceneLink.transform.Find("fusebox-vertx");
            sceneLink.transform.Find("hand-Key-Animation");
            sceneLink.transform.Find("arrow-switch-1");
            sceneLink.transform.Find("arrow-switch-2");
            sceneLink.transform.Find("arrow-switch-3");
        }
    }

    IEnumerator SetupFusebox()
    {
        yield return new WaitForSeconds(2.0f);

        //ElectricBox
        var box = sceneLink.transform.Find("fusebox-vertx");
        if (box == null)
            FuseboxNode = SceneLink.Instance.CreateNode(
                "fusebox-vertx",
                new Vector3(0, 0, 0),
                Quaternion.identity,
                Vector3.one,
                "863b5c9a-7b83-4a88-b4d6-41a33bdba80e"
                );
        else
            FuseboxNode = box.gameObject;

            handKeyAnimation = SceneLink.Instance.CreateNode(
            "hand-Key-Animation",
            new Vector3(0f, 0f, 0f),
            Quaternion.identity,
            Vector3.one,
            "41fb345e-02a6-41fb-abdd-57e9b099acc0"
            );

        GameObject switchOne = SceneLink.Instance.CreateNode(
            "arrow-switch-1",
            new Vector3(0f, 0f, 0f),
            Quaternion.identity,
            Vector3.one,
            "36748b64-8251-4b76-8672-d67d5522dfb1"
            );

        GameObject switchTwo = SceneLink.Instance.CreateNode(
            "arrow-switch-2",
            new Vector3(0f, 0f, 0f),
            Quaternion.identity,
            Vector3.one,
            "c74cf1eb-da54-448e-b5c2-a423d16064a4"
            );

        GameObject switchThree = SceneLink.Instance.CreateNode(
            "arrow-switch-3",
            new Vector3(0f, 0f, 0f),
            Quaternion.identity,
            Vector3.one,
            "ec53c75e-ec10-4014-8d96-f73f2e27ca82"
            );




    }

    // Update is called once per frame
    void Update () {
		
	}
}
