﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DfeDemo.Models
{
    public partial class User
    {
		public User() { }
        public string Id { get; set; }
		[Required]
        public string Firstname { get; set; }
		[Required]
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
