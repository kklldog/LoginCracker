using System;
using System.Collections.Concurrent;
using System.IO;

namespace LoginCracker
{
    internal class HttpFileReader
    {
        public HttpFileReader(string filePath)
        {
            FilePath = filePath;
            var line = "";
            var counter = 0;
            var queryString = "";
            Headers = new ConcurrentDictionary<string, string>();
            using (var file =
                new StreamReader(filePath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    Console.WriteLine(line);
                    counter++;
                    if (counter == 1)
                    {
                        var arr = line.Split(' ');
                        Method = arr[0];
                        queryString = arr[1];
                        continue;
                    }

                    var headerArr = line.Split(new string[1] {": "}, StringSplitOptions.RemoveEmptyEntries);
                    if (headerArr.Length == 1)
                    {
                        BodyPatten = line;
                        continue;
                    }

                    var key = headerArr[0];
                    var value = headerArr[1];
                    if (key == "Content-Length" || key == "Host"
                        || key == "Connection" || key == "Referer"
                        || key == "Accept")
                    {
                        continue;
                    }
                    if (key == "Origin")
                    {
                        RequestUrl = value + queryString;
                        continue;
                    }
                    if (key == "Content-Type")
                    {
                        ContentType = value;
                        continue;
                    }
                    if (key == "User-Agent")
                    {
                        UserAgent = value;
                        continue;
                    }

                    Headers.TryAdd(key, value);
                }
            }
        }

        public string Method { get; set; }

        public string RequestUrl { get; set; }

        public string ContentType { get; set; }

        public string UserAgent { get; set; }

        public ConcurrentDictionary<string, string> Headers { get; set; }

        public string BodyPatten { get; set; }

        public string FilePath { get; }
    }
}