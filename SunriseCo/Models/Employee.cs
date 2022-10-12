using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunriseCo.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [DisplayName("Full Name")]
        [Required(ErrorMessage ="Full name is required.")]
        [MinLength(12, ErrorMessage ="Full name mustn't be less than 12 Characters")]
        [MaxLength(50, ErrorMessage = "Full name mustn't be less than 12 Characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Provide a valid Position")]
        [MinLength(2, ErrorMessage = "Full name mustn't be less than 2 Characters")]
        [MaxLength(20, ErrorMessage = "Full name mustn't be less than 20 Characters")]
        public string Position { get; set; }

        [Range(2500, 25000, ErrorMessage = "Salary must be between 2500 and 25000")]
        public double Salary { get; set; }

        [Range(1000, double.MaxValue, ErrorMessage = "Bonus mustn't be less than 1000")]
        public double Bonus { get; set; }

        [DisplayName("Phone Number")]
        [RegularExpression("^0\\d{10}$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [NotMapped]
        [Compare("Email", ErrorMessage = "Email and confirm email don't match")]
        public string ConfirmEmail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage = "Password and confirm Password don't match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DisplayName("Hiring Date & Time")]
        public DateTime HiringDateTime { get; set; }

        [DisplayName("Birth Date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [DisplayName("Attendance Time")]
        [DataType(DataType.Time)]
        public DateTime AttendanceTime { get; set; }

        [DisplayName("Leaving Time")]
        [DataType(DataType.Time)]
        public DateTime LeavingTime { get; set; }

        [DisplayName("Created At")]
        [ValidateNever]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Last Update At")]
        [ValidateNever]
        public DateTime LastUpdateAt { get; set; }

        [DisplayName("Department")]
        [Range(1, int.MaxValue, ErrorMessage = "Choose a valid department")]
        public int DepartmentId { get; set; }

        // Navigation Property
        [ValidateNever]
        public Department Department { get; set; }

        [DisplayName("Iamge")]
        [ValidateNever]
        public string ImageUrl { get; set; }

    }
}
