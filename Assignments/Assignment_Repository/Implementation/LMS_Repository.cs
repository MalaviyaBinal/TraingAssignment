using Assignment_Repository.Interface;
using Assignment_WebEntity.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Repository.Implementation
{
    public class LMS_Repository : ILMS_Repository
    {
        private readonly ApplicationContext _context;
        public LMS_Repository(ApplicationContext context) {
            _context = context;
        }

        public void addBookTable(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void addBorrowerTable(Borrower borrower)
        {
            _context.Borrowers.Add(borrower);
            _context.SaveChanges();
        }

        public Book getBookDataByBookID(int bookId)
        {
            return _context.Books.Include(e => e.GenereNavigation).FirstOrDefault(e => e.Id == bookId);
        }

        public Book getBorrowerByName(string? borroweName)
        {
            
            return _context.Books.FirstOrDefault(e => e.Borrowername.ToLower() == borroweName.ToLower());
        }

        public List<Genere> getGenereList()
        {
            return _context.Generes.ToList();
        }

        

        public IQueryable<Book> getLibraryRecordTableData()
        {
            return _context.Books.Include(e => e.GenereNavigation).Where(e => e.IsDeleted == false);
        }

        public void updateBookTable(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }
    }
}
