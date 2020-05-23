using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediaModels
{
    public class UserConnection
    {
        [Key]
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public Connection Connection { get; set; }
        
    }
    
}