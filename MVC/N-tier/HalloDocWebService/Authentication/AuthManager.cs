using HalloDocWebEntity.Data;
using HalloDocWebRepo.Interface;
using HalloDocWebServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocWebService.Authentication
{
    public class AuthManager
    {
        private readonly IAdmin_Repository _repo;

        public AuthManager()
        {
        }

        public AuthManager(IAdmin_Repository repo)
        {
            _repo = repo;
        }

        public Aspnetuser Login(string usarname, string passwordhash)
        {
            return _repo.getAspnetuserByEmail(usarname);
        }

    }
    public class CustomAuthorize : Attribute, IAuthorizationFilter 
    {
        private readonly string _role;
        private readonly int _menu;
  

        public CustomAuthorize(string role = "", int menu = 0)
        {
            this._role = role;
            this._menu = menu;
        }
    

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtService = context.HttpContext.RequestServices.GetService<IJWT_Service>();
            var _repo = context.HttpContext.RequestServices.GetService<IAdmin_Repository>();
            Physician phy = new();
            Admin admin = new Admin();
            List<int> menus = new List<int>();
            if (jwtService == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];
            if (_role == "LoginStr")
            {
                if (token == null)
                {
                    return;
                }
                if (!jwtService.ValidateToken(token, out JwtSecurityToken jwtTokenforLogin))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                    return;
                }

                Claim roleClaim = jwtTokenforLogin.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                switch (roleClaim.Value)
                {
                    case "Patient":
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PatientDashboard" }));
                        return;
                    case "Admin":
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "AdminDashboard" }));
                        return;
                    case "Provider":
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Provider", action = "ProviderDashboard" }));
                        return;

                }
            }
            else
            {

                if (token == null || !jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                    return;
                }

                var roleClaim = jwtToken.Claims.ToList();
                if (roleClaim.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value != _role)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                    return;
                }
                switch (roleClaim.FirstOrDefault(c => c.Type == "roleId").Value)
                {

                    case "0":

                        break;
                    default:
                        //menus.Clear();
                        var temp = int.Parse(roleClaim.FirstOrDefault(c => c.Type == "roleId").Value);
                        menus = _repo.getRoleMenuByRoleid(temp);
                        break;
                }

                if(!menus.Any(e => e == _menu) && _menu !=0)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Provider", action = "Error" }));
                    return;
                }

            }

        }
    }

}
