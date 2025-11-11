using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Reader
    {
        [Key]
        public int ReaderId { get; set; }

        public string? FullName { get; set; }  // Đã thêm '?'
        public string? StudentCode { get; set; } // Đã thêm '?'
        public string? Phone { get; set; }     // Đã thêm '?'
        public string? Address { get; set; }   // Đã thêm '?'
    }
}