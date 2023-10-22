using System.ComponentModel.DataAnnotations;

namespace hr.Models
{
    public class Contacts
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public DateTime CreateDate { get; set; }
     
    }
}
