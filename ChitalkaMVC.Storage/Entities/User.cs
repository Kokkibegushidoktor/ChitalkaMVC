﻿namespace ChitalkaMVC.Storage.Entities
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string? Mail { get; set; }
        public string Password { get; set; }
        public bool isAdmin { get; set; }
        public List<Note>? Notes { get; set; }
        public List<UserBook>? UserBooks { get; set; }
    }
}
