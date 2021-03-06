﻿using System;
using System.Net;

namespace LoginCracker.SuccessHanlders
{
    public class DefaultHandler : ISuccessHandler
    {
        public Func<HttpStatusCode, string, bool> HandlerSuccessResponse
        {
            get
            {
                return (code, rep) =>
                {
                    return code == HttpStatusCode.Found
                           || code == HttpStatusCode.Redirect;
                };
            }
        }

        public bool IsStopAllTasksWhenFindOne {
            get { return false; }
        }
    }
}