using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.Controllers
{
    public class SensorParser
    {
        private Dictionary<string, string> _parsedInput;
        private string _sensorName;

        public SensorParser()
        {
            _parsedInput = new Dictionary<string, string>();
        }

        public void ParseInput(byte[] sensorData)
        {
            var list = FromByteArrayToStringList(sensorData).ToList();
            _parsedInput = InputWithKeyValue(list);
        }

        private Dictionary<string, string> InputWithKeyValue(List<string> input)
        {
            _sensorName = input[0];
            input.RemoveAt(0);
            string[] splitOn = { ": " };
            var splitInput = new List<string>();


            foreach (string str in input)
            {
                splitInput.AddRange(str.Split(splitOn, StringSplitOptions.RemoveEmptyEntries).ToList());
            }

            if(splitInput.Count%2 != 0)
            {
                throw new ArgumentOutOfRangeException("Incorrect format of input");
            }
            var result = new Dictionary<string, string>();
            for(int i = 0; i < splitInput.Count; i+=2)
            {
                result.Add(splitInput[i], splitInput[i+1]);
            }

            return result;
        }


        private IEnumerable<string> FromByteArrayToStringList(byte[] byteArray)
        {
            var broadcast = Encoding.ASCII.GetString(byteArray);
            var stringList = broadcast.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            return stringList;
        }

    }
}