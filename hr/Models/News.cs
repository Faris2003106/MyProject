using System.ComponentModel.DataAnnotations;

namespace hr.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required]

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public string NewsUrl { get; set; }

        [Display(Name = "صورة")]
        [Required(ErrorMessage = "مطلوب")]
        public string ImageUrl { get; set; }
        public Boolean status { get; set; }



    }
}
