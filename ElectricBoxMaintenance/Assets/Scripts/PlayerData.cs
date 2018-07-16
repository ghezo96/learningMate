using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;

public class PlayerData : MonoBehaviour {


    public ResponseObject responseObject;

    // Use this for initialization
    void Start ()
    {
        
	}
	
    public void GetData()
    {
        StartCoroutine(ServiceRequestCoroutine());
    }

	IEnumerator ServiceRequestCoroutine()
    {
        yield return new WaitForSeconds(5f);

        string dataUri = "/core/v1.0/Resource//b13daeb3-d3db-4d7d-bb3c-87fa090b58b8/TextInfo.json";
        ResultContainer<ResponseObject> result = new ResultContainer<ResponseObject>();

        yield return ServiceRequest.Get<ResponseObject>(dataUri, result);

        ResponseObject returnedObject = result.value as ResponseObject;
        if(returnedObject != null)
        {
            Debug.Log(returnedObject.WindowText[0]);
        }

    }
}
