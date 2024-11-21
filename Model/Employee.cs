using System.ComponentModel.DataAnnotations;

namespace DynamicGridGeneration.Model
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
        public string? Department { get; set; }
    }
}
