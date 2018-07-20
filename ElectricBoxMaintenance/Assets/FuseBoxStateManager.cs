using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

[RequireComponent(typeof(SceneLink))]
public class FuseBoxStateManager : MonoBehaviour {

    public SceneLink sceneLink;

    public GameObject fuseBoxNode;

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
    }

    IEnumerator SetupFusebox()
    {
        yield return new WaitForSeconds(2.0f);

        var box = sceneLink.transform.Find("fusebox-twin");
        if (box == null)
            fuseBoxNode = sceneLink.CreateNode("fusebox-twin", Vector3.zero, Quaternion.identity, Vector3.one, null, null, "FuseBoxState");
        else
            fuseBoxNode = box.gameObject;

    }

    // Update is called once per frame
    void Update () {
		
	}
}
