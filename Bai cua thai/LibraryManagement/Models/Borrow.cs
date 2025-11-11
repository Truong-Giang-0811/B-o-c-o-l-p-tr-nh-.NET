using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Borrow
    {
        [Key]
        public int BorrowId { get; set; }

        [ForeignKey("Reader")]
        public int ReaderId { get; set; }
        public Reader? Reader { get; set; } // Đã thêm '?'

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book? Book { get; set; }     // Đã thêm '?'

        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; } 
    }
}