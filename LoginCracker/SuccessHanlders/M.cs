using System;
using System.Net;
using Newtonsoft.Json;

namespace LoginCracker.SuccessHanlders
{
    public class MHandler : ISuccessHandler
    {
        public Func<HttpStatusCode, string, bool> HandlerStatusCode
        {
            get
            {
                return (code, rep) =>
                {
                    if (string.IsNullOrEmpty(rep))
                    {
                        return false;
                    }
                    var obj = JsonConvert.DeserializeObject<dynamic>(rep);
                    var statu = obj.Statu;
                    return statu == "Y";
                };
            }
        }
    }
}