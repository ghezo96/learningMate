using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;
using System.Text;

public class VertxServiceRequest : VertexSingleton<VertxServiceRequest> {


    public class WindowText
    {
        public string WindowName { get; set; }
        public string WindowTitle { get; set; }
        public string WindowDescription { get; set; }
    }

    public class ResponseObject
    {
        public List<WindowText> WindowText { get; set; }
    }


    IEnumerator UploadTextData()
    {
        Debug.Log("Uploading");
        string response = "{\"WindowText\":[{\"WindowName\":\"PropertyWindow\",\"WindowTitle\":\"this is title\",\"WindowDescription\":\"this is sample description\"},{\"WindowName\":\"RecordWindow\",\"WindowTitle\":\"this is title\",\"WindowDescription\":\"this is sample description\"},{\"WindowName\":\"StatusWindow\",\"WindowTitle\":\"this is title\",\"WindowDescription\":\"this is sample description\"}]}";
        
        yield return ServiceRequest.Post("/core/v1.0/Resource/b13daeb3-d3db-4d7d-bb3c-87fa090b58b8/TextInfo.json", Encoding.UTF8.GetBytes(response));
        Debug.Log("Uploaded");
    }

    IEnumerator FetchTextData()
    {
        Debug.Log("Fetching");
        ResultContainer<ResponseObject> result = new ResultContainer<ResponseObject>();
        yield return ServiceRequest.Get<ResponseObject>("/core/v1.0/Resource//b13daeb3-d3db-4d7d-bb3c-87fa090b58b8/TextInfo.json", result);

        Debug.Log("Respo" + JsonConvert.SerializeObject(result.Value));
    }
}
