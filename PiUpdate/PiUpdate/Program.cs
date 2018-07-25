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
            string guid = "4789e6e2-45f2-45d1-b2b7-0af7073a4151";
            string sceneId = "b7993275-9930-4f11-a6aa-93ea2cefc4f6";
            components.Add(new Component("KEY_ANIMATION", "gpio8", "0"));
            components.Add(new Component("SWITCH_ONE", "gpio7", "0"));
            components.Add(new Component("SWITCH_TWO", "gpio18", "0"));
            components.Add(new Component("SWITCH_THREE", "gpio17", "0"));

            WebClient client = new WebClient();
            client.BaseAddress = "https://staging.vertx.cloud";
            client.Headers.Add("Content-Type", "application/json");
            
            //Create pin Directories and contents to check its current status
            foreach(Component component in components)
            {
                if(!Directory.Exists("/sys/class/gpio/" + component.getGPIO() + "/")) {
                    File.WriteAllText("/sys/class/gpio/export", component.getPinNumber());
                    File.WriteAllText("/System/class/gpio/" + component.getGPIO() + "/direction", "in");
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
                        string json = component.getJson();
                        client.Headers.Add("Content-Type", "application/json");
                        Console.WriteLine(json);
                        Console.WriteLine("Client sending to VERTX");
                        client.UploadData("/session/fire/" + sceneId + "/" + guid + "/OnUpdate", System.Text.UTF8Encoding.UTF8.GetBytes(json));
                        Console.WriteLine("Data sent");
                    }
                }
                
                System.Threading.Thread.Sleep(50);
            }
        }
    }
}
