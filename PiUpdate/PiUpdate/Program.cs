using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

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
        static List<SwitchStatus> switchList = new List<SwitchStatus>();

        static void Main(string[] args)
        {

            boxStatus = new BoxStatus();
            boxStatus.door = "OPEN";

            boxStatus.switchStatusList = new List<SwitchStatus>();

            WebClient client = new WebClient();
            client.BaseAddress = "https://staging.vertx.cloud";
            client.Headers.Add("Authorization", "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjVBNzBGNUE2MTJGQjM5OTg3RjdGNUI3RTU2NzA2MTMyNDNGMTZENkMiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJXbkQxcGhMN09aaF9mMXQtVm5CaE1rUHhiV3cifQ.eyJuYmYiOjE1MzE5MTI1OTEsImV4cCI6MTUzMTk3NzM5MSwiaXNzIjoiaHR0cHM6Ly9zdGFnaW5nLnZlcnR4LmNsb3VkL2lkZW50aXR5IiwiYXVkIjpbImh0dHBzOi8vc3RhZ2luZy52ZXJ0eC5jbG91ZC9pZGVudGl0eS9yZXNvdXJjZXMiLCJ2ZXJ0ZXhfYXV0aF9hcGkiLCJ2ZXJ0ZXhfY29yZV9hcGkiLCJ2ZXJ0ZXhfZ2l0cGlwZWxpbmVfYXBpIiwidmVydGV4X2dsdGZwaXBlbGluZV9hcGkiLCJ2ZXJ0ZXhfaW1wb3J0ZXJfYXBpIiwidmVydGV4X3NjcmlwdF9hcGkiLCJ2ZXJ0ZXhfc2Vzc2lvbl9hcGkiXSwiY2xpZW50X2lkIjoidmVydGV4X3dlYl9mcm9udGVuZCIsInN1YiI6IjU4NTM2ODBmLWYxZDUtNGI3MS00ZDEzLTA4ZDVkYTk4ZmExNSIsImF1dGhfdGltZSI6MTUzMTkxMjU5MSwiaWRwIjoiTWljcm9zb2Z0QXp1cmVBZCIsIkFzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiNjI2OTc5ZWEtNmE3ZS00YTMyLThmN2UtMzNmMDFmMjUyOTIxIiwidmVydGV4X3RlbmFudCI6WyJkYWQzMjI4ZC0yYjJkLTQ2MTEtNzM4MC0wOGQ1ZGE3YTQ0ZjIiLCJkYWQzMjI4ZC0yYjJkLTQ2MTEtNzM4MC0wOGQ1ZGE3YTQ0ZjIiXSwidmVydGV4X3RjIjoiMSIsInByZWZlcnJlZF91c2VybmFtZSI6InNhbnRvc2gudmlzaHdha2FybWFAdmlzcmFjY2VsZXJhdG9yLm9ubWljcm9zb2Z0LmNvbSIsImVtYWlsIjoic2FudG9zaC52aXNod2FrYXJtYUB2aXNyYWNjZWxlcmF0b3Iub25taWNyb3NvZnQuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJuYW1lIjoiU2FudG9zaCBWaXNod2FrYXJtYSIsInZlcnRleF9saWNlbmNlZCI6IjEiLCJzY29wZSI6WyJvcGVuaWQiLCJwcm9maWxlIiwiZW1haWwiLCJ2ZXJ0ZXhfcHJvZmlsZSIsInZlcnRleF9hdXRoX2FwaSIsInZlcnRleF9jb3JlX2FwaSIsInZlcnRleF9naXRwaXBlbGluZV9hcGkiLCJ2ZXJ0ZXhfZ2x0ZnBpcGVsaW5lX2FwaSIsInZlcnRleF9pbXBvcnRlcl9hcGkiLCJ2ZXJ0ZXhfc2NyaXB0X2FwaSIsInZlcnRleF9zZXNzaW9uX2FwaSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJleHRlcm5hbCJdfQ.L_-D0r-NuOpIEqKxERxbQbR1oexXueqdzmSGVFgx1AJpDv1xVQ6E-_LZeKcCsCEoB48eQ_UiFfXP85UyvfJ8hwD-kVgqyvy5zvl6ywL2bbnKH_vD1j32hTLvY65rMX1yjJITMlNDT9-SDNln2-SeSLDzjWLxVgWw8MpL4cqh3vGod0YLlDWl4ICtiaB08lDi6AwT3c4CytYh5ajGv10gD1TUwZJFu84lcEHkY-7HHKiJj8F1zIEeEcPK8VejuG80Fo2As0U1YYAr3P4kDvq-fbrXqcTCRj5oWZVZJF0zsOhARXhOwLl1mmxtNKTakoqJsMsIk74Aq38SzDfhOWS4GQ");

            //Create pin GPIO7 Directory and contents to keep track of its current status
            if (!Directory.Exists("/sys/class/gpio/gpio7/"))
            {
                File.WriteAllText("/sys/class/gpio/export", "7");
                File.WriteAllText("/sys/class/gpio/gpio7/direction", "in");
            }

            //Create pin GPIO8 Directory and contents to check its current status
            if (!Directory.Exists("/sys/class/gpio/gpio8/"))
            {
                File.WriteAllText("/sys/class/gpio/export", "8");
                File.WriteAllText("/sys/class/gpio/gpio8/direction", "in");
            }
           
            //Constant service running to check state change
            while (true)
            {
                ReadSwitchStatus("gpio7");
                ReadSwitchStatus("gpio8");
                //boxStatus.switchStatusList.Clear();
                boxStatus.switchStatusList = switchList;

                string boxStatusJson = JsonConvert.SerializeObject(boxStatus);
                Console.WriteLine("Client sending to VERTX");
                client.UploadData("/core/v1.0/resource/b13daeb3-d3db-4d7d-bb3c-87fa090b58b8/BoxStatus.json", System.Text.UTF8Encoding.UTF8.GetBytes(boxStatusJson));
                Console.WriteLine("Client finished writing to VERTX");
                System.Threading.Thread.Sleep(2000);
                //switchList.Clear();
            }
        }

        //Function for updating pin charges and adding them to the new list
        private static void ReadSwitchStatus(string pinName) {
            //Reading pin value and removing all excessive characters
            string currentGPIOCharge = File.ReadAllText("/sys/class/gpio/" + pinName + "/value");
            currentGPIOCharge = Regex.Replace(currentGPIOCharge, @"\t|\n|\r", "");

            SwitchStatus thisSwitch = switchList.FirstOrDefault(x => x.switchName == pinName);

            if (thisSwitch == null)
                switchList.Add(getSwitchObjectWithStatus(pinName, currentGPIOCharge));
            else
                thisSwitch.switchStatus = currentGPIOCharge;
        }

        private static SwitchStatus getSwitchObjectWithStatus(string pinName, string status)
        {
            // create PIN STatus Json object
            SwitchStatus switchStatus = new SwitchStatus();
            switchStatus.switchName = pinName;
            switchStatus.switchStatus = status;
            return switchStatus;
        }
    }
}
