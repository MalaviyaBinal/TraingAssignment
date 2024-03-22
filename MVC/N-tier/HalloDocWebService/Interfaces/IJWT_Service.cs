using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocWebServices.Interfaces
{
    public interface IJWT_Service
    {
        string GenerateToken(Aspnetuser user);
        bool ValidateToken(string token,out JwtSecurityToken jwttoken);
    }
}
