using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediaModels
{
    public class Connection
    {
        [Key]
        public string ConnectionId { get; set; }
        public string RoomName { get; set; }
        public bool Connected { get; set; }

        public virtual ICollection<UserConnection> UserConnections { get; set; }

    }
    
}