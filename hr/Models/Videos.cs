using System.ComponentModel.DataAnnotations;

namespace hr.Models
{
    public class Videos
    {
        [Key]
        public int Id { get; set; }
        public string VideoLink { get; set; }
        public string VideoTitle { get; set; }
        public string VideoPosition { get; set; }
        public Boolean status { get; set; }
    }
}
