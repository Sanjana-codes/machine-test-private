using System.ComponentModel.DataAnnotations;

namespace MachineTestProject.Models
{
    public class Product
    {

        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

    }
}
