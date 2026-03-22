using System.ComponentModel.DataAnnotations;

namespace WorkTrace.Models.ViewModel
{
    public class EmployeesVM
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
