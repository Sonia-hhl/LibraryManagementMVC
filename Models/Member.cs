using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement2.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name ="Pass")]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Display(Name ="PhoneNum")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "SignUpDate")]
        [DataType(DataType.Date)]
        public DateTime SignUpDate { get; set; }

        [Display(Name ="Loans")]
        public List<Loan> Loans{ get; set; } = [];

        [Display(Name = "Fine")]
        public int Fine { get; set; } = 0;
        [Display(Name = "Reserve Left")]
        public int MaxReserveNum { get; set; } = 5;
        public bool IsAdmin { get; set; } = false;

    }
}
