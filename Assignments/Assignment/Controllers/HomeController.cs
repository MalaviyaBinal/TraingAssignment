using Assignment.Models;
using Assignment_Service.Interface;
using Assignment_WebEntity.ViewModal;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILMS_Service _service;

        public HomeController(ILogger<HomeController> logger, ILMS_Service service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            TableDataViewModel model = new TableDataViewModel
            {
                CurrentPage = 1,
                pagesize=3
            };
            return View(model);
        }

        public IActionResult _BookFormModal(int bookId)
        {
            return PartialView(_service.getGenereList(bookId));
        }
        public IActionResult AddOrEditBookForm(BookFormViewModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.bookId == 0)
                    _service.AddBookForm(model);
                else
                    _service.EditBookForm(model);

            }
            
            return RedirectToAction(nameof(Index));

        }
        public IActionResult _LibraryRecordTable(string bookname,int pagesize = 3,int pagenumber=1)
        {
            return PartialView(_service.getLibraryRecordTableData(bookname,pagesize,pagenumber));
        }
        public IActionResult DeleteRecord(int id)
        {
            _service.DeleteRecord(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult _DeleteConfirmModal(int id)
        {

            return PartialView(_service.GetBookDataByBookId(id));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}