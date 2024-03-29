﻿using Newtonsoft.Json;
using Scylla.Net;
using station.meteo.family.Models;

namespace station.meteo.family.Services
{
    public class DBService
    {
        private IConfiguration _Configuration;  //Configuration Load
        private ILogger<DBService> _Logger; //Logger



        public DBService(IConfiguration conf, ILogger<DBService> logger)
        {
            _Logger = logger;
            _Configuration = conf;
        }

        /// <summary>
        /// Initialize DB Connection
        /// </summary>
        /// <returns></returns>
        private Scylla.Net.ISession DBConnectionBuilder()
        {
            Cluster cluster = Cluster.Builder().AddContactPoints(_Configuration.GetSection("DBContactPoints").Get<List<string>>().ToArray()).Build();   //Initialize Cluster
            return cluster.Connect(_Configuration["DBKeyspace"]); //Connect to DB Cluster
        }


        /// <summary>
        /// Save Meteo Station Data to DB
        /// </summary>
        /// <param name="ID">Station ID</param>
        /// <param name="data">Data to Save</param>
        public void SaveDataToDB(long ID, V1Uri data)
        {
            SaveDataToDB(ID, (V1Body)data);
        }

        /// <summary>
        /// Save Meteo Station Data to DB
        /// </summary>
        /// <param name="ID">Station ID</param>
        /// <param name="data">Data to Save</param>
        public async void SaveDataToDB(long ID, V1Body data)
        {
            Scylla.Net.ISession? conn;
            try
            {
                conn = DBConnectionBuilder();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Error with connecting to DB: {ex.Message}");
                return;
            }

            try
            {
                await Task.Run(() => conn.Execute($"INSERT INTO meteostation_data (mid, datetime, temp_air5, temp_air200, humidity_air, pressure_air, wind_speed, wind_direction, rain_quantity, other) VALUES ({ID}, {DateTime.ParseExact(data.DateTime, "yyyy-MM-dd_HH-mm-ss", null)}, {data.Temp5}, {data.Temp200}, {data.Humidity}, {data.Pressure}, {data.WindSpeed}, {data.WindDirection}, {data.RainQuantity}, '{JsonConvert.SerializeObject(data.Other)}')"));    //Code for Save Data to DB
                _Logger.LogDebug($"Meteostation ({ID}) added data do DB");
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Error when adding data to DB from Meteostation ({ID}) with message: {ex.Message}");
            }
        }

        /// <summary>
        /// Check if request has access to write as selected station
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Token"></param>
        public bool StationWriteAccess(long ID, string Token)
        {
            Scylla.Net.ISession? conn;
            try
            {
                conn = DBConnectionBuilder();
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Error with connecting to DB: {ex.Message}");
                return false;
            }

            try {
                var rows = conn.Execute($"SELECT mid FROM meteostation WHERE mid = {ID} AND token = '{Token}'");

                if (rows != null && rows.Count() > 0)
                {
                    _Logger.LogDebug($"Meteostation {ID} authentificated");
                    return true;
                }
                else
                {
                    _Logger.LogDebug($"Meteostation {ID} is not authentificated");
                    return false;
                }
            }
            catch ( Exception ex )
            {
                _Logger.LogError($"Error when authentification meteostation {ID} with message: {ex.Message}");
                return false;
            }
        }










    }
}
