using Microsoft.AspNetCore.Mvc;

namespace hr.Models
{
    public class Publications 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string Department { get; set; }
        public string ImageUrl { get; set; }
        public Boolean status { get; set; }
    }
}
