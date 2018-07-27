using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PiUpdate
{

    class Program
    {
        static List<Component> components = new List<Component>();
        
        static void Main(string[] args)
        {
            string guid = GetGUIDBySceneIDFromVertx();
            string sceneId = "585e5033-954b-4288-8f3f-639160786fc7";
            components.Add(new Component("KEY_ANIMATION", "gpio8", "0"));
            components.Add(new Component("SWITCH_ONE", "gpio7", "0"));
            components.Add(new Component("SWITCH_TWO", "gpio18", "0"));
            components.Add(new Component("SWITCH_THREE", "gpio22", "0"));
            components.Add(new Component("DOOR_ANIMATION", "gpio27", "0"));
            //components.Add(new Component("BATTERY_ANIMATION", "gpio17", "0"));

            WebClient client = new WebClient();
            client.BaseAddress = "https://staging.vertx.cloud";
            client.Headers.Add("Content-Type", "application/json");
            
            //Create pin Directories and contents to check its current status
            foreach(Component component in components)
            {
                if(!Directory.Exists("/sys/class/gpio/" + component.getGPIO() + "/")) {
                    //Console.WriteLine(component.getGPIO());
                    File.WriteAllText("/sys/class/gpio/export", component.getPinNumber());
                    File.WriteAllText("/sys/class/gpio/" + component.getGPIO() + "/direction", "in");
                }
                    component.update();
            }

            //Constant service running to check state change
            while (true)
            {
                foreach(Component component in components)
                {
                    component.update();
                    bool changed = component.isChanged();

                    if (changed)
                    {
                        try
                        {
                            string json = component.getJson();
                            client.Headers.Add("Content-Type", "application/json");
                            Console.WriteLine(json);
                            Console.WriteLine("Client sending to VERTX");
                            client.UploadData("/session/fire/" + sceneId + "/" + guid + "/OnUpdate", System.Text.UTF8Encoding.UTF8.GetBytes(json));
                            Console.WriteLine("Data sent");
                        }
                        catch(System.Net.WebException webEcxeption)
                        {
                            Console.WriteLine("WebException thrown");
                        }
                    }
                }
            }
        }

        static string GetGUIDBySceneIDFromVertx()
        {
            WebRequest request = WebRequest.Create("https://staging.vertx.cloud/session/scene/585e5033-954b-4288-8f3f-639160786fc7");
            // Get the response.  
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            // Display the content.  
            // Deserialize the repsonse from vertx
            VertxObject responseObj = JsonConvert.DeserializeObject<VertxObject>(responseFromServer);

            Console.WriteLine(responseObj.rootNode.children.Count);
            string guid = null;
            foreach(Child vetxObj in responseObj.rootNode.children)
            {
                if(vetxObj.id == "VertxEventManager")
                {
                    guid = vetxObj.guid;
                }
            }

            Console.WriteLine("GUID : " + guid );
            // Clean up the streams and the response.  
            reader.Close();
            response.Close();

            return guid;

        }
    }
}
