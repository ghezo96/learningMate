using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace PiUpdate
{

    class Program
    {
        static ElectricBox box = new ElectricBox();
        static string sceneId = "ab324e2d-823a-4031-9ad4-34fdf77583c3";
        static string guid;
        
        static void Main(string[] args)
        {
            try
            {
                guid = GetGUIDBySceneIDFromVertx();
            }
            catch(NullReferenceException exception)
            {
                Console.WriteLine("Null reference exception caught, " + exception.Message);
            }
            box.Add(new Component("KEY_ANIMATION", "gpio8", "0"));
            box.Add(new Component("SWITCH_ONE", "gpio7", "0"));
            box.Add(new Component("SWITCH_TWO", "gpio18", "0"));
            box.Add(new Component("SWITCH_THREE", "gpio22", "0"));
            box.Add(new Component("DOOR_ANIMATION", "gpio27", "0"));
            //box.Add(new Component("BATTERY_ANIMATION", "gpio17", "0"));

            WebClient client = new WebClient();
            client.BaseAddress = "https://staging.vertx.cloud";
            client.Headers.Add("Content-Type", "application/json");
            
            //Create pin Directories and contents to check its current status
            foreach(Component component in box.getComponents())
            {
                if(!Directory.Exists("/sys/class/gpio/" + component.getGPIO() + "/")) {
                    component.Export();
                }
                    component.update();
            }

            //Send current box status to unity application
            try
            {
                Console.WriteLine(box.getCurrentState());
                Console.WriteLine("Sending current state to VERTX");
                client.UploadData("/session/fire/" + sceneId + "/" + guid + "/OnUpdate", System.Text.UTF8Encoding.UTF8.GetBytes(box.getCurrentState()));
                Console.WriteLine("Data sent to VERTX");
            }
            catch(WebException webException)
            {
                Console.WriteLine("Web exception caught => " + webException.Message);
            }

            //Constant service running to check state change
            while (true)
            {
                if(guid == null)
                {
                    try
                    {
                        guid = GetGUIDBySceneIDFromVertx();
                    }
                    catch (NullReferenceException exception)
                    {
                        Console.WriteLine("Null reference exception caught, " + exception.Message);
                    }
                }
                foreach(Component component in box.getComponents())
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
                            Console.WriteLine("WebException thrown => " + webEcxeption.Message);
                        }
                    }
                }
            }
        }

        static string GetGUIDBySceneIDFromVertx()
        {
            WebRequest request = WebRequest.Create("https://staging.vertx.cloud/session/scene/" + sceneId);
            // Get the response.  
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            // Deserialize the repsonse from vertx
            VertxObject responseObj = JsonConvert.DeserializeObject<VertxObject>(responseFromServer);
            
            Child child =  responseObj.rootNode.children.FirstOrDefault(vetxObj => vetxObj.id == "VertxEventManager");
            // Clean up the streams and the response.  
            reader.Close();
            response.Close();

            return child.guid;

        }
    }
}
