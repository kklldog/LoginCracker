using LoginCracker.SuccessHanlders;

namespace LoginCracker
{
    internal class SuccessHandlerConfig
    {
        private static ISuccessHandler _handler;

        public static ISuccessHandler Handler
        {
            get
            {
               // return _handler ?? (_handler = new DefaultHandler());
                return _handler??(_handler=new MHandler());
            }
        }
    }
}