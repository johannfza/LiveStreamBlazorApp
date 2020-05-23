using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediaModels
{
    public class ChatUser
    {
        public string UserName { get; set; }
        public string RoomName { get; set; }

        public ChatUser(string username, string roomname)
        {
            UserName = username;
            RoomName = roomname;
        }
    }
    
}