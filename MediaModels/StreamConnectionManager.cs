using System;
namespace MediaModels
{
    public static class StreamConnectionManager
    {

        public static HttpConnectionAttributes STREAMSERVER = new HttpConnectionAttributes { SSL = true, HostIP = "heights.treedays.cloud", Port = "", Route = "nms" };

        public static HttpConnectionAttributes NOTIFICATIONSERVER = new HttpConnectionAttributes { SSL = true, HostIP = "localhost", Port = "5001", Route = "notificationhub" };

        public static HttpConnectionAttributes CHATSERVER = new HttpConnectionAttributes { SSL = true, HostIP = "localhost", Port = "5001", Route = "chathub" };

        public static HttpConnectionAttributes LIVESTREAMNOTIFICATIONSERVER = new HttpConnectionAttributes { SSL = true, HostIP = "localhost", Port = "5001", Route = "livestreamhub" };

        public static HttpConnectionAttributes MediaApi = new HttpConnectionAttributes { SSL = true, HostIP = "localhost", Port = "5001", Route = "" };


    }
}
