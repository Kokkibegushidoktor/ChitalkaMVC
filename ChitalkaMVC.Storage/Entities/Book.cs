﻿namespace ChitalkaMVC.Storage.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public bool Visibility { get; set; }
        [Required]
        public string ImagePath { get; set; }

        [Required]
        public int AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public virtual Author? Author { get; set; }
        [Required]
        public int CenturyId { get; set; }
        [ForeignKey(nameof(CenturyId))]
        public virtual Century? Century { get; set; }
        public List<Genre>? Genres { get; set; }
        public List<UserBook>? UserBooks { get; set; }
    }
}
