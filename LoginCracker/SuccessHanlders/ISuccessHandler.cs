using System;
using System.Net;

namespace LoginCracker.SuccessHanlders
{
    public interface ISuccessHandler
    {
        Func<HttpStatusCode, string, bool> HandlerStatusCode { get; }

        bool IsStopAllTasksWhenFindOne { get; }
    }
}