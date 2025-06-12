using System.ComponentModel.DataAnnotations;

namespace LibraryManagement2.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Author is required.")]
        [Display(Name = "Author")]
        public string Author { get; set; }
        [RegularExpression(@"\d{13}", ErrorMessage = "ISBN must be a 13-digit number.")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Published Date")]
        public DateTime PublishedDate { get; set; }
        [Display(Name = "Loan History")]
        public List<Loan> LoansHistory { get; set; }
        [Display(Name = "Donor")]
        public Member donor { get; set; }
        [Display(Name = "Available Copies")]
        public int AvailableCopies  { get; set; }
        [Display(Name = "Genre")]
        public string Genre { get; set; }
        [Display(Name = "Reviews")]
        public List<Review> Reviews { get; set; } = new List<Review>();
        [Display(Name = "Average Rating")]
        public float Stars => Reviews.Count == 0
            ? 0
            : (float)Math.Round(Reviews.Average(r => r.Rating), 2);
    }
}
