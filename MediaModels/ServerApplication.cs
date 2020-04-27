using System;
namespace MediaModels
{
    public class ServerApplication
    {

        public string Address { get; set; }
        public string Port { get; set; }
        public string AppName { get; set; }

        public ServerApplication(string address, string port, string appName)
        {
            Address = address;
            Port = port;
            AppName = appName;
        }

        public ServerApplication()
        {
        }
    }


}
