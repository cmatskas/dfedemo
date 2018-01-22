using System;

namespace Models
{
	public partial class UserCase
    {
        public int CaseId { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Laid { get; set; }
        public bool? CaseStatus { get; set; }
        public bool? Bullying { get; set; }
        public bool? Absence { get; set; }
        public bool? Other { get; set; }
        public string UserId { get; set; }
    }
}
