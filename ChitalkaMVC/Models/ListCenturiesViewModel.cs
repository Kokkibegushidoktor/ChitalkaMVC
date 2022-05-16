using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChitalkaMVC.Models
{
    public class ListCenturiesViewModel
    {
        public int CenturyId { get; set; }
        public SelectList Centuries { get; set; }
    }
}
