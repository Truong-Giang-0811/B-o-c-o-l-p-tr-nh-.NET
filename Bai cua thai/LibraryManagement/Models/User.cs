using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string? Username { get; set; }     // Đã thêm '?'
        public string? PasswordHash { get; set; } // Đã thêm '?'
        public string? Role { get; set; }         // Đã thêm '?'
    }
}