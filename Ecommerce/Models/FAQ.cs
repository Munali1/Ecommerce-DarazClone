using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class FAQ
    {
        [Key]
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer {  get; set; }

    }
}
