// See https://aka.ms/new-console-template for more information
using System;
using System.Net;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Data.Sqlite;

//* Console app which listens to Ambient Weather 
//* José Oliver-Didier - https://github.com/josemoliver
//* Reference: https://ambientweather.com/faqs/question/view/id/1857


Console.WriteLine("Weather Server Started");

var settingsJSON = System.IO.File.ReadAllText(@"Settings.json"); // Fetch user defined config from Settings.json

string port     = JObject.Parse(settingsJSON)["port"].ToString().Trim();   //port number to use
string path     = JObject.Parse(settingsJSON)["path"].ToString().Trim();   //path as defined in the Awnet Advanced Settings - important add question mark at the end ?
string dbPath   = JObject.Parse(settingsJSON)["dbPath"].ToString().Trim(); //file system path for SQLlite db WeatherStation.sqlite


using (var listener = new HttpListener())
    {
        listener.Prefixes.Add("http://*:"+port+"/"+path);

        listener.Start();

        while (true) // Keep looping listening for station readings. Httplistener ref: https://learn.microsoft.com/en-us/dotnet/api/system.net.httplistener?view=net-6.0
    {

            Console.WriteLine("Listening...");

            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;


            using (HttpListenerResponse response = context.Response)
            {


            Microsoft.Data.Sqlite.SqliteConnection m_dbConnection = new SqliteConnection("Data Source="+ dbPath +"WeatherStation.sqlite;"); // SQLite db 
            m_dbConnection.Open();

            string sqlCommandText = "";
            string valueparams = "";
            string keyparams = "";
            string dateutc = "";

            foreach (string key in request.QueryString.AllKeys)
                {
                    //Console.WriteLine(key + " = " + request.QueryString[key]);

                    string value = "";
                    string keyname = "";
   

                    try
                    {
                        keyname = key; 
                        value = request.QueryString[key].ToString().Trim();

                        if (keyname == "dateutc")
                        {
                            dateutc = value;
                        }

                        keyparams = keyparams + keyname + ",";
                        valueparams = valueparams +"\""+ value + "\",";
                    }
                    catch
                    {
                    }

                }

            keyparams = keyparams.Trim(',');
            valueparams = valueparams.Trim(',');

            sqlCommandText = @"INSERT INTO LocalReadings ("+keyparams+") VALUES ("+valueparams+")"; // Note: You will need to create a db and LocalReadings table refer to Create_LocalReadings_table.sql and edit the command for your PWS model.

            Microsoft.Data.Sqlite.SqliteCommand insertBatchID = new SqliteCommand(sqlCommandText, m_dbConnection);

            insertBatchID.ExecuteNonQuery();
            m_dbConnection.Close();
            Console.WriteLine(dateutc);

            }
        }
    }
