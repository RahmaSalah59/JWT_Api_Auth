using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace userIdentity.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        public string Name { get; set; } = string.Empty;

        // Foreign Key
        public int DepartmentId { get; set; }

        // Navigation Property
        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }
    }
}
