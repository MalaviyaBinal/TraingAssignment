using Assignment_WebEntity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_WebEntity.ViewModal
{
    public class TableDataViewModel
    {
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int pagesize { get; set; }
        public bool PreviousPage { get; set; }
        public bool NextPage { get; set; }
        public List<Book> bookList { get; set; }
        public int TotalRecord { get; set; }
        public int FromRec { get; set; }
        public int ToRec { get; set; }
    }
}
