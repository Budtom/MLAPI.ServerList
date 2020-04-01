using System;
using System.Collections.Generic;
using System.Linq;
using MLAPI.ServerList.Client;
using MLAPI.ServerList.Shared;

namespace ClientExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Register 100 servers
            for (int i = 0; i < 10; i++)
            {
                ServerConnection advertConnection = new ServerConnection();

                // Connect
                advertConnection.Connect("127.0.0.1", 9423);

                var currentLocation = new Geolocation();
                currentLocation.type = "Point";
                currentLocation.coordinates = new float[] { -73.8560f, 40.8484f };

                // Create server data
                Dictionary<string, object> data = new Dictionary<string, object>
                    {
                        { "Players", (int)i },
                        { "Name", "This is the name" },
                        { "Geolocation", currentLocation }
                    };

                // Register server
                advertConnection.StartAdvertisment(data);
            }

            using (ServerConnection queryConnection = new ServerConnection())
            {
                // Connect
                queryConnection.Connect("127.0.0.1", 9423);

                // Send query
                List<ServerModel> models = queryConnection.SendQuery(@"
                {
                    ""$and"": [
                        {
                            ""Players"": {
                                ""$lte"": 20
                            }
},
                        {
                            ""$near"": {
                                ""$coordinates"": [ -73.8560, 40.8484 ],
                                ""$minDistance"": 0,
                                ""$maxDistance"": 2000
                            }
                        }
                    ]   
                }");

                //List<ServerModel> models = queryConnection.SendQuery(@"
                //{
                //    ""$near"": {
                //        ""$coordinates"": [ -73.8560, 40.8484 ],
                //        ""$minDistance"": 0,
                //        ""$maxDistance"": 2000
                //    }
                //}");

                Console.WriteLine(string.Format("| {0,5} | {1,5} | {2,5} |", "UUID", "Name", "Players"));
                Console.WriteLine(string.Join(Environment.NewLine, models.Select(x => string.Format("| {0,5} | {1,5} | {2,5} |", x.Id, x.ContractData["Name"], x.ContractData["Players"]))));
            }
        }
    }
}
