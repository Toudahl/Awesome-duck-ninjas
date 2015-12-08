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

        public SensorParser()
        {
        }

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

        private async Task<Dictionary<Sensor, Value>> ToSensorValueDictionaryAsync(IDictionary<string,string> stringDictionary)
        {
            Dictionary<Sensor, Value> dictionary = new Dictionary<Sensor, Value>();
                foreach (KeyValuePair<string, string> keyValuePair in stringDictionary)
                {
                    var sensorType = await GetSensorTypeAsync(keyValuePair.Key);
                    var sensor = new Sensor //TODO modify here, if there should only be only one entry for each sensor in the DB
                                 { 
                                     Fk_Broadcaster = _broadcaster.Id,
                                     Fk_Location = _location.Id,
                                     Fk_SensorType = sensorType.Id
                                 };

                    dictionary[sensor] = new Value {ValueInput = keyValuePair.Value};
                }
            return dictionary;
        }

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

            return result;
        }

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