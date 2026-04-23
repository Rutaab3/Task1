using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "EmpNo")]
        [StringLength(50)]
        public string? EmpNo { get; set; }

        [Required]
        [Display(Name = "Class")]
        [StringLength(50)]
        public string? CabinName { get; set; }

        [Range(18, 100)]
        public int Age { get; set; }
    }
}
