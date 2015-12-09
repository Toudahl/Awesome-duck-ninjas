using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Helper
{
    public class SensorParser
    {
        private Broadcaster _broadcaster;
        private Location _location;
        private Model1 _dbConnection;

        /// <summary>
        /// Takes a byte array, convert it to strings. Parses it, and saves it to the database.
        /// </summary>
        /// <param name="sensorData">sensor data</param>
        /// <returns>Ok, on success, BadRequest on failure</returns>
        public async Task<HttpStatusCode> ParseInput(byte[] sensorData)
        {
            using(_dbConnection = new Model1())
            {
                var stringList = FromByteArrayToStringList(sensorData).ToList();
                var dictionary = await ListToDictionaryAsync(stringList);
                var sensorValues = await ToSensorValueDictionaryAsync(dictionary);
                return AddToDatabase(sensorValues);
            }
        }

        /// <summary>
        /// This will store the data in the database.
        /// </summary>
        /// <param name="sensorValues">Dictionary containing the sensor as key, and the value as - well - value</param>
        /// <returns>OK if everything was added correctly to the db. BadRequest if something went wrong.</returns>
        private HttpStatusCode AddToDatabase(IDictionary<Sensor,Value> sensorValues)
        {
            try
            {
                foreach(KeyValuePair<Sensor, Value> keyValuePair in sensorValues)
                {
                    _dbConnection.Sensors.Add(keyValuePair.Key);
                    _dbConnection.SaveChanges();

                    keyValuePair.Value.FK_Sensor = keyValuePair.Key.Id;
                    _dbConnection.Values.Add(keyValuePair.Value);
                    _dbConnection.SaveChanges();
                }
                return HttpStatusCode.OK;
            }
            catch(DbEntityValidationException ex)
            {
                foreach(var dbEntityValidationResult in ex.EntityValidationErrors)
                {
                    foreach(DbValidationError dbValidationError in dbEntityValidationResult.ValidationErrors)
                    {
                        Debug.WriteLine($"error message: {dbValidationError.ErrorMessage}\nproperty name: {dbValidationError.PropertyName}");
                    }
                }
            }
            catch(Exception x)
            {
                Debug.WriteLine("unknown exception: " + x.GetType());
            }
            return HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// This will take the "string,string" dictionary, and turn it into "sensor,value" dictionary.
        /// </summary>
        /// <param name="stringDictionary"></param>
        /// <returns></returns>
        private async Task<Dictionary<Sensor, Value>> ToSensorValueDictionaryAsync(IDictionary<string,string> stringDictionary)
        {
            Dictionary<Sensor, Value> dictionary = new Dictionary<Sensor, Value>();
                foreach (KeyValuePair<string, string> keyValuePair in stringDictionary)
                {
                    var sensorType = await GetSensorTypeAsync(keyValuePair.Key);
                    var sensor = await GetSensorAsync(_broadcaster.Id,_location.Id, sensorType.Id);
                    //var sensor = new Sensor //TODO modify here, if there should only be only one entry for each sensor in the DB
                    //             { 
                    //                 Fk_Broadcaster = _broadcaster.Id,
                    //                 Fk_Location = _location.Id,
                    //                 Fk_SensorType = sensorType.Id
                    //             };

                    dictionary[sensor] = new Value {ValueInput = keyValuePair.Value};
                }
            return dictionary;
        }

        private async Task<Sensor> GetSensorAsync(int broadcasterId, int locationId, int sensorTypeId)
        {
            var sensor = await _dbConnection.Sensors.FirstOrDefaultAsync(
                s => s.Fk_Broadcaster == broadcasterId 
                && s.Fk_Location == locationId 
                && s.Fk_SensorType == sensorTypeId);
            if(sensor != null)
            {
                return sensor;
            }

            sensor = new Sensor //TODO modify here, if there should only be only one entry for each sensor in the DB
            {
                Fk_Broadcaster = broadcasterId,
                Fk_Location = locationId,
                Fk_SensorType = sensorTypeId
            };

            _dbConnection.Sensors.Add(sensor);
            await _dbConnection.SaveChangesAsync();

            return sensor;
        }

        /// <summary>
        /// Either gets the sensor type from the DB, or creates a new one.
        /// </summary>
        /// <param name="type">Name of the requested type</param>
        /// <returns>SensorType object</returns>
        private async Task<SensorType> GetSensorTypeAsync(string type)
        {
            var sensorType = await _dbConnection.SensorTypes.FirstOrDefaultAsync(s => s.Type.ToLower() == type.ToLower());
            if(sensorType != null)
            {
                return sensorType;
            }

            sensorType = new SensorType {Type = FirstLetterToUpperRestLower(type)};
            _dbConnection.SensorTypes.Add(sensorType);
            await _dbConnection.SaveChangesAsync();

            return sensorType;
        }

        /// <summary>
        /// Will take a list of string. Split them, and return a dictionary of "string,string"
        /// </summary>
        /// <param name="input">List of strings.</param>
        /// <returns>Dictionary</returns>
        private async Task<Dictionary<string, string>> ListToDictionaryAsync(IList<string> input)
        {
            _broadcaster = await GetBroadcasterAsync(input[0]);

            input.RemoveAt(0);
            string[] splitOn = { ": " };
            var splitInput = new List<string>();


            foreach (string str in input)
            {
                splitInput.AddRange(str.Split(splitOn, StringSplitOptions.RemoveEmptyEntries).ToList());
            }

            if (splitInput.Count % 2 != 0)
            {
                throw new ArgumentOutOfRangeException("Incorrect format of input");
            }
            var result = new Dictionary<string, string>();
            for (int i = 0; i < splitInput.Count; i += 2)
            {
                result.Add(splitInput[i], splitInput[i + 1]);
            }

            _location = await GetLocationAsync(result["Location"]);
            result.Remove("Location");
            result.Remove("Platform");
            result.Remove("Machine");

            return result;
        }

        /// <summary>
        /// Either gets the location from the db, or adds a new one.
        /// </summary>
        /// <param name="name">Location name</param>
        /// <returns>Location object</returns>
        private async Task<Location> GetLocationAsync(string name)
        {
                Location location = await _dbConnection.Locations.FirstOrDefaultAsync(b => b.Name.ToLower() == name.ToLower());

                if(location != null)
                {
                    return location;
                }

                location = new Location {Name = FirstLetterToUpperRestLower(name) };

                _dbConnection.Locations.Add(location);
                await _dbConnection.SaveChangesAsync();

                return location;
            
        }

        /// <summary>
        /// Either gets the broadcaster from the db, or adds a new one.
        /// </summary>
        /// <param name="name">name of the broadcaster</param>
        /// <returns>Broadcaster object</returns>
        private async Task<Broadcaster> GetBroadcasterAsync(string name)
        {
                Broadcaster broadcaster = await _dbConnection.Broadcasters.FirstOrDefaultAsync(b => b.Name.ToLower() == name.ToLower());

                if(broadcaster != null)
                {
                    return broadcaster;
                }

                broadcaster = new Broadcaster {Name = FirstLetterToUpperRestLower(name) };

                _dbConnection.Broadcasters.Add(broadcaster);
                await _dbConnection.SaveChangesAsync();

                return broadcaster;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        private IEnumerable<string> FromByteArrayToStringList(byte[] byteArray)
        {
            var broadcast = Encoding.ASCII.GetString(byteArray);
            return broadcast.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        private string FirstLetterToUpperRestLower(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1)
                                                 .ToLower();
        }


    }
}