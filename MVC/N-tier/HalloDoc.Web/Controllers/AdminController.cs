using HalloDocWebEntity.ViewModel;
using HalloDocWebServices.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using HalloDocWebEntity.Data;

namespace HalloDoc.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdmin_Service _service;
        public AdminController(IAdmin_Service service)
        {
            _service = service;
        }
        //[CustomAuthorize("Admin")]
        public IActionResult AdminDashboard()
        {
            var request = HttpContext.Request;
            var token = request.Cookies["Isheader"];
            var count = _service.setAdminDashboardCount();
            AdminDashboardDataWithRegionModel model = new AdminDashboardDataWithRegionModel();
            model.adminCount = count;
            model.regions = _service.getRegionList();
            if (token == "unset")
                Response.Cookies.Append("Isheader", "true", new CookieOptions { MaxAge = TimeSpan.FromDays(1) });
            return View(model);
        }

        public IActionResult AdminViewDocument(int id)
        {
            if (id != 0)
                HttpContext.Session.SetInt32("req_id", (int)id);
            var data = _service.getPatientDocument(HttpContext.Session.GetInt32("req_id"));

            return View(data);
        }
        public IActionResult EncounterForm(int id)
        {
            return View(_service.EncounterAdmin(id));
        }


        public IActionResult AdminDashboardAccess()
        {
            return View();
        }
        public IActionResult CreateProviderAdmin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProviderAdmin(AdminProviderModel model)
        {
            _service.addProviderByAdmin(model);
            return RedirectToAction(nameof(AdminDashboard));
            
        }
        public IActionResult EditProviderDetail()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Provider(int regid)
        {
            
            AdminProviderModel model = _service.getProviderDataForAdmin(regid);
            return View(model);
        }
        //[CustomAuthorize("Admin")]
        public IActionResult AdminDashboardMyProfile()
        {
            AdminProfile model = _service.getAdminProfileData(HttpContext.Request.Cookies["userEmail"]);
            return View(model);
        }
        public IActionResult AdminDashboardPartners()
        {
            return View();
        }
        public IActionResult AdminDashboardProviderLocation()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SendOrder(int id, int profId = 0, int businessId = 0)
        {
            SendOrderModel model = _service.getOrderModel(id, profId, businessId);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ContactProviderModal(int id)
        {
            Physician model = _service.getPhysicianByID(id);
            return PartialView("_ContactProviderModal", model);
        }
        [HttpPost]
        public async Task<IActionResult> ContactProviderSendMessage(Physician info)
        {
            _service.ContactProviderSendMessage(info);
            return RedirectToAction(nameof(AdminDashboard));
        }
        public IActionResult AdminDashboardProviders()
        {
            return View();
        }
        public IActionResult AdminDashboardRecords()
        {
            return View();
        }
        public IActionResult PatientRequestAdmin()
        {
            return PartialView();
        }
        public IActionResult CancelCaseModal(int id)
        {
            var user = _service.getRequestClientByID(id);
            TempData["reqid"] = id;
            return PartialView("_CancelCase", user);
        }
        public IActionResult SendAgreementModal(int id)
        {
            var user = _service.getRequestClientByID(id);
            TempData["reqid"] = id;
            return PartialView("_SendAgreement", user);
        }
        public IActionResult ClearCaseModal(int id)
        {
            var user = _service.getRequestClientByID(id);
            TempData["reqid"] = id;
            return PartialView("_ClearCase", user);
        }
        public IActionResult BlockCaseModal(int id)
        {
            var user = _service.getRequestClientByID(id);
            TempData["reqid"] = id;
            return PartialView("_BlockCase", user);
        }

        [HttpPost]
        public ActionResult CancelConfirm(int id, int reasonid, string notes)
        {
            _service.cancelConfirm(id, reasonid, notes);
            return RedirectToAction(nameof(AdminDashboard));
        }

        public ActionResult SendAgreementConfirm(int id, string username)
        {
            _service.SendAgreementConfirm(id);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        public ActionResult sendMailAgreement(int id)
        {
            _service.sendAgreementMail(id);
            return RedirectToAction(nameof(AdminDashboard));
        }
        public ActionResult SendAgreement(string token)
        {

            return View(_service.sendAgreementService(token));
        }
        [HttpPost]
        public ActionResult SendAgreement(int Requestid, string notes)
        {
            _service.SendAgreementCancleConfirm(Requestid, notes, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public ActionResult ClearConfirm(int id)
        {
            _service.clearConfirm(id);
            return RedirectToAction(nameof(AdminDashboard));
        }
        [HttpPost]
        public ActionResult BlockConfirm(int id, string notes)
        {
            _service.blockConfirm(id, notes);
            return RedirectToAction(nameof(AdminDashboard));
        }

        public async Task<IActionResult> AssignCaseModal(int id, int regid)
        {

            TempData["reqid"] = id;
            return PartialView("_AssignCaseModal", _service.AssignCaseModel(regid));
        }
        public async Task<IActionResult> TransferCaseModal(int id, int regid)
        {

            TempData["reqid"] = id;
            return PartialView("_TransferCaseModal", _service.AssignCaseModel(regid));
        }

        [HttpPost]
        public ActionResult RequestAssgin(int phyId, string notes, int id)
        {

            _service.requestAssign(phyId, notes, id, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(AdminDashboard));
        }
        public ActionResult RequestTransfer(int phyId, string notes, int id)
        {

            _service.requestTransfer(phyId, notes, id, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(AdminDashboard));
        }
        [HttpPost]
        public async Task<IActionResult> SendOrderDetail(SendOrderModel info)
        {
            _service.sendOrder(info);
            return RedirectToAction(nameof(AdminDashboard));
        }
        public async Task<IActionResult> PatientRequestbyAdmin(RequestForMe info)
        {
            _service.patientRequestByAdmin(info, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(AdminDashboard));
        }

        [HttpPost]
        public ActionResult NavAdmin(int id)
        {


            if (id == 1)
            {
                Response.Cookies.Append("Isheader", "false", new CookieOptions { MaxAge = TimeSpan.FromDays(1) });
                //HttpContext.Session.SetString("Isheader", "false");
                return RedirectToAction("AdminDashboard");
            }

            else if (id == 2) return RedirectToAction("AdminDashboardProviderLocation");
            else if (id == 3) return RedirectToAction("AdminDashboardMyProfile");
            else if (id == 4) return RedirectToAction("AdminDashboardProviders");
            else if (id == 5) return RedirectToAction("AdminDashboardPartners");
            else if (id == 6) return RedirectToAction("AdminDashboardAccess");
            else if (id == 7) return RedirectToAction("AdminDashboardRecords");
            else return RedirectToAction("AdminDashboard");
        }

        public ActionResult Export(int id, int check, string searchValue, int searchRegion)
        {
            try
            {

                IQueryable<AdminDashboardTableModel> tabledata1;

                tabledata1 = _service.getDashboardTables(id, check);

                if (searchValue != null)
                {
                    if (!string.IsNullOrWhiteSpace(searchValue))
                    {
                        tabledata1 = tabledata1.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));
                    }
                }
                if (searchRegion != 0)
                {
                    tabledata1 = tabledata1.Where(x => x.RegionID == searchRegion);
                }


                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Data");


                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Date Of Birth";
                worksheet.Cell(1, 3).Value = "Requestor";
                worksheet.Cell(1, 4).Value = "Physician Name";
                worksheet.Cell(1, 5).Value = "Date of Service";
                worksheet.Cell(1, 6).Value = "Requested Date";
                worksheet.Cell(1, 7).Value = "Phone Number";
                worksheet.Cell(1, 8).Value = "Address";
                worksheet.Cell(1, 9).Value = "Notes";

                int row = 2;
                foreach (var item in tabledata1)
                {
                    var statusClass = "";
                    var dos = "";
                    var notes = "";
                    if (item.RequestTypeId == 1)
                    {
                        statusClass = "patient";
                    }
                    else if (item.RequestTypeId == 4)
                    {
                        statusClass = "business";
                    }
                    else if (item.RequestTypeId == 2)
                    {
                        statusClass = "family";
                    }
                    else
                    {
                        statusClass = "concierge";
                    }
                    //foreach (var stat in item.Requeststatuslogs)
                    //{
                    //    if (stat.Status == 2)
                    //    {
                    //        dos = stat.Createddate.ToString("MMMM dd,yyyy");
                    //        notes = stat.Notes ?? "";
                    //    }
                    //}
                    worksheet.Cell(row, 1).Value = item.Name;
                    worksheet.Cell(row, 2).Value = item.DOB;
                    worksheet.Cell(row, 3).Value = item.Requestor;
                    worksheet.Cell(row, 4).Value = item.physician;
                    worksheet.Cell(row, 5).Value = item.Dateofservice;
                    worksheet.Cell(row, 6).Value = item.Requesteddate;
                    worksheet.Cell(row, 7).Value = item.Phonenumber;
                    worksheet.Cell(row, 8).Value = item.Address;
                    worksheet.Cell(row, 9).Value = item.Notes;
                    row++;
                }
                worksheet.Columns().AdjustToContents();

                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        [HttpPost]
        public ActionResult DashboardTables(int id, int check, string searchValue, int searchRegion, int pagesize, int pagenumber = 1)
        {
            IQueryable<AdminDashboardTableModel> tabledata1 = _service.getDashboardTables(id, check);
            AdminDashboardDataWithRegionModel model = new AdminDashboardDataWithRegionModel();
            model.physicians = _service.getPhysicianList();
            model.regions = _service.getRegionList();


            if (searchValue != null)
            {
                if (!string.IsNullOrWhiteSpace(searchValue))
                {
                    tabledata1 = tabledata1.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));
                }

            }
            if (searchRegion != 0)
            {

                tabledata1 = tabledata1.Where(x => x.RegionID == searchRegion);

            }
            model.tabledata = tabledata1;
            var count = tabledata1.Count();
            if (count > 0)
            {
                tabledata1 = tabledata1.Skip((pagenumber - 1) * 3).Take(3);
                model.tabledata = tabledata1;
                model.TotalPages = (int)Math.Ceiling((double)count / 3);
                model.CurrentPage = pagenumber;
                model.PreviousPage = pagenumber > 1;
                model.NextPage = pagenumber < model.TotalPages;
            }

            if (id == 1)
                return PartialView("_New", model);
            else if (id == 2)
                return View("_Pending", model);
            else if (id == 3) return View("_Active", model);
            else if (id == 4) return View("_Conclude", model);
            else if (id == 5) return View("_Toclose", model);
            else if (id == 6) return View("_Unpaid", model);
            else return View("_New", model);
        }
      
        public async Task<ActionResult> ViewCase(int id)
        {

            return View(_service.ViewCaseModel(id));

        }

        [HttpPost]

        public async Task<IActionResult> SaveNote(Notes n, int id)
        {
            _service.saveNotes(n, id);
            return RedirectToAction(nameof(AdminDashboard));
        }

        [Route("/Home")]
        public async Task<ActionResult> ViewNotes(int id)
        {

            return View(_service.ViewNotes(id));
        }

        [HttpPost]
        public ActionResult DownloadFiles([FromBody] string[] filenames)
        {
            var zipStream = _service.downloadFile(filenames);
            return File(zipStream.ToArray(), "application/zip", "selected_files.zip");
        }
        public async Task<ActionResult> DeleteAll(int id, string[] filenames)
        {
            _service.deleteAllFile(id, filenames);
            return RedirectToAction(nameof(AdminViewDocument), (int)HttpContext.Session.GetInt32("req_id"));
        }
        public async Task<ActionResult> SendMail(int id, string[] filenames)
        {
            _service.SendEmail(id, filenames);
            return RedirectToAction(nameof(AdminViewDocument), (int)HttpContext.Session.GetInt32("req_id"));
        }
        public async Task<ActionResult> DeleteFile(int id)
        {
            _service.deleteFile(id);
            return RedirectToAction(nameof(AdminViewDocument), (int)HttpContext.Session.GetInt32("req_id"));
        }
        [HttpPost]
        public IActionResult UploadFileAdmin(IFormFile fileToUpload)
        {
            _service.uploadFileAdmin(fileToUpload, (int)HttpContext.Session.GetInt32("req_id"), HttpContext.Request.Cookies["userEmail"]);

            if (fileToUpload != null && fileToUpload.Length > 0)
            {
                return RedirectToAction(nameof(AdminViewDocument), (int)HttpContext.Session.GetInt32("req_id"));
            }
            else
            {
                // User did not select a file
                return RedirectToAction(nameof(AdminViewDocument));
            }

        }

        public async Task<IActionResult> DownloadFile(int id)
        {
            var file = _service.getRequestWiseFileByID(id);
            //var filepath = "D:\\Projects\\HelloDOC\\MVC\\Hallodoc - Copy\\wwwroot\\UploadedFiles\\" + Path.GetFileName(file.Filename);
            var bytes = _service.getBytesForFile(id);
            return File(bytes, "application/octet-stream", file.Filename);
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("userName");
            Response.Cookies.Delete("juserEmailwt");
            Response.Cookies.Delete("Isheader");
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.PatientLogin), "Home");
        }
        public ActionResult SaveEncounterForm(Encounterformmodel info)
        {
            _service.saveEncounterForm(info);
            return RedirectToAction(nameof(AdminDashboard));
        }
        public IActionResult ExportAll()
        {
            byte[] excelBytes = _service.GetBytesForExportAll();
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportedData.xlsx");
        }
        public IActionResult ExportStateWise(int id)
        {
            byte[] excelBytes = _service.GetBytesForExport(id);
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportedData.xlsx");
        }

        public IActionResult CloseCase(int id)
        {
            ViewBag.id = id;
            AdminViewUpload model = _service.closeCase(id);
            return View(model);
        }
        public IActionResult SaveCloseCase(int id, AdminViewUpload n)
        {
            _service.closeCaseSaveData(id, n);
            return RedirectToAction(nameof(AdminDashboard));
        }

        public ActionResult UpdateAddressInformationOfAdmin(AdminProfile info)
        {
            _service.updateadminaddress(info);
            return RedirectToAction(nameof(AdminDashboard));
        }
        public ActionResult UpdateAdminProfile(AdminProfile info)
        {
            _service.updateadminform(info);
            return RedirectToAction(nameof(AdminDashboard));
        }
        public ActionResult SendLink(AdminDashboardDataWithRegionModel info)
        {
            _service.sendLinkAdminDashboard(info);
            return RedirectToAction(nameof(AdminDashboard));
        }
    }
}