using System;
namespace MediaModels
{
    public class HttpConnectionAttributes
    {
        public bool SSL { get; set; }
        public string HostIP { get; set; }
        public string Port { get; set; }
        public string Route { get; set; } = "";

        public string GetURL()
        {

            if (!string.IsNullOrEmpty(Port))
            {
                if (SSL)
                {
                    return $"https://{HostIP}:{Port}/{Route}";
                }
                else
                {
                    return $"http://{HostIP}:{Port}/{Route}";
                }
            }

            else return $"http://{HostIP}/{Route}";
        }


        public HttpConnectionAttributes()
        {

        }

        public HttpConnectionAttributes(bool sSL, string hostIP, string port, string route)
        {
            SSL = sSL;
            HostIP = hostIP;
            Port = port;
            Route = route;
        }
    }
}
