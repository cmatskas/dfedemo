using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class SearchCriteria
    {
		[Required]
		public string Name { get; set; }
		public List<Ladata> LaData { get; set; }
		[Required]
		public int LaValue { get; set; }
		public bool CaseStatus { get; set; }
		public bool Absence { get; set; }
		public bool Bullying { get; set; }
		public bool Other { get; set; }
	}
}
