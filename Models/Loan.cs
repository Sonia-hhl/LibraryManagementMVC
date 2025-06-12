using LibraryManagement2.Models;
using System.ComponentModel.DataAnnotations;
namespace LibraryManagement2.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Required]
        public int LoaneeId { get; set; }

        public Member Loanee { get; set; }

        [Required]
        public int BookId { get; set; }

        public Book LentBook { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; } = false;

    }
}
