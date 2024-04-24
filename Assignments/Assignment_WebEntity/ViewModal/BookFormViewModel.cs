using Assignment_WebEntity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_WebEntity.ViewModal
{
    public class BookFormViewModel
    {
        public int bookId { get; set; }
        public string bookName { get; set; }
        public string? authorName { get; set; }
        public string? borroweName { get; set; }
        public DateOnly dateOfIssue { get; set; }
        public int? genereID { get; set; }
        public string city { get; set; }
        public List<Genere> genereList { get; set; }
    }
}
