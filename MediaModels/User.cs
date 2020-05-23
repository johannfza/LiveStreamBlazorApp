using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediaModels
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        public string UserName { get; set;}
        public ICollection<UserConnection> Connections { get; set; }

        
    }
    
}