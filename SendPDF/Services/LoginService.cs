using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SendMailPDF.Data;
using SendMailPDF.Repo.Interface;
using SendMailPDF.Services.Interface;
using Microsoft.Extensions.Configuration;
using System.Web.Helpers;
using System.Security.Cryptography;
using SendMailPDF.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.WebRequestMethods;
using System.Globalization;
using SendMailPDF.Models;

namespace SendMailPDF.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly IUserRepo userRepo;
        private readonly IConfiguration _config;
        private ResultModel Result;
        public LoginService(ILogger<LoginService> logger, IUserRepo loginRepo, IConfiguration config)
        {
            _logger = logger;
            this.userRepo = loginRepo;
            _config = config;
        }
        public ResultModel Login(InputLoginModel inputModel)
        {
            try
            {
                ResultModel userdetai = new ResultModel();
                if (inputModel.UserName != "" && inputModel.UserName != null && inputModel.PassWord != "" && inputModel.PassWord != null)
                {
                    //var user = userRepo.GetDetailByName(inputModel);
                    var user = userRepo.CheckUser(inputModel.UserName);
                    if (user == null)
                    {
                        return ResUnAuthorized.Unauthor();
                    }
                    var checkPass = Crypto.VerifyHashedPassword(user[0].Password, inputModel.PassWord + user[0].SaltKey);
                    if (!checkPass)
                    {
                        return ResUnAuthorized.Unauthor();
                    }
                    userdetai = new ResultModel()
                    {
                        Message = "OK",
                        Code = 200,
                        Token = GenerateJwt(user[0]),
                    };
                    var Au = AuthenticateUser(inputModel);

                }
                return userdetai;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResUnAuthorized.Unauthor();
            }
        }

        public string GenerateJwt(Users user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            DateTime jwtDate = DateTime.UtcNow;
            //Header
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalSetting.Secret));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            //Header
            var expires = DateTime.UtcNow.AddHours(24);
            //Payload
            var token = new JwtSecurityToken(
                issuer: "http://::80",
                audience: "http://::80",
                claims,
                notBefore: jwtDate,
                expires: expires,
                signingCredentials: creds
            );
            //Payload
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private CurrentUserModel AuthenticateUser(InputLoginModel inputModel)
        {
            CurrentUserModel user = new CurrentUserModel();
            try
            {
                //var data = userRepo.GetDetailByName(inputModel);
                var data = userRepo.CheckUser(inputModel.UserName);
                user = new CurrentUserModel()
                {
                    Id = data[0].Id,
                    FullName = data[0].FullName,
                    UserName = data[0].UserName,
                    RoleId = data[0].RoleId,
                };
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"USER-LOGIN - {inputModel.UserName} : {ex.Message}!");
                return user;
            }
        }

        public async Task<ResultModel> ForgotPassWordAsync(ForgotPassWordModel forgotPassWordModel)
        {
            try
            {
                if ((forgotPassWordModel.NewPassWord != "" || forgotPassWordModel.NewPassWord != null) 
                    && (forgotPassWordModel.ConfirmPassWord != "" || forgotPassWordModel.ConfirmPassWord != null) 
                    && (forgotPassWordModel.OTP != "" || forgotPassWordModel.OTP != null)
                    && (forgotPassWordModel.NewPassWord == forgotPassWordModel.ConfirmPassWord))
                {
                    var checkOTP = new checkOTPModel()
                    {
                        Email = forgotPassWordModel.Email,
                        OTP = forgotPassWordModel.OTP,
                    };
                    var otp = userRepo.CheckOTP(checkOTP);
                    if (otp.Count == 0)
                    {
                        _logger.LogError("Mã OTP này không khả dụng");
                        Result = new ResultModel()
                        {
                            Message = "Mã OTP này không khả dụng",
                            Code = 404,
                        };
                        return Result;
                    }
                    string salt = "";
                    string hashedPassword = "";
                    if (forgotPassWordModel != null)
                    {
                        var pass = forgotPassWordModel.NewPassWord;
                        salt = Crypto.GenerateSalt();
                        var password = forgotPassWordModel.NewPassWord + salt;
                        hashedPassword = Crypto.HashPassword(password);
                    }
                    ChangePassWordModel us = new ChangePassWordModel()
                    {
                        Email = forgotPassWordModel.Email,
                        NewPassWord = hashedPassword,
                        SaltKey = salt,
                    };
                    var rs = await userRepo.ForgotPassWordUs(us);
                    Result = new ResultModel()
                    {
                        Data = rs,
                        Message = (rs == true ? "OK" : "Bad Request"),
                        Code = (rs == true ? 200 : 400),
                    };
                }
                return Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result;
            }
        }
    }
}
