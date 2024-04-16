using HalloDocWebEntity.ViewModel;
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
            return RedirectToAction(nameof(AdminDashboard));
        }
        public async Task<IActionResult> TransferCaseModal(int id)
        {

            TempData["reqid"] = id;
            return PartialView("_TransferCaseModal", _service.GetRequestData(id));
        }


        #endregion
    }
}