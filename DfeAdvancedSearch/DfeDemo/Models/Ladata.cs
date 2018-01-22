using System.ComponentModel.DataAnnotations;

namespace Models
{
	public partial class Ladata
    {
        public int Id { get; set; }

		[Required]
        public string Name { get; set; }
    }
}
