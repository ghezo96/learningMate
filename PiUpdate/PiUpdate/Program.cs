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

            guid = (guid == null) ? GetGUIDBySceneIDFromVertx() : null;
            Console.WriteLine(guid);

            box.Add(new Component("KEY_ANIMATION", "gpio8"));
            box.Add(new Component("SWITCH_ONE", "gpio7"));
            box.Add(new Component("SWITCH_TWO", "gpio18"));
            box.Add(new Component("SWITCH_THREE", "gpio22"));
            box.Add(new Component("DOOR_ANIMATION", "gpio27"));
            box.Add(new Component("FUSE_ANIMATION", "gpio10"));

            WebClient client = new WebClient();
            client.BaseAddress = "https://staging.vertx.cloud";
            client.Headers.Add("Content-Type", "application/json");
            
            Console.WriteLine("Program ready!\n");

            //Send current box status to unity application
            /*try
            {
                Console.WriteLine(box.getCurrentState());
                Console.WriteLine("Sending current state to VERTX");
                client.UploadData("/session/fire/" + sceneId + "/" + guid + "/OnUpdate", System.Text.UTF8Encoding.UTF8.GetBytes(box.getCurrentState()));
                Console.WriteLine("Data sent to VERTX");
            }
            catch(WebException webException)
            {
                Console.WriteLine("Web exception caught => " + webException.Message);
            }*/

            //Constant service running to check state change
            while (true)
            {
                foreach(Component component in box.getComponents())
                {
                    component.update();
                    bool changed = component.isChanged();

                    if (changed)
                    {
                        guid = (guid == null) ? GetGUIDBySceneIDFromVertx() : null;
                        Console.WriteLine(guid);
                        //if (guid == null)
                        //{
                        //    //try
                        //    //{
                        //    //    guid = GetGUIDBySceneIDFromVertx();
                        //    //}
                        //    //catch (NullReferenceException exception)
                        //    //{
                        //    //    Console.WriteLine("Null reference exception caught, " + exception.Message);
                        //    //}
                        //}//end if

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
                            if (webEcxeption.Message.Contains("error: (404) Not Found."))
                            {
                                guid = GetGUIDBySceneIDFromVertx();
                            }
                        }
                    }//end if
                }//end foreach
            }//end while
        }//end main

        static string GetGUIDBySceneIDFromVertx()
        {
            string _guid;
            try
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
                
                _guid = child.guid;

            } catch(Exception e)
            {
                _guid = null;
                Console.WriteLine(e.Message);
            }

            return _guid;
        }
    }
}
