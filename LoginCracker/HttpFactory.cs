using System;
using System.Net;
using System.Text;

namespace LoginCracker
{
    internal class HttpFactory
    {
        public static HttpWebRequest CreateHttp(RequestFileReader httpFileReader, string userName, string password)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(httpFileReader.RequestUrl);
                request.AllowAutoRedirect = false;
                request.Method = httpFileReader.Method;
                if (!string.IsNullOrEmpty(httpFileReader.ContentType))
                {
                    request.ContentType = httpFileReader.ContentType;
                }
                if (!string.IsNullOrEmpty(httpFileReader.UserAgent))
                {
                    request.UserAgent = httpFileReader.UserAgent;
                }

                //add headers
                foreach (var header in httpFileReader.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                if (!string.IsNullOrEmpty(httpFileReader.BodyPatten))
                {
                    //add body
                    var body = string.Format(httpFileReader.BodyPatten, userName, password);
                    var dataToPost = Encoding.UTF8.GetBytes(body);
                    request.ContentLength = dataToPost.Length;
                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(dataToPost, 0, dataToPost.Length);
                    }
                }


                return request;
            }
            catch (Exception exc)
            {

                Console.WriteLine(exc);
            }

            return null;

        }
    }
}