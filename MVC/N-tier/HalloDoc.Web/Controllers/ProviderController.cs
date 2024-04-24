using HalloDocWebEntity.ViewModel;

using HalloDocWebServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

using HalloDocWebEntity.Data;
using DocumentFormat.OpenXml.Office2010.Excel;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace HalloDoc.Web.Controllers
{
    public class ProviderController : Controller
    {
        private readonly IProvider_Service _service;
        public ProviderController(IProvider_Service service)
        {
            _service = service;
        }

        #region ProviderDashboard
        public IActionResult ProviderDashboard()
        {

            var count = _service.setProviderDashboardCount(HttpContext.Request.Cookies["userEmail"]);
            AdminDashBoardPagination model = new AdminDashBoardPagination();
            model.adminCount = count;
            model.regions = _service.getRegionList();
            return View(model);
        }
        [HttpPost]
        public ActionResult DashboardTables(int id, int check, string searchValue, int searchRegion, int pagesize, int pagenumber = 1)
        {
            List<AdminDashboardTableModel> tabledata1 = _service.getDashboardTables(id, check, HttpContext.Request.Cookies["userEmail"]);
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
            model.TotalRecord = count;
            if (count > 0)
            {
                tabledata1 = tabledata1.Skip((pagenumber - 1) * 3).Take(3).ToList();
                model.tabledata = tabledata1;
                model.FromRec = (pagenumber - 1) * 3;
                model.ToRec = model.FromRec + 3;
                if (model.ToRec > model.TotalRecord)
                {
                    model.ToRec = model.TotalRecord;
                }
                model.FromRec += 1;
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
                default: return PartialView("_New", model);
            }

        }
        public IActionResult PatientRequestProvider()
        {
            return PartialView();
        }
        public async Task<IActionResult> PatientRequestbyProvider(RequestForMe info)
        {
            if (!ModelState.IsValid)
            {
                return  View("PatientRequestProvider", info);
            }
            _service.patientRequestByProvider(info, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(ProviderDashboard));
        }
        public ActionResult _SendLinkModal()
        {
            return View();
        }
        public ActionResult SendLink(AdminDashBoardPagination info)
        {
           
            _service.sendLinkProviderDashboard(info, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(ProviderDashboard));
        }
        #endregion

        #region Provider Dashboard Actions
        public IActionResult GeneratePDF([FromQuery] int requestid)
        {
            var encounterFormView = _service.EncounterProvider(requestid);
            if (encounterFormView == null)
            {
                return NotFound();
            }
            return new ViewAsPdf("FinalizeForm", encounterFormView)
            {
                FileName = "Encounter_Form.pdf"
            };

        }
        public ActionResult AcceptConsultRequest(int id)
        {
            _service.AcceptConsultRequest(id, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(ProviderDashboard));
        }
        public async Task<ActionResult> ViewCase(int id)
        {

            return View(_service.ViewCaseModel(id));

        }
        public async Task<ActionResult> ViewNotes(int id)
        {

            return View(_service.ViewNotes(id));
        }
        public async Task<IActionResult> SaveNote(Notes n, int id)
        {
            _service.saveNotes(n, id);
            return RedirectToAction(nameof(ProviderDashboard));
        }
        public IActionResult SendAgreementModal(int id)
        {
            Requestclient user = _service.getRequestClientByID(id);
            TempData["reqid"] = id;
            ViewBag.reqType = _service.getRequestTypeByRequestID(id);
            return PartialView("_SendAgreement", user);
        }
        public ActionResult sendMailAgreement(int id)
        {
            _service.sendAgreementMail(id, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(ProviderDashboard));
        }
        public async Task<IActionResult> TransferCaseModal(int id)
        {

            TempData["reqid"] = id;
            return PartialView("_TransferCaseModal", _service.GetRequestData(id));
        }

        public async Task<IActionResult> TransferConfirm(Request reg)
        {
            _service.TransferConfirm(reg, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(ProviderDashboard));
        }
        public IActionResult ViewDocument(int id)
        {
            if (id != 0)
                HttpContext.Session.SetInt32("req_id", (int)id);
            AdminViewUpload data = _service.getPatientDocument(HttpContext.Session.GetInt32("req_id"));
            Dictionary<int, string> requestIdCounts = _service.GetExtension((int)HttpContext.Session.GetInt32("req_id"));

            ViewBag.fileExtensions = requestIdCounts;
            return View(data);
        }
        public async Task<ActionResult> DeleteAll(int id, string[] filenames)
        {
            _service.deleteAllFile(id, filenames);
            return RedirectToAction(nameof(ViewDocument), (int)HttpContext.Session.GetInt32("req_id"));
        }
        [HttpPost]
        public IActionResult UploadFileProvider(IFormFile fileToUpload)
        {

            if (fileToUpload != null && fileToUpload.Length > 0)
            {
                _service.uploadFileAdmin(fileToUpload, (int)HttpContext.Session.GetInt32("req_id"), HttpContext.Request.Cookies["userEmail"]);

                return RedirectToAction(nameof(ViewDocument), (int)HttpContext.Session.GetInt32("req_id"));
            }
            else
            {

                return RedirectToAction(nameof(ViewDocument));
            }

        }
        
        [HttpPost]
        public ActionResult DownloadFiles([FromBody] string[] filenames)
        {
            MemoryStream zipStream = _service.downloadFile(filenames);
            return File(zipStream.ToArray(), "application/zip", "selected_files.zip");
        }
        public async Task<IActionResult> SendOrder(int id, int profId = 0, int businessId = 0)
        {
            SendOrderModel model = _service.getOrderModel(id, profId, businessId);
            return View(model);
        }
        public async Task<IActionResult> SendOrderDetail(SendOrderModel info)
        {
            _service.sendOrder(info);
            return RedirectToAction(nameof(AdminDashboard));
        }
        public async Task<IActionResult> _CallTypeModal(int id)
        {
            //var data = _service.opencancelmodel(id);
            ViewBag.reqid = id;
            return View();
        }
        public async Task<IActionResult> _EncounterModal(int id)
        {
            //var data = _service.opencancelmodel(id);
            ViewBag.reqid = id;

            return View();
        }
        public ActionResult ConsultCall(int id)
        {
            _service.ConsultCall(id);
            return RedirectToAction(nameof(ProviderDashboard));
        }
        public ActionResult HouseCall(int id)
        {
            _service.HouseCall(id);
            return RedirectToAction(nameof(ProviderDashboard));
        }
        public ActionResult EncounterFinalize(int id)
        {
            _service.EncounterFinalize(id);

            return RedirectToAction(nameof(ProviderDashboard));
        }
        public IActionResult EncounterForm(int id)
        {
            return View(_service.EncounterProvider(id));
        }
        public ActionResult SaveEncounterForm(Encounterformmodel info)
        {
            _service.saveEncounterForm(info);
            return RedirectToAction(nameof(ProviderDashboard));
        }
        public async Task<ActionResult> DeleteFile(int id)
        {
            _service.deleteFile(id);
            return RedirectToAction(nameof(ViewDocument), (int)HttpContext.Session.GetInt32("req_id"));
        }
        public async Task<ActionResult> SendMail(int id, string[] filenames)
        {
            _service.SendEmail(id, filenames, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(ViewDocument), (int)HttpContext.Session.GetInt32("req_id"));
        }
        public ActionResult HouseCallToConclude(int id)
        {
            _service.HouseCallToConclude(id);
            return RedirectToAction(nameof(ProviderDashboard));
        }
        public IActionResult ConcludeCare(int id)
        {
            ViewBag.id = id;
            Dictionary<int, string> requestIdCounts = _service.GetExtension(id);

            ViewBag.fileExtensions = requestIdCounts;
            AdminViewUpload model = _service.ConcludeCare(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult ConcludeUploadFileProvider(IFormFile fileToUpload)
        {

            if (fileToUpload != null && fileToUpload.Length > 0)
            {
                _service.uploadFileAdmin(fileToUpload, (int)HttpContext.Session.GetInt32("req_id"), HttpContext.Request.Cookies["userEmail"]);

                return RedirectToAction(nameof(ConcludeCare), (int)HttpContext.Session.GetInt32("req_id"));
            }
            else
            {

                return RedirectToAction(nameof(ConcludeCare));
            }

        }
        public IActionResult ConcludeFinal(AdminViewUpload model)
        {
            _service.ConcludeFinal(model, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(ProviderDashboard));
        }
        #endregion
        #region My Profile
        public IActionResult MyProfile()
        {
            return View(_service.getPhyProfileData(HttpContext.Request.Cookies["userEmail"]));
        }
        public IActionResult UpdateProfileRequest(AdminProviderModel model)
        {
            _service.UpdateProfileRequest(HttpContext.Request.Cookies["userEmail"], model.Adminnotes);
            return RedirectToAction(nameof(MyProfile));
        }
        public ActionResult SavePhysicianPassword(AdminProviderModel info)
        {
            if (!ModelState.IsValid)
            {
                var pwd = info.Passwordhash;
                info = _service.getPhyProfileData(HttpContext.Request.Cookies["userEmail"]);
                info.Passwordhash = pwd;
                return View("MyProfile", info);
            }
           
            _service.savePhysicianPassword(info);
            return RedirectToAction(nameof(MyProfile));
        }
        #endregion

        #region Scheduling
       
        public IActionResult MySchedule(int reg)
        {
            return View(_service.getSchedulingData(HttpContext.Request.Cookies["userEmail"]));
        }
        public IActionResult _ViewShiftModal(int id, int regid)
        {

            return View(_service.getViewShiftData(id, regid));
        }
        public async Task<IActionResult> _AddShiftModal(int regionid)
        {
            return View( _service.openShiftModel(regionid));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateShift(SchedulingViewModel info)
        {
            _service.CreateShift(info, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(MySchedule));
        }
        public IActionResult DeleteShiftDetails(int id)
        {
            _service.DeleteShiftDetails(id);
            return RedirectToAction(nameof(MySchedule));
        }
        #endregion



    }
}