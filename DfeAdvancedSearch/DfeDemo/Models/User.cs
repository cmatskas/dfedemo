using System.ComponentModel.DataAnnotations;

namespace Models
{
	public partial class User
    {
        public string Id { get; set; }
		[Required]
        public string Firstname { get; set; }
		[Required]
		public string Surname { get; set; }
        public string Email { get; set; }
    }
}
