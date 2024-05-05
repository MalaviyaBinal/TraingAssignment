using Assignment_Repository.Interface;
using Assignment_Service.Interface;
using Assignment_WebEntity.Data;
using Assignment_WebEntity.ViewModal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Service.Implementation
{
    public class LMS_Service : ILMS_Service
    {
        private readonly ILMS_Repository _repository;
        public LMS_Service(ILMS_Repository repo)
        {
            _repository = repo;
        }

        public void AddBookForm(BookFormViewModel model)
        {

            Book borrowerBook = _repository.getBorrowerByName(model.borroweName);
            int borrowID = 0;
            if (borrowerBook == null)
            {
                Borrower borrower = new Borrower
                {
                    City = model.city,
                };
                _repository.addBorrowerTable(borrower);
                borrowID = borrower.Id;
            }
            else
            {
                borrowID = borrowerBook.Borrowerid;
            }
            Book book = new Book
            {
                Borrowerid = borrowID,
                Bookname = model.bookName,
                Author = model.authorName,
                Borrowername = model.borroweName,
                DateOfIssue = DateTime.Now,
                City = model.city,
                Genere = model.genereID,
                IsDeleted = false
            };
            _repository.addBookTable(book);

        }

        public void DeleteRecord(int id)
        {
            Book book = _repository.getBookDataByBookID(id);
            book.IsDeleted = true;
            _repository.updateBookTable(book);
        }

        public void EditBookForm(BookFormViewModel model)
        {

            Book book = _repository.getBookDataByBookID(model.bookId);
            book.Bookname = model.bookName;
            book.Author = model.authorName;
            book.DateOfIssue = DateTime.Parse(model.dateOfIssue.ToString("yyyy-MM-dd"));
            book.City = model.city;
            book.Genere = model.genereID;
            book.Borrowername = model.borroweName;
            Book borrowerBook = _repository.getBorrowerByName(model.borroweName);
            int borrowID = 0;
            if (borrowerBook == null)
            {
                Borrower borrower = new Borrower
                {
                    City = model.city,
                };
                _repository.addBorrowerTable(borrower);
                borrowID = borrower.Id;
            }
            else
            {
                borrowID = borrowerBook.Borrowerid;
            }
            book.Borrowerid = borrowID;
            _repository.updateBookTable(book);
        }

        public Book GetBookDataByBookId(int id)
        {
            return _repository.getBookDataByBookID(id);
        }

        public BookFormViewModel getGenereList(int bookId)
        {
            if (bookId == 0)
            {
                return new BookFormViewModel
                {
                    genereList = _repository.getGenereList()
                };
            }
            else
            {
                Book book = _repository.getBookDataByBookID(bookId);

                return new BookFormViewModel
                {
                    genereList = _repository.getGenereList(),
                    bookName = book.Bookname,
                    authorName = book.Author,
                    borroweName = book.Borrowername,
                    dateOfIssue = DateOnly.Parse(DateOnly.FromDateTime((DateTime)book.DateOfIssue).ToString("yyyy-MM-dd")),
                    genereID = book.Genere,
                    city = book.City,
                    bookId = bookId
                };
            }


        }

        public TableDataViewModel getLibraryRecordTableData(string bookname, int pagesize, int pagenumber)
        {
            TableDataViewModel model = new TableDataViewModel();
            var datalist = _repository.getLibraryRecordTableData();
            if (bookname != null)
            {

                datalist = datalist.Where(x => x.Bookname.ToLower().Contains(bookname.ToLower()));
              
            }
            var count = datalist.Count();
            model.TotalRecord = count;
            if (count > 0)
            {
                datalist = datalist.Skip((pagenumber - 1) * pagesize).Take(pagesize);
                model.bookList = datalist.ToList();
                model.FromRec = (pagenumber - 1) * pagesize;
                model.ToRec = model.FromRec + pagesize;
                if (model.ToRec > model.TotalRecord)
                {
                    model.ToRec = model.TotalRecord;
                }
                model.FromRec += 1;
                model.TotalPages = (int)Math.Ceiling((double)count / pagesize);
                model.CurrentPage = pagenumber;
                model.PreviousPage = pagenumber > 1;
                model.NextPage = pagenumber < model.TotalPages;
                model.pagesize = pagesize;  
            }
            return model;
        }
       
    }
}
