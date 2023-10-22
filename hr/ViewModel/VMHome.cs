using hr.Models;

namespace hr.ViewModel
{
    public class VMHome
    {
        public List<events> events { get; set; }
        public List<HomePage> statics { get; set; }
        public List<Courses> courses { get; set; }
        public List<Honoring> honoring { get; set; }
        public List<Publications> publications { get; set; }
        public List<Discounts> discounts { get; set; }
        public List<News> news { get; set; }
        public List<Videos> videos { get; set; }

    }
}
