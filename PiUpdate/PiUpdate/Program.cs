using System;
using System.IO;
using System.Net;

namespace PiUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            string previousReading = "0";
            WebClient client = new WebClient();
            client.BaseAddress = "https://staging.vertx.cloud";
            client.Headers.Add("Authorization", "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjVBNzBGNUE2MTJGQjM5OTg3RjdGNUI3RTU2NzA2MTMyNDNGMTZENkMiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJXbkQxcGhMN09aaF9mMXQtVm5CaE1rUHhiV3cifQ.eyJuYmYiOjE1MzE3NDI4NTYsImV4cCI6MTUzMTgwNzY1NiwiaXNzIjoiaHR0cHM6Ly9zdGFnaW5nLnZlcnR4LmNsb3VkL2lkZW50aXR5IiwiYXVkIjpbImh0dHBzOi8vc3RhZ2luZy52ZXJ0eC5jbG91ZC9pZGVudGl0eS9yZXNvdXJjZXMiLCJ2ZXJ0ZXhfYXV0aF9hcGkiLCJ2ZXJ0ZXhfY29yZV9hcGkiLCJ2ZXJ0ZXhfZ2l0cGlwZWxpbmVfYXBpIiwidmVydGV4X2dsdGZwaXBlbGluZV9hcGkiLCJ2ZXJ0ZXhfaW1wb3J0ZXJfYXBpIiwidmVydGV4X3NjcmlwdF9hcGkiLCJ2ZXJ0ZXhfc2Vzc2lvbl9hcGkiXSwiY2xpZW50X2lkIjoidmVydGV4X3dlYl9mcm9udGVuZCIsInN1YiI6IjU4NTM2ODBmLWYxZDUtNGI3MS00ZDEzLTA4ZDVkYTk4ZmExNSIsImF1dGhfdGltZSI6MTUzMTc0Mjg1NiwiaWRwIjoiTWljcm9zb2Z0QXp1cmVBZCIsIkFzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiNjI2OTc5ZWEtNmE3ZS00YTMyLThmN2UtMzNmMDFmMjUyOTIxIiwidmVydGV4X3RlbmFudCI6WyJkYWQzMjI4ZC0yYjJkLTQ2MTEtNzM4MC0wOGQ1ZGE3YTQ0ZjIiLCJkYWQzMjI4ZC0yYjJkLTQ2MTEtNzM4MC0wOGQ1ZGE3YTQ0ZjIiXSwidmVydGV4X3RjIjoiMSIsInByZWZlcnJlZF91c2VybmFtZSI6InNhbnRvc2gudmlzaHdha2FybWFAdmlzcmFjY2VsZXJhdG9yLm9ubWljcm9zb2Z0LmNvbSIsImVtYWlsIjoic2FudG9zaC52aXNod2FrYXJtYUB2aXNyYWNjZWxlcmF0b3Iub25taWNyb3NvZnQuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJuYW1lIjoiU2FudG9zaCBWaXNod2FrYXJtYSIsInZlcnRleF9saWNlbmNlZCI6IjEiLCJzY29wZSI6WyJvcGVuaWQiLCJwcm9maWxlIiwiZW1haWwiLCJ2ZXJ0ZXhfcHJvZmlsZSIsInZlcnRleF9hdXRoX2FwaSIsInZlcnRleF9jb3JlX2FwaSIsInZlcnRleF9naXRwaXBlbGluZV9hcGkiLCJ2ZXJ0ZXhfZ2x0ZnBpcGVsaW5lX2FwaSIsInZlcnRleF9pbXBvcnRlcl9hcGkiLCJ2ZXJ0ZXhfc2NyaXB0X2FwaSIsInZlcnRleF9zZXNzaW9uX2FwaSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJleHRlcm5hbCJdfQ.b7q1-hysGLQHnAmb4waX8E3Nnlb-JgwKGhwMtPMwUHhi7d-qVEGmdi5I3QtU9YaiNn1VW9WHKG18_BnYliWk2bbTzepq30c8h5KKgysclBbmq62crZexld1mZx6pRQBjlltFbjEEm2_be1janx028VFf8hkN13M60wMfeIYeYiRr__oT_Z3VxRtkrDrmpdd09-VE7-ULm8cLStL9D92NxpmVdZHU5lyPegaOXeaM51HHTWxxeeh2ESYBdyh4XCtWMotAZUhQuHVth9Ol8caFoPNHoGYSya5oJVrkXI2YbpGUrEn-SjtL99raqzUinFz2oZwQyYBwMYlG7kcW1ScGbA");

            //Create pin Directory and contents to keep track of its current status
            if (!Directory.Exists("/sys/class/gpio/gpio7/"))
            {
                File.WriteAllText("/sys/class/gpio/export", "7");
                File.WriteAllText("/sys/class/gpio/gpio7/direction", "in");
            }

            //Constant service running to check state change
            while (true)
            {
                string currentCharge = File.ReadAllText("/sys/class/gpio/gpio7/value");
                if (!currentCharge.Equals(previousReading))
                {
                    Console.WriteLine("previous reading is " + previousReading + ", currentCharge is " + currentCharge);
                    Console.WriteLine("Client sending to VERTX");
                    string testStr = "{\"Name\" : \"Switch1\", \"Status\" : " + currentCharge + "}";
                    client.UploadData("/core/v1.0/resource/b13daeb3-d3db-4d7d-bb3c-87fa090b58b8/BoxStatus.json", System.Text.UTF8Encoding.UTF8.GetBytes(testStr));
                    //client.UploadData("session/fire/b13daeb3-d3db-4d7d-bb3c-87fa090b58b8/9bc4ca9a-088f-444c-9805-a58b4e989969/SwitchChanged", System.Text.UTF8Encoding.UTF8.GetBytes("newChargeValue"));
                    Console.WriteLine("Client finished writing to VERTX");
                    previousReading = currentCharge;
                }
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
