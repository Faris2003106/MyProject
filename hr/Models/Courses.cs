using System.ComponentModel.DataAnnotations;

namespace hr.Models
{
    public class Courses
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime? CourseDate { get; set; }
        public string? Organization { get; set; }
        public string OrganizationLogoUrl { get; set; }
        public string? RegisterLink { get; set; }
        public string? description { get; set; }
        public Boolean status { get; set; }

    }
}
