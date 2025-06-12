using System.ComponentModel.DataAnnotations;

namespace LibraryManagement2.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Range(0, 5)]
        [Required(ErrorMessage = "Rating is required.")]
        [Display(Name = "Rating")]
        public float Rating { get; set; }
        [Display(Name = "Comment")]
        public string Comment { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Display(Name = "BookId")]
        public int BookId { get; set; }
        [Display(Name = "Book")]
        public Book? Book { get; set; }
        [Required]
        public int MemberId { get; set; }
        public Member? Member { get; set; }

    }
}
