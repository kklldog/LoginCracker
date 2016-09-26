using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LoginCracker.SuccessHanlders;

namespace LoginCracker
{
    internal class HttpTask
    {
        private static readonly CancellationTokenSource cancellationToken = new CancellationTokenSource();

        public static Task<string> CreateTask(HttpWebRequest request, ISuccessHandler successHandler, string userName,
            string password)
        {
            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var response = (HttpWebResponse) request.GetResponse())
                    {
                        var responseStream = response.GetResponseStream();
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                            {
                                var content = reader.ReadToEnd();
                                if (successHandler.HandlerStatusCode(response.StatusCode, content))
                                {
                                    cancellationToken.Cancel();

                                    Console.WriteLine(content);
                                    return string.Format("SUCCESS!!!! {0} {1} {2}", response.StatusCode, userName,
                                        password);
                                }
                            }
                        }

                        return "";
                    }
                }
                catch (WebException ex)
                {
                    using (var response = (HttpWebResponse) ex.Response)
                    {
                        var responseStream = response.GetResponseStream();
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                            {
                                var content = reader.ReadToEnd();
                                Console.WriteLine(content);
                                return "";
                            }
                        }
                    }
                }

                return "";
            }, cancellationToken.Token);

            return task;
        }
    }
}