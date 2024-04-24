using Assignment_WebEntity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Repository.Interface
{
    public interface ILMS_Repository
    {
        void addBookTable(Book book);
        void addBorrowerTable(Borrower borrower);
        Book getBookDataByBookID(int bookId);
        Book getBorrowerByName(string? borroweName);
        List<Genere> getGenereList();
        
        List<Book> getLibraryRecordTableData();
        void updateBookTable(Book book);
    }
}
