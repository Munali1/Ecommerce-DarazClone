using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Ecommerce.Models
{
    public class Category
    {
        [Key]
        public int Id {  get; set; }
        
        public string category_Name {  get; set; }

        public List<Product> products { get; set; }
    }
}
