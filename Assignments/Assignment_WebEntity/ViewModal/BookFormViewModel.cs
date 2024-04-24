using Assignment_WebEntity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_WebEntity.ViewModal
{
    public class BookFormViewModel
    {
        public int bookId { get; set; }
        [Required(ErrorMessage ="Book name required...")]
        public string bookName { get; set; }
        [Required(ErrorMessage = "Author name required...")]
        public string? authorName { get; set; }
        [Required(ErrorMessage = "Borrower name required...")]
        public string? borroweName { get; set; }
        [Required(ErrorMessage = "Select Date of Isuue")]
        [DataType(DataType.DateTime)]
        public DateOnly dateOfIssue { get; set; }
        [Required(ErrorMessage = "Select Genere..")]
        public int? genereID { get; set; }
        [Required(ErrorMessage = "City required...")]
        public string city { get; set; }
        public List<Genere> genereList { get; set; }
    }
}
