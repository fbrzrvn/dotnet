using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApp.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3), Required]
        public string Title { get; set; }

        [Display(Name = "Release Date"), DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\/\s]*$"), StringLength(30), Required]
        public string Genre { get; set; }

        [Range(1, 10), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Rating { get; set; }
    }
}
