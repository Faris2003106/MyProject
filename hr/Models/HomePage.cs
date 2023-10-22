using System.ComponentModel.DataAnnotations;

namespace hr.Models
{
    public class HomePage
    {
        [Key]
        public int StaticId { get; set; }
        public string StaitcTitle { get; set; }
        public string StaitcNum { get; set; }
        public Boolean status { get; set; }

    }
}
