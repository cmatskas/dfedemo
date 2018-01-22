using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DfeDemo.Models
{
    public class SearchCriteria
    {
		public string Name { get; set; }
		public int LaValue { get; set; }
		public bool CaseStatus { get; set; }
		public bool Absence { get; set; }
		public bool Bullying { get; set; }
		public bool Other { get; set; }

	}
}
