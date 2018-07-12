using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

using UnityEngine.UI;

public class WindowManager : MonoBehaviour {

    public PanelWindow recordWindow;
    public PanelWindow propertyWindow;
    public PanelWindow statusWindow;
    ResponseObject responseObject;


    string response = "{\"WindowText\":[{\"WindowName\":\"PropertyWindow\",\"WindowTitle\":\"this is title\",\"WindowDescription\":\"this is sample description\"},{\"WindowName\":\"RecordWindow\",\"WindowTitle\":\"this is title\",\"WindowDescription\":\"this is sample description\"},{\"WindowName\":\"StatusWindow\",\"WindowTitle\":\"this is title\",\"WindowDescription\":\"this is sample description\"}]}";

    // Use this for initialization
    void Start () {

        recordWindow.Show();
        propertyWindow.Show();
        statusWindow.Show();

        // Desearilize the json string
        //ResponseObject responseObject = new ResponseObject();

        responseObject = JsonConvert.DeserializeObject<ResponseObject>(response);

        Debug.Log(responseObject.WindowText.Count);
        SetWindowText();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetWindowText()
    {

        List<WindowText> windowTextList = responseObject.WindowText;

        //Debug.Log("SetWindowText" + windowTextList.Count);
        
        foreach (WindowText windowText in windowTextList)
        {
            Debug.Log("SettingWindowText" + windowText.WindowName);
           
            if(windowText.WindowName == "PropertyWindow")
            {
                propertyWindow.TitleText = windowText.WindowTitle;
                propertyWindow.DescriptionText = windowText.WindowDescription;
            } else if (windowText.WindowName == "RecordWindow")
            {
                recordWindow.TitleText = windowText.WindowTitle;
                recordWindow.DescriptionText = windowText.WindowDescription;
            } else if (windowText.WindowName == "StatusWindow")
            {
                statusWindow.TitleText = windowText.WindowTitle;
                statusWindow.DescriptionText = windowText.WindowDescription;
            }

           
        }
    }


}
