using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DfeDemo.Models
{
    public partial class UserCase
    {
		public UserCase() { }
        public int CaseId { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
		[Required]
        public DateTime? DateOfBirth { get; set; }
        public int? Laid { get; set; }
        public bool? CaseStatus { get; set; }
        public bool? Bullying { get; set; }
        public bool? Absence { get; set; }
        public bool? Other { get; set; }
        public string UserId { get; set; }
    }
}
