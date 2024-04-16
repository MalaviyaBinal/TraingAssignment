using HalloDocWebEntity.ViewModel;
using HalloDocWebServices.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using HalloDocWebEntity.Data;
using System.Security.Claims;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2010.Excel;

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
            AdminDashBoardPagination model = new AdminDashBoardPagination();
            model.adminCount = count;
            model.regions = _service.getRegionList();
            return View(model);
        }

        public IActionResult AdminViewDocument(int id)
        {
            if (id != 0)
                HttpContext.Session.SetInt32("req_id", (int)id);
            var data = _service.getPatientDocument(HttpContext.Session.GetInt32("req_id"));
            var requestIdCounts = _service.GetExtension((int)HttpContext.Session.GetInt32("req_id"));

            ViewBag.fileExtensions = requestIdCounts;
            return View(data);
        }
        public IActionResult EncounterForm(int id)
        {
            return View(_service.EncounterAdmin(id));
        }

        #region Provider
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

        [HttpPost]
        public IActionResult Provider(int regid)
        {

            AdminProviderModel model = _service.getProviderDataForAdmin(regid);
            return View( model);
        }
        #endregion

        //[CustomAuthorize("Admin")]
        public IActionResult AdminDashboardMyProfile(int id)
        {
            if (id == 0)
                return View(_service.getAdminProfileData(HttpContext.Request.Cookies["userEmail"]));
            else return View(_service.getAdminData(id));
        }
        public IActionResult UserAccess()
        {
            return View();
        }
        public IActionResult GetUserAccessTable(int roleid)
        {
            return PartialView("_UserAccessTable", _service.getUserAccessData(roleid));
        }

        public IActionResult AdminDashboardPartners()
        {
            return View(_service.getVenderDetail(0,1,null));
            //return View(_service.getHealthProfessionalList());
        }

        public IActionResult ShowDeleteModal(int id, string acntType)
        {

            return PartialView("_DeleteAccountModal", _service.openDeleteModal(id, acntType));
        }



        public IActionResult DeleteVender(int id)
        {
            _service.deleteVender(id);
            return RedirectToAction(nameof(AdminDashboardPartners));
        }
        public IActionResult UnblockRequest(int id)
        {
            _service.unblockRequest(id, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(BlockHistory));
        }
        public IActionResult DeleteAccessRole(int id)
        {
            _service.deleteAccessRole(id);
            return RedirectToAction(nameof(AdminDashboardAccess));
        }
        public IActionResult DeletePhysician(int id)
        {
            _service.deletePhysicianAccount(id);
            return RedirectToAction(nameof(AdminDashboardProviders));
        }

        public IActionResult AdminDashboardAccess()
        {

            return View(_service.getRoleList());
        }
        public IActionResult AdminDashboardProviderLocation()
        {
            return View(_service.getPhysicianLocation());
        }

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
        public async Task<IActionResult> ContactProviderSendMessage(string email,string phone, string note,int selected)
        {
            _service.ContactProviderSendMessage(email,phone,note,selected);
            return RedirectToAction(nameof(AdminDashboardProviders));
        }
        public IActionResult AdminDashboardProviders()
        {
            AdminProviderModel model = _service.getProviderDataForAdmin(0);
            return View(model);
        }
        public IActionResult AdminDashboardRecords(AdminRecordsModel model)
        {
            model.ReqType = _service.GetRequestTypes();

            return View(model);
        }


        [HttpPost]
        public IActionResult _SearchRecordsTable(AdminRecordsModel model, int status, string mobile, string email, string pname, DateTime tdate, DateTime fdate, int reqtype, string searchstr , int pagenumber)
        {
            //_service.SendSms("+918320056504", "sample message");
            model = _service.getSearchRecordData(model);
            if (!string.IsNullOrWhiteSpace(searchstr))
            {
                model.Data = model.Data.Where(x => x.Firstname.ToLower().Contains(searchstr.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                model.Data = model.Data.Where(x => x.Phonenumber.ToLower().Contains(mobile.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                model.Data = model.Data.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList();
            }
            //if (!string.IsNullOrWhiteSpace(pname))
            //{
            //    model.Data = model.Data.Where(x => x.FirstName.ToLower().Contains(searchstr.ToLower())).ToList();
            //}
            if (status != 0)
            {

                model.Data = model.Data.Where(x => x.Request.Status == status).ToList();

            }
            if (reqtype != 0)
            {

                model.Data = model.Data.Where(x => x.Request.Requesttypeid == reqtype).ToList();

            }
            if (fdate != DateTime.MinValue)
            {

                model.Data = model.Data.Where(x => x.Request.Createddate > fdate).OrderBy(x => x.Request.Createddate).ToList();

            }

            if (tdate != DateTime.MinValue)
            {
                model.Data = model.Data.Where(x => x.Request.Createddate < tdate).OrderBy(x => x.Request.Createddate).ToList();
            }

            if (tdate != DateTime.MinValue && tdate != DateTime.MinValue)
            {
                model.Data = model.Data.Where(x => x.Request.Createddate > fdate && x.Request.Createddate < tdate).OrderBy(x => x.Request.Createddate).ToList();
            }

            
            var count = model.Data.Count();
            if (count > 0)
            {
                model.Data = model.Data.Skip((pagenumber - 1) * 3).Take(3).ToList();
              
                model.TotalPages = (int)Math.Ceiling((double)count / 3);
                model.CurrentPage = pagenumber;
                model.PreviousPage = pagenumber > 1;
                model.NextPage = pagenumber < model.TotalPages;
            }

            return PartialView(model);
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
            ViewBag.reqType = _service.getRequestTypeByRequestID(id);
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
            _service.sendAgreementMail(id, HttpContext.Request.Cookies["userEmail"]);
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


        public IActionResult CreateRole(int check)
        {

            return View(_service.GetMenuData(check));
        }

        public IActionResult GenerateRole(string RoleName, string[] selectedRoles, int check)
        {
            _service.generateRole(RoleName, selectedRoles, check, HttpContext.Request.Cookies["UserEmail"]);
            return RedirectToAction(nameof(AdminDashboardAccess));
        }

        public ActionResult Export(int id, int check, string searchValue, int searchRegion)
        {
            try
            {

                List<AdminDashboardTableModel> tabledata1;

                tabledata1 = _service.getDashboardTables(id, check);

                if (searchValue != null)
                {
                    if (!string.IsNullOrWhiteSpace(searchValue))
                    {
                        tabledata1 = tabledata1.Where(x => x.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                    }
                }
                if (searchRegion != 0)
                {
                    tabledata1 = tabledata1.Where(x => x.RegionID == searchRegion).ToList();
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
            List<AdminDashboardTableModel> tabledata1 = _service.getDashboardTables(id, check);
            AdminDashBoardPagination model = new AdminDashBoardPagination();
            model.physicians = _service.getPhysicianList();
            model.regions = _service.getRegionList();


            if (searchValue != null)
            {
                if (!string.IsNullOrWhiteSpace(searchValue))
                {
                    tabledata1 = tabledata1.Where(x => x.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                }

            }
            if (searchRegion != 0)
            {

                tabledata1 = tabledata1.Where(x => x.RegionID == searchRegion).ToList();

            }
            model.tabledata = tabledata1;
            var count = tabledata1.Count();
            if (count > 0)
            {
                tabledata1 = tabledata1.Skip((pagenumber - 1) * 3).Take(3).ToList();
                model.tabledata = tabledata1;
                model.TotalPages = (int)Math.Ceiling((double)count / 3);
                model.CurrentPage = pagenumber;
                model.PreviousPage = pagenumber > 1;
                model.NextPage = pagenumber < model.TotalPages;
            }
            switch (id)
            {

                case 2: return PartialView("_Pending", model);
                case 3: return PartialView("_Active", model);
                case 4: return PartialView("_Conclude", model);
                case 5: return PartialView("_Toclose", model);
                case 6: return PartialView("_Unpaid", model);
                default: return PartialView("_New", model);
            }

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
            _service.SendEmail(id, filenames, HttpContext.Request.Cookies["userEmail"]);
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
            return RedirectToAction(nameof(AdminDashboardMyProfile));
        }
        public ActionResult UpdateAdminProfile(AdminProfile info)
        {
            _service.updateadminform(info);
            return RedirectToAction(nameof(AdminDashboardMyProfile));
        }

        public IActionResult EditProviderDetail(int id,string username)
        {

            return View(_service.getProviderByAdmin(id,username));
        }
        public ActionResult SendLink(AdminDashBoardPagination  info)
        {
            _service.sendLinkAdminDashboard(info, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(AdminDashboard));
        }
        public ActionResult _SendLinkModal()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SavePhysicianPassword(AdminProviderModel info)
        {
            _service.savePhysicianPassword(info);
            return RedirectToAction(nameof(EditProviderDetail), new { id = info.physician.Physicianid });
        }
        [HttpPost]
        public ActionResult SavePhysicianInfo(AdminProviderModel info)
        {
            _service.savePhysicianInfo(info);
            return RedirectToAction(nameof(EditProviderDetail), new { id = info.physician.Physicianid });
        }
        [HttpPost]
        public ActionResult SavePhysicianBillingInfo(AdminProviderModel info)
        {
            _service.savePhysicianBillingInfo(info);
            return RedirectToAction(nameof(EditProviderDetail), new { id = info.physician.Physicianid });
        }
        [HttpPost]
        public ActionResult SavePhysicianProfile(AdminProviderModel info)
        {
            _service.savePhysicianProfile(info);
            return RedirectToAction(nameof(EditProviderDetail), new { id = info.physician.Physicianid });
        }
        [HttpPost]
        public ActionResult SavePhysicianDocuments(AdminProviderModel info)
        {
            _service.savePhysicianDocuments(info);
            return RedirectToAction(nameof(EditProviderDetail), new { id = info.physician.Physicianid });
        }
        public IActionResult UpdateRole(RoleModel roleModel)
        {
            _service.updateroleof(roleModel);
            return RedirectToAction(nameof(AdminDashboard));
        }

        public ActionResult EditRole(int id)
        {
            return View(_service.EditRole(id));
        }

        [HttpPost]
        public IActionResult CreateAdminAccount(AdminProfile model)
        {
            _service.CreateAdminAccount(model, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(AdminDashboard));
        }
        public IActionResult CreateAdminAccount()
        {
            return View(_service.getAdminRoleData());
        }

        public IActionResult EditBusiness(int id)
        {
            return View(_service.GetEditBusinessData(id));
        }
        [HttpPost]
        public IActionResult EditBusiness(SendOrderModel model)
        {
            _service.UpdateBusinessData(model);
            return RedirectToAction(nameof(AdminDashboardPartners));
        }
        public IActionResult ProviderSchedulingDayWise(int reg)
        {
            return View(_service.getSchedulingData(reg));
        }
        public IActionResult ProviderSchedulingWeekWise(int reg)
        {
            return View(_service.getSchedulingData(reg));
        }
        public IActionResult ProviderSchedulingMonthWise(int reg)
        {
            return View(_service.getSchedulingData(reg));
        }
        public IActionResult EmailLogs()
        {
            Email_SMS_LogModel model = new Email_SMS_LogModel
            {
                CurrentPage = 1
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult EmailLogTable(int roleid, string name, string email, string createdDate, string sentDate ,int pagenumber)
        {
            Email_SMS_LogModel emaillogs = _service.GetEmailLogs(roleid, name, email, createdDate, sentDate,pagenumber);
            return PartialView("_EmailLogTable", emaillogs);
        }
        public IActionResult SMSLogs()
        {
            Email_SMS_LogModel model = new Email_SMS_LogModel
            {
                CurrentPage = 1
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult SmsLogTable(int roleid, string name, string mobile, string createdDate, string sentDate, int pagenumber)
        {
           var smslogs = _service.GetSmsLogs(roleid, name, mobile, createdDate, sentDate, pagenumber);
            //List<ProviderMenuModel> providers = _service.GetProviders(regionid, order);
            return PartialView("_SMSLogTable", smslogs);
        }
        public async Task<IActionResult> OpenAddShiftModal(int regionid)
        {
            return PartialView("_AddShiftModal", _service.openShiftModel(regionid));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateShift(SchedulingViewModel info)
        {
            _service.CreateShift(info);
            return RedirectToAction(nameof(ProviderSchedulingDayWise));
        }
        public IActionResult BlockHistory()
        {
            AdminRecordsModel model = new AdminRecordsModel
            {
                CurrentPage = 1
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult _BlockHistoryTable(string searchstr, DateTime date, string email, string mobile, int pagenumber)
        {

            return View(_service.getBlockHistoryData(searchstr, date, email, mobile,pagenumber));
        }
        public IActionResult DeleteRequest(int id)
        {
            _service.deleteRequest(id);
            return RedirectToAction(nameof(AdminDashboardRecords));
        }
        public IActionResult PatientHistory()
        {
            PatientHistoryTable model = new PatientHistoryTable
            {
                CurrentPage = 1
            };
            return View(model);
        }
        public IActionResult PatientHistoryTable(string? fname, string? lname, string? email, string? phone, int pagenumber)
        {
            var model = _service.PatientHistoryTable(fname, lname, email, phone, pagenumber);
     
            return PartialView("_PatientHistoryTable", model);
        }
        public IActionResult PatientRecord(int id)
        {
            ViewBag.rid = id;
            PatientRecordModel model = new PatientRecordModel
            {
                CurrentPage = 1
            };
            return View(model);
            
        }
        public IActionResult _PatientRecordTable(int id, int pagenumber)
        {
            ViewBag.rid = id;
            return View( _service.PatientRecord(id, pagenumber));
        }
        public IActionResult _ViewShiftModal(int id, int regid)
        {
         
            return View(_service.getViewShiftData(id, regid));
        }
        public IActionResult UpdateShiftDetailData(ShiftDetailsModel model)
        {
            _service.UpdateShiftDetailData(model, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(ProviderSchedulingDayWise));
        }
        public IActionResult DeleteShiftDetails(int id)
        {
            _service.DeleteShiftDetails(id);
            return RedirectToAction(nameof(ProviderSchedulingDayWise));
        }
        public IActionResult UpdateShiftStatus(int id)
        {
            _service.UpdateShiftDetailsStatus(id);
            return RedirectToAction(nameof(ProviderSchedulingDayWise));
        }
        public IActionResult ShiftForReview(int reg = 0)
        {

            return View(_service.getReviewShiftData(reg,false));
        } 
        public IActionResult _ShiftForReview(int reg = 0,bool isCurrentMonth = false)
        {

            return View(_service.getReviewShiftData(reg,isCurrentMonth));
        }
        public IActionResult DeleteShift(string[] selectedShifts)
        {
            _service.DeletShift(selectedShifts);
            return RedirectToAction(nameof(ProviderSchedulingDayWise));
        }
        public IActionResult ApproveShift(string[] selectedShifts)
        {
            _service.ApproveShift(selectedShifts);
            return RedirectToAction(nameof(ProviderSchedulingDayWise));
        }
        public IActionResult ProviderOnCall(int reg = 0)
        {

            return View(_service.getProviderOnCall(reg));
        }
        public IActionResult SaveNotificationStatus(string[] phyList)
        {
            _service.SaveNotificationStatus(phyList);
            return RedirectToAction(nameof(AdminDashboardProviders));
        }
        public IActionResult DTYSupportRequest(AdminDashBoardPagination model)
        {
            _service.DTYSupportRequest(model.Notes, HttpContext.Request.Cookies[""]);
            return RedirectToAction(nameof(AdminDashboardProviders));
        } 
        public IActionResult AddVendor()
        {
            return View(_service.getVenderData());
        }
        public IActionResult _VendorList(int reg,int pagenumber,string searchstr)
        {
            return View(_service.getVenderDetail(reg,pagenumber, searchstr));
        }
        [HttpPost]
        public IActionResult AddVendor(SendOrderModel model)
        {
            _service.AddVendor(model);
            return RedirectToAction(nameof(AdminDashboardPartners));
        }
    }
}