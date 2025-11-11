using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        public string? Title { get; set; }    // Đã thêm '?'
        public string? Author { get; set; }   // Đã thêm '?'
        public string? Category { get; set; } // Đã thêm '?'
        public int Quantity { get; set; }
    }
}