using System;
using System.Collections.Generic;

namespace DfeDemo.Models
{
    public partial class User
    {
		public User() { }
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
