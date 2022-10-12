using System.ComponentModel.DataAnnotations;

namespace SunriseCo.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [MinLength(5, ErrorMessage = "Name mustn't be less than 5 characters")]
        [MaxLength(50, ErrorMessage = "Name mustn't be exceed 50 characters")]
        public string Name { get; set; }

        public string Address { get; set; }
    }
}
