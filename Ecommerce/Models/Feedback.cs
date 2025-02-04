using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
    }
}
