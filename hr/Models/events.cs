using System.ComponentModel.DataAnnotations;

namespace hr.Models
{
    public class events
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Organization { get; set; }

        public string? description { get; set; }

        public DateTime? CreateDate { get; set; }

        [Display(Name ="صورة")]
        [Required(ErrorMessage ="مطلوب")]
        public string   ImageUrl { get; set; }
        public Boolean status { get; set; }
        
    }
}
