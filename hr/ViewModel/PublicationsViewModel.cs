using hr.Models;
using System.Drawing;

namespace hr.ViewModel
{
    public class PublicationsViewModel: Publications
    {
        public IFormFile Image { get; set; } 
    }
}
