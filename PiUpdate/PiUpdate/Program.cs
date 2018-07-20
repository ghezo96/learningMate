using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace PiUpdate
{

    //class SwitchStatus
    //{
    //    public string switchName;
    //    public string switchStatus;
    //}

    //class BoxStatus
    //{
    //    public string door;
    //    public List<SwitchStatus> switchStatusList;
    //}

    class Program
    {

        //static BoxStatus boxStatus;
        //static List<SwitchStatus> switchList = new List<SwitchStatus>();
        static List<Component> components = new List<Component>();

        static void Main(string[] args)
        {

            //boxStatus = new BoxStatus();
            //boxStatus.door = "OPEN";

            //boxStatus.switchStatusList = new List<SwitchStatus>();
            components.Add(new Component("Switch 1", "gpio8", "0"));

            WebClient client = new WebClient();
            client.BaseAddress = "https://staging.vertx.cloud";
            //client.Headers.Add("Authorization", "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjVBNzBGNUE2MTJGQjM5OTg3RjdGNUI3RTU2NzA2MTMyNDNGMTZENkMiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJXbkQxcGhMN09aaF9mMXQtVm5CaE1rUHhiV3cifQ.eyJuYmYiOjE1MzE5OTA4NDIsImV4cCI6MTUzMjA1NTY0MiwiaXNzIjoiaHR0cHM6Ly9zdGFnaW5nLnZlcnR4LmNsb3VkL2lkZW50aXR5IiwiYXVkIjpbImh0dHBzOi8vc3RhZ2luZy52ZXJ0eC5jbG91ZC9pZGVudGl0eS9yZXNvdXJjZXMiLCJ2ZXJ0ZXhfYXV0aF9hcGkiLCJ2ZXJ0ZXhfY29yZV9hcGkiLCJ2ZXJ0ZXhfZ2l0cGlwZWxpbmVfYXBpIiwidmVydGV4X2dsdGZwaXBlbGluZV9hcGkiLCJ2ZXJ0ZXhfaW1wb3J0ZXJfYXBpIiwidmVydGV4X3NjcmlwdF9hcGkiLCJ2ZXJ0ZXhfc2Vzc2lvbl9hcGkiXSwiY2xpZW50X2lkIjoidmVydGV4X3dlYl9mcm9udGVuZCIsInN1YiI6ImRhNWYwMzNiLTA3NDQtNGUyNS00ZDBhLTA4ZDVkYTk4ZmExNSIsImF1dGhfdGltZSI6MTUzMTk5MDg0MiwiaWRwIjoiTWljcm9zb2Z0QXp1cmVBZCIsIkFzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiMWY2NmIxNGMtYWM3Yi00MzQ5LWI0ZTItYWYzNjFkZGMyMWZkIiwidmVydGV4X3RlbmFudCI6WyJkYWQzMjI4ZC0yYjJkLTQ2MTEtNzM4MC0wOGQ1ZGE3YTQ0ZjIiLCJkYWQzMjI4ZC0yYjJkLTQ2MTEtNzM4MC0wOGQ1ZGE3YTQ0ZjIiXSwidmVydGV4X3RjIjoiMSIsInByZWZlcnJlZF91c2VybmFtZSI6IkFobWVkLkVsLkdoYXphd3lAdmlzcmFjY2VsZXJhdG9yLm9ubWljcm9zb2Z0LmNvbSIsImVtYWlsIjoiQWhtZWQuRWwuR2hhemF3eUB2aXNyYWNjZWxlcmF0b3Iub25taWNyb3NvZnQuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJuYW1lIjoiQWhtZWQgRWwgR2hhemF3eSIsInZlcnRleF9saWNlbmNlZCI6IjEiLCJzY29wZSI6WyJvcGVuaWQiLCJwcm9maWxlIiwiZW1haWwiLCJ2ZXJ0ZXhfcHJvZmlsZSIsInZlcnRleF9hdXRoX2FwaSIsInZlcnRleF9jb3JlX2FwaSIsInZlcnRleF9naXRwaXBlbGluZV9hcGkiLCJ2ZXJ0ZXhfZ2x0ZnBpcGVsaW5lX2FwaSIsInZlcnRleF9pbXBvcnRlcl9hcGkiLCJ2ZXJ0ZXhfc2NyaXB0X2FwaSIsInZlcnRleF9zZXNzaW9uX2FwaSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJleHRlcm5hbCJdfQ.ngJbrVoJ_K83aDa_B46MeyjPcoFhmyTl0oWP8Ra8J_E5R-7TDkRxVxfAqQypChuRJPIJ54dED59Bd8rYiDAYYIV4UtXRf_r6UTAwRr0wic_wyfIoJ7ZEmwSmX-_pNbixNVKFRqZNYedv6AWf0GaLPvQEbB5XoybujTj-FkMtHu4R6SDYW1D02CczgTuadKrAbeIfkn7a2eqDVAkuubWChmLjDOs03A8CON2oFgIbTtsAY1i2HnaxmkHjPm2IStuFEPiTqEstWO96nm3ciVp10GGdV0hEFHrrzHenOavlGetOtQAjBGkk5vwLTcx_oBFk27TqEq2KxU8gdOroZXn0Dg");
            client.Headers.Add("Content-Type", "application/json");
            //Create pin GPIO7 Directory and contents to keep track of its current status
            //if (!Directory.Exists("/sys/class/gpio/gpio7/"))
            //{
            //    File.WriteAllText("/sys/class/gpio/export", "7");
            //    File.WriteAllText("/sys/class/gpio/gpio7/direction", "in");
            //}


            /*foreach(Component component in components)
            {
                if(!Directory.Exists("/sys/class/gpio/" + component.getGPIO() + "/")) {
                    File.WriteAllBytes("/sys/class/gpio/export", component.getGPIO())
                }
            }*/

            //Create pin GPIO8 Directory and contents to check its current status
            if (!Directory.Exists("/sys/class/gpio/gpio8/"))
            {
                File.WriteAllText("/sys/class/gpio/export", "8");
                File.WriteAllText("/sys/class/gpio/gpio8/direction", "in");
            }

            //string lastState = null; 

            //Constant service running to check state change
            while (true)
            {
                foreach(Component component in components)
                {
                    component.update();
                    bool changed = component.isChanged();

                    if (changed)
                    {
                        //string encodedState = $"\"state:{boxStatus.switchStatusList[1].switchName}:{boxStatus.switchStatusList[1].switchStatus}\"";
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
