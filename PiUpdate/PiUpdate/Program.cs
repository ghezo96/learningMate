using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;


namespace PiUpdate
{

    class SwitchStatus
    {
        public string switchName;
        public string switchStatus;
    }

    class BoxStatus
    {
        public string door;
        public List<SwitchStatus> switchStatusList;
    }

    class Program
    {

        static BoxStatus boxStatus;

        static void Main(string[] args)
        {

            boxStatus = new BoxStatus();
            boxStatus.door = "ON";

            boxStatus.switchStatusList = new List<SwitchStatus>();
            List<SwitchStatus> switchList = new List<SwitchStatus>();

            string previous7Reading = "0";
            string previous8Reading = "0";
            WebClient client = new WebClient();
            client.BaseAddress = "https://staging.vertx.cloud";
            client.Headers.Add("Authorization", "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjVBNzBGNUE2MTJGQjM5OTg3RjdGNUI3RTU2NzA2MTMyNDNGMTZENkMiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJXbkQxcGhMN09aaF9mMXQtVm5CaE1rUHhiV3cifQ.eyJuYmYiOjE1MzE3NDI4NTYsImV4cCI6MTUzMTgwNzY1NiwiaXNzIjoiaHR0cHM6Ly9zdGFnaW5nLnZlcnR4LmNsb3VkL2lkZW50aXR5IiwiYXVkIjpbImh0dHBzOi8vc3RhZ2luZy52ZXJ0eC5jbG91ZC9pZGVudGl0eS9yZXNvdXJjZXMiLCJ2ZXJ0ZXhfYXV0aF9hcGkiLCJ2ZXJ0ZXhfY29yZV9hcGkiLCJ2ZXJ0ZXhfZ2l0cGlwZWxpbmVfYXBpIiwidmVydGV4X2dsdGZwaXBlbGluZV9hcGkiLCJ2ZXJ0ZXhfaW1wb3J0ZXJfYXBpIiwidmVydGV4X3NjcmlwdF9hcGkiLCJ2ZXJ0ZXhfc2Vzc2lvbl9hcGkiXSwiY2xpZW50X2lkIjoidmVydGV4X3dlYl9mcm9udGVuZCIsInN1YiI6IjU4NTM2ODBmLWYxZDUtNGI3MS00ZDEzLTA4ZDVkYTk4ZmExNSIsImF1dGhfdGltZSI6MTUzMTc0Mjg1NiwiaWRwIjoiTWljcm9zb2Z0QXp1cmVBZCIsIkFzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiNjI2OTc5ZWEtNmE3ZS00YTMyLThmN2UtMzNmMDFmMjUyOTIxIiwidmVydGV4X3RlbmFudCI6WyJkYWQzMjI4ZC0yYjJkLTQ2MTEtNzM4MC0wOGQ1ZGE3YTQ0ZjIiLCJkYWQzMjI4ZC0yYjJkLTQ2MTEtNzM4MC0wOGQ1ZGE3YTQ0ZjIiXSwidmVydGV4X3RjIjoiMSIsInByZWZlcnJlZF91c2VybmFtZSI6InNhbnRvc2gudmlzaHdha2FybWFAdmlzcmFjY2VsZXJhdG9yLm9ubWljcm9zb2Z0LmNvbSIsImVtYWlsIjoic2FudG9zaC52aXNod2FrYXJtYUB2aXNyYWNjZWxlcmF0b3Iub25taWNyb3NvZnQuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJuYW1lIjoiU2FudG9zaCBWaXNod2FrYXJtYSIsInZlcnRleF9saWNlbmNlZCI6IjEiLCJzY29wZSI6WyJvcGVuaWQiLCJwcm9maWxlIiwiZW1haWwiLCJ2ZXJ0ZXhfcHJvZmlsZSIsInZlcnRleF9hdXRoX2FwaSIsInZlcnRleF9jb3JlX2FwaSIsInZlcnRleF9naXRwaXBlbGluZV9hcGkiLCJ2ZXJ0ZXhfZ2x0ZnBpcGVsaW5lX2FwaSIsInZlcnRleF9pbXBvcnRlcl9hcGkiLCJ2ZXJ0ZXhfc2NyaXB0X2FwaSIsInZlcnRleF9zZXNzaW9uX2FwaSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJleHRlcm5hbCJdfQ.b7q1-hysGLQHnAmb4waX8E3Nnlb-JgwKGhwMtPMwUHhi7d-qVEGmdi5I3QtU9YaiNn1VW9WHKG18_BnYliWk2bbTzepq30c8h5KKgysclBbmq62crZexld1mZx6pRQBjlltFbjEEm2_be1janx028VFf8hkN13M60wMfeIYeYiRr__oT_Z3VxRtkrDrmpdd09-VE7-ULm8cLStL9D92NxpmVdZHU5lyPegaOXeaM51HHTWxxeeh2ESYBdyh4XCtWMotAZUhQuHVth9Ol8caFoPNHoGYSya5oJVrkXI2YbpGUrEn-SjtL99raqzUinFz2oZwQyYBwMYlG7kcW1ScGbA");

            //Create pin GPIO7 Directory and contents to keep track of its current status
            if (!Directory.Exists("/sys/class/gpio/gpio7/"))
            {
                File.WriteAllText("/sys/class/gpio/export", "7");
                File.WriteAllText("/sys/class/gpio/gpio7/direction", "in");
            }

            //Create pin GPIO8Directory and contents to check its current status
            if (!Directory.Exists("/sys/class/gpio/gpio8/"))
            {
                File.WriteAllText("/sys/class/gpio/export", "8");
                File.WriteAllText("/sys/class/gpio/gpio8/direction", "in");
            }
            string currentGPIO7Charge;
            string currentGPIO8Charge;
           
            //Constant service running to check state change
            while (true)
            {
                currentGPIO7Charge = File.ReadAllText("/sys/class/gpio/gpio7/value");
                if (!currentGPIO7Charge.Equals(previous7Reading))
                {
                    Console.WriteLine("previous 7 reading is " + previous7Reading + ", currentGPIO7Charge is " + currentGPIO7Charge);
                    Console.WriteLine("Client sending to VERTX");
                    //testStr += "{\"Name\" : \"Pin7\", \"Status\" : " + currentGPIO7Charge + "}";
                    var thisSwitch = switchList.FirstOrDefault(x => x.switchName == "gpio7");

                    if (thisSwitch == null)
                        switchList.Add(getSwitchStatus("gpio7", currentGPIO7Charge));
                    else
                        thisSwitch.switchStatus = currentGPIO7Charge;

                    //client.UploadData("session/fire/b13daeb3-d3db-4d7d-bb3c-87fa090b58b8/9bc4ca9a-088f-444c-9805-a58b4e989969/SwitchChanged", System.Text.UTF8Encoding.UTF8.GetBytes("newChargeValue"));
                    Console.WriteLine("Client finished writing to VERTX");
                    previous7Reading = currentGPIO7Charge;
                }
                currentGPIO8Charge = File.ReadAllText("/sys/class/gpio/gpio8/value");
                if (!currentGPIO8Charge.Equals(previous8Reading))
                {
                    Console.WriteLine("previous 8 reading is " + previous8Reading + ", currentGPIO8Charge is " + currentGPIO8Charge);
                    Console.WriteLine("Client sending to VERTX");
                    var thisSwitch = switchList.FirstOrDefault(x => x.switchName == "gpio8");

                    if (thisSwitch == null)
                        switchList.Add(getSwitchStatus("gpio8", currentGPIO8Charge));
                    else
                        thisSwitch.switchStatus = currentGPIO8Charge;                    //client.UploadData("session/fire/b13daeb3-d3db-4d7d-bb3c-87fa090b58b8/9bc4ca9a-088f-444c-9805-a58b4e989969/SwitchChanged", System.Text.UTF8Encoding.UTF8.GetBytes("newChargeValue"));
                    Console.WriteLine("Client finished writing to VERTX");
                    previous8Reading = currentGPIO8Charge;
                }
                //boxStatus.switchStatusList.Clear();
                boxStatus.switchStatusList = switchList;

                string boxStatusJson = JsonConvert.SerializeObject(boxStatus);
                client.UploadData("/core/v1.0/resource/b13daeb3-d3db-4d7d-bb3c-87fa090b58b8/BoxStatus.json", System.Text.UTF8Encoding.UTF8.GetBytes(boxStatusJson));
                System.Threading.Thread.Sleep(5000);
                //switchList.Clear();
            }
        }

        private static SwitchStatus getSwitchStatus(string pinName, string status)
        {
            // create PIN STatus Json object
            SwitchStatus switchStatus = new SwitchStatus();
            switchStatus.switchName = pinName;
            switchStatus.switchStatus = status;
            return switchStatus;
        }
    }
}
