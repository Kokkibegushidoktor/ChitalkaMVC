using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChitalkaMVC.Models
{
    public class ListAuthorsViewModel
    {
        public int AuthorId { get; set; }
        public SelectList Authors { get; set; }
    }
}
