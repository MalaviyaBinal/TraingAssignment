using Assignment_WebEntity.Data;
using Assignment_WebEntity.ViewModal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Service.Interface
{
    public interface ILMS_Service
    {
        void AddBookForm(BookFormViewModel model);
        void DeleteRecord(int id);
        void EditBookForm(BookFormViewModel model);
        Book GetBookDataByBookId(int id);
        BookFormViewModel getGenereList(int modalType);
        TableDataViewModel getLibraryRecordTableData(string bookname, int pagesize, int pagenumber);
    }
}
