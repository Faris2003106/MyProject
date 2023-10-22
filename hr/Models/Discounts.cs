using System.ComponentModel.DataAnnotations;

namespace hr.Models
{
    public class Discounts
    {

       [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? TwitterUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InsatgramUrl { get; set; }
        public string? SnapchatUrl { get; set; }


        [Display(Name = "صورة")]
        [Required(ErrorMessage = "مطلوب")]
        public string ImageUrl { get; set; }

        public Boolean status { get; set; }

    }
}

