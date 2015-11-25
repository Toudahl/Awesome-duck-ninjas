using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Helper
{
    class ParseSensorData
    {
        private Dictionary<string, string> _sensorsDict;

        public Dictionary<string, string> Sensors
        {
            get { return _sensorsDict; }
        }

        private Dictionary<string, string> _keywordsDict;

        public Dictionary<string, string> Keywords
        {
            get { return _keywordsDict; }
        }

        private List<Dictionary<string, string>> _sensorsWithKeywords;

        public List<Dictionary<string, string>> SensorsWithKeywords
        {
            get { return _sensorsWithKeywords; }
        }


        private readonly string[] _keywords = { "Location", "Platform", "Machine" };

        public ParseSensorData(byte[] input)
        {
            var str = System.Text.Encoding.UTF8.GetString(input);
            var lines = str.Split(new string[] { "\r\n" }, StringSplitOptions.None).Where(x => x.Split(':').Length == 2).ToList();

            _keywordsDict = parseKeywords(lines);
            _sensorsDict = parseSensors(lines);

            _sensorsWithKeywords = new List<Dictionary<string, string>>();
            foreach (var sensor in _sensorsDict)
            {
                var sensorWithKeyword = new Dictionary<string, string>(_keywordsDict);
                sensorWithKeyword.Add(sensor.Key, sensor.Value);
                _sensorsWithKeywords.Add(sensorWithKeyword);
            }
        }

        private Dictionary<string, string> parseKeywords(List<string> lines)
        {
            var keywordDict = new Dictionary<string, string>();

            foreach (var keyword in _keywords)
            {
                var line = lines.FirstOrDefault(x => x.Contains(keyword));
                var lineSplits = line.Split(':');

                lineSplits[1] = lineSplits[1].Trim();
                lineSplits[0] = lineSplits[0].Trim();

                keywordDict.Add(lineSplits[0], lineSplits[1]);
            }
            return keywordDict;
        }


        private Dictionary<string, string> parseSensors(List<string> lines)
        {
            var sensorDict = new Dictionary<string, string>();

            foreach (var keyword in _keywords)
            {
                lines.Remove(lines.FirstOrDefault(x => x.Contains(keyword)));
            }

            foreach (var line in lines.Where(x => x.Contains(':')))
            {
                var lineSplits = line.Split(':');

                lineSplits[1] = lineSplits[1].Trim();
                lineSplits[0] = lineSplits[0].Trim();
                sensorDict.Add(lineSplits[0], lineSplits[1]);
            }

            return sensorDict;
        }

    }
}