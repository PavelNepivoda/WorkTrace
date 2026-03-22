using System.ComponentModel.DataAnnotations;

namespace WorkTrace.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [StringLength(4)]
        public string PINCode { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }

        public Guid? BranchId { get; set; }
        public Branch? Branch { get; set; }

        public Guid? RoleId { get; set; }
        public EmployeeRole? Role { get; set; }

        public Guid? ContractTypeId { get; set; }
        public ContractType? ContractType { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}