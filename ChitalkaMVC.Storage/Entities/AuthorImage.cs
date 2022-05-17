using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace ChitalkaMVC.Storage.Entities
{
    public class AuthorImage
    {
        [Key]
        public int Id { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        [DisplayName("Upload Image")]
        public IFormFile ImageFile { get; set; }

        public Author Author { get; set; }
    }
}
