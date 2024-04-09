﻿using HalloDoc.Web.Models;
using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using HalloDocWebServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HalloDocWebService.Authentication;
using HalloDocWebService.Utils;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using HalloDocWebServices.Implementation;
using System.IdentityModel.Tokens.Jwt;
namespace HalloDoc.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPatient_Service _service;
        private readonly IJWT_Service _jwtservice;
        public HomeController(IPatient_Service service, IJWT_Service jwtservice)
        {
            _service = service;
            _jwtservice = jwtservice;
        }
        public async Task< IActionResult> Index()
        {
            return View();
        }
        public IActionResult Profile()
        {
            var profile = _service.ReturnRequest(HttpContext.Request.Cookies["userEmail"]);
           
            return View(profile);
        }
        public IActionResult SubmitPatientRequest()
        {
            return View();
        }
        public IActionResult RequestBusiness()
        {
            return View();
        }
        public IActionResult RequestConcierge()
        {
            return View();
        }
        public IActionResult RequestFrdFamily()
        {
            return View();
        }
        public IActionResult RequestPatient()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientForgotPwd(ForgotPwdModel info)
        {
            _service.sendMail(info);
            return View();
        }
        [CustomAuthorize("patient") ]
        public async Task<IActionResult> PatientDashboard()
        {
            var request = HttpContext.Request;
            var jwt = request.Cookies["jwt"];
            _jwtservice.ValidateToken(jwt, out JwtSecurityToken jwtToken);
           Claim claim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userEmail");
            if (claim.Value != null)
            {
                var profile = _service.ReturnRequest(claim.Value);
                var requestIdCounts = _service.GetCount(claim.Value);
                ViewBag.RequestIdCounts = requestIdCounts;
                return View(profile);
            }
            return Problem("Entityset Aspnetuser is NULL");
        }
        [CustomAuthorize("LoginStr") ]
        public IActionResult PatientLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult  PatientLogin(loginModel user)
        {
            //var u = new AuthManager().Login(user.Usarname, user.Passwordhash);
                bool isRegistered = _service.ValidateUser(user.Usarname, user.Passwordhash);
                if (isRegistered) {
                    //SessionUtils.SetLoggedUsers(HttpContext.Session, u);
                    var userobj = _service.getAspnetuserByEmail(user.Usarname);
                    var jwttoken = _jwtservice.GenerateToken(userobj);
                    //Response.Cookies.Append("jwt", jwttoken);
                    Response.Cookies.Append("jwt", jwttoken ,new CookieOptions { MaxAge = TimeSpan.FromDays(1) });
                    Response.Cookies.Append("userName", userobj.Usarname, new CookieOptions { MaxAge = TimeSpan.FromDays(1) });
                    Response.Cookies.Append("userEmail", userobj.Email, new CookieOptions { MaxAge = TimeSpan.FromDays(1) });
                    return RedirectToAction(nameof(AdminController.AdminDashboard),"Admin");
                }
            return View();
        }
        public IActionResult PatientViewDocument(int? Id = 0)
        {
            HttpContext.Session.SetInt32("req_id", (int)Id);
            var list = _service.getPatientDocument(Id);
            return View(list);
        }
        public IActionResult CreateAccount(string token)
        {
            loginModel model = _service.createAccountService(token);
            if(model.Usarname == null)
                return Problem("Invalid request");
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateAccount(loginModel info)
        {
            _service.createAccountSaveData(info);
            return RedirectToAction(nameof(PatientLogin));
        }
        [HttpPost]
        public IActionResult UploadFile(IFormFile fileToUpload)
        {
            if (fileToUpload != null && fileToUpload.Length > 0)
            {
                _service.uploadFile(fileToUpload, (int)HttpContext.Session.GetInt32("req_id"));
              
            }
           
                return RedirectToAction(nameof(PatientViewDocument), new {Id = (int)HttpContext.Session.GetInt32("req_id") });
           
        }
        [Route("/Home/RequestPatient/{email}")]
        [Route("/Home/RequestBusiness/{email}")]
        [Route("/Home/RequestFrdFamily/{email}")]
        [Route("/Home/RequestConcierge/{email}")]
        [HttpGet]
        public IActionResult CheckEmailExists(string email)
        {
            var emailExists = _service.getAspnetUserAny(email);
            if(emailExists == false)
            {
                _service.sendMailForCreateAccount(email);
            }
            return Json(new { exists = emailExists });
        }
        public async Task<IActionResult> CreatePatientByBusiness(BusinessPatientRequest info)
        {
            _service.createPatientByBusiness(info);            
            return RedirectToAction(nameof(SubmitPatientRequest));
        }
        public async Task<IActionResult> CreatePatientByConierge(ConciergePatientRequest info)
        {
            _service.createPatientByConcierge(info);
            return RedirectToAction("SubmitPatientRequest", "Home");
        }
        public async Task<IActionResult> CreatePatientByFamilyFrd(FamilyFrdPatientRequest info)
        {
            _service.createPatientByFamilyFrd(info);
            return RedirectToAction("SubmitPatientRequest", "Home");
        }
        public async Task<IActionResult> CreatePatient(PatientRequest info)
        {
            _service.createPatient(info);
            return RedirectToAction("SubmitPatientRequest", "Home");
        }
        public IActionResult PatientDashboardRequestForSomeone()
        {
            return View();
        }
        public async Task<IActionResult> PatientDashboardRequestForMe()
        {
            var user = _service.getUserByEmail(HttpContext.Request.Cookies["userEmail"]);
            //var user = await _context.Users.FirstOrDefaultAsync(m => m.Email == HttpContext.Request.Cookies["userEmail"]);
            return View(user);
        }
        public async Task<IActionResult> RequestForsSomeone(RequestForMe info)
        {
            _service.saveDataRequestForMe(info, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(PatientDashboard));
        }
        public async Task<IActionResult> SubmitRequestOnMe(RequestForMe info)
        {
            _service.saveDataForSomeone(info, HttpContext.Request.Cookies["userEmail"]);
            return RedirectToAction(nameof(PatientDashboard));
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(PatientRequest model)
        {
            _service.editProfile(model, HttpContext.Request.Cookies["userEmail"]);
            Response.Cookies.Append("userName", model.first_name, new CookieOptions { MaxAge = TimeSpan.FromDays(1) });
            return RedirectToAction(nameof(PatientDashboard));
        }
        public async Task<IActionResult> DownloadFile(int id)
        {
            var file = _service.getRequestWiseFile(id);
            var bytes = _service.getBytesForFileDownload(id);
            return File(bytes, "application/octet-stream", file.Filename);
        }
        [HttpPost]
        public IActionResult DownloadAll([FromBody] string[] filenames)
        {
            var ms = _service.downloadAlll(filenames);
            return File(ms.ToArray(), "application/zip", "download.zip");
        }
    }
}