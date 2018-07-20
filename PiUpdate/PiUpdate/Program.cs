using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace PiUpdate
{

    class Program
    {
       
        static List<Component> components = new List<Component>();

        static void Main(string[] args)
        {
            components.Add(new Component("Switch 1", "gpio8", "0"));

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
                        client.UploadData("/session/fire/134959e9-2b71-460e-9144-3d4d3a445b83/8039a87d-1524-4ba8-826d-4b7326f5696e/OnUpdate", System.Text.UTF8Encoding.UTF8.GetBytes(json));
                    }
                }
                
                System.Threading.Thread.Sleep(50);
            }
        }
    }
}
