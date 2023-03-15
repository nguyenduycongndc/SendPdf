using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using SendMailPDF.Common;
using SendMailPDF.Data;
using SendMailPDF.Models;
using SendMailPDF.Repo;
using SendMailPDF.Repo.Interface;
using SendMailPDF.Services.Interface;
using System.Text;
using System.Web.Helpers;
using static System.Net.Mime.MediaTypeNames;

namespace SendMailPDF.Services
{
    public class UserServices : IUserService
    {
        private readonly ILogger<UserServices> _logger;
        private readonly IUserRepo userRepo;
        private ResultModel Result;

        public UserServices(IUserRepo userRepo, ILogger<UserServices> logger)
        {
            this.userRepo = userRepo;
            _logger = logger;
        }
        #region Get All Data
        public async Task<ResultModel> GetAllUser(SearchUserModel searchUserModel)
        {
            var lst = await userRepo.CheckAllUser();
            var qr = await userRepo.GetAll(searchUserModel);
            var listUser = qr.Select(x => new UserModel()
            {
                Id = x.Id,
                UserName = x.UserName,
                FullName = x.FullName,
                Email = x.Email,
                IsActive = x.IsActive,
            }).OrderBy(x => x.Id).ToList();
            var data = new ResultModel()
            {
                Data = listUser,
                Message = "Successfull",
                Code = 200,
                Count = lst.Count(),
            };
            //var data = new ResultModel()
            //{
            //    Data = listUser,
            //    Count = listUser.Count(),
            //};
            return data;
        }
        public async Task<ExportUserModel> GetAllExport(SearchUserModel searchUserModel)
        {
            var qr = await userRepo.GetAll(searchUserModel);
            var listUser = qr.Select(x => new UserModel()
            {
                Id = x.Id,
                UserName = x.UserName,
                FullName = x.FullName,
                Email = x.Email,
                IsActive = x.IsActive,
            }).OrderBy(x => x.Id).ToList();
            var data = new ExportUserModel()
            {
                Data = listUser,
                Message = "Successfull",
                Code = 200,
                Count = listUser.Count(),
            };
            return data;
        }
        #endregion
        #region Create
        public async Task<ResultModel> CreateUser(CreateModel input, CurrentUserModel _userInfo)
        {
            try
            {
                var checkUser = userRepo.CheckUser(input.UserName);
                var checkRoles = await userRepo.CheckRoles(input.RoleId);
                if (checkUser.Count() > 0)
                {
                    _logger.LogError("Tài khoản đã tồn tại");
                    Result = new ResultModel()
                    {
                        Message = "Not Found",
                        Code = 404,
                    };
                    return Result;
                }
                if (checkRoles.Count() == 0)
                {
                    _logger.LogError("Không tồn tại quyền này");
                    Result = new ResultModel()
                    {
                        Message = "Not Found",
                        Code = 404,
                    };
                    return Result;
                }
                if (input.UserName == "" || input.UserName == null || input.Password == "" || input.Password == null)
                {
                    Result = new ResultModel()
                    {
                        Message = "Bad Request",
                        Code = 400,
                    };
                    return Result;
                }
                string salt = "";
                string hashedPassword = "";
                if (input != null)
                {
                    var pass = input.Password;
                    salt = Crypto.GenerateSalt(); // salt key
                    var password = input.Password + salt;
                    hashedPassword = Crypto.HashPassword(/*input.Password*/password);
                }

                UserCreateModel us = new UserCreateModel()
                {
                    UserName = input.UserName.Trim(),
                    EmailAddress = input.EmailAddress.Trim(),
                    PassWord = hashedPassword,
                    SaltKey = salt,
                    RoleId = input.RoleId,
                    CreatedBy = /*_userInfo.Id*/1,
                };
                var rs = await userRepo.CreateUs(us);
                Result = new ResultModel()
                {
                    Data = rs,
                    Message = (rs == true ? "OK" : "Bad Request"),
                    Code = (rs == true ? 200 : 400),
                };
                return Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result;
            }
        }
        #endregion
        //public CurrentUserModel GetDetailModels(int Id)
        //{
        //    try
        //    {
        //        var data = userRepo.GetDetail(Id);

        //        var detailUs = new CurrentUserModel()
        //        {
        //            Id = data.Id,
        //            UserName = data.UserName,
        //            FullName = data.FullName,
        //            IsActive = data.IsActive,
        //            Email = data.Email,
        //            RoleId = data.RoleId,
        //        };

        //        return detailUs;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return null;
        //    }
        //}
        #region Detail
        public ResultModel GetDetailModels(int Id)
        {
            try
            {
                var rs = userRepo.GetDetail(Id);
                if (rs.Count == 0)
                {
                    return Result;
                }
                else
                {
                    var detailUs = new CurrentUserModel()
                    {
                        Id = rs[0].Id,
                        UserName = rs[0].UserName,
                        FullName = rs[0].FullName,
                        IsActive = rs[0].IsActive,
                        Email = rs[0].Email,
                        RoleId = rs[0].RoleId,
                    };

                    Result = new ResultModel()
                    {
                        Data = detailUs,
                        Message = "OK"/*"Successfull"*/,
                        Code = 200,
                    };

                    return Result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result;
            }
        }
        #endregion
        //public static string EncodeServerName(string serverName)
        //{
        //    return Convert.ToBase64String(Encoding.UTF8.GetBytes(serverName));
        //}
        #region Update
        public async Task<ResultModel> UpdateUser(UpdateModel updateModel, CurrentUserModel _userInfo)
        {
            try
            {
                var checkEmailUser = new List<Users>();
                var checkUser = userRepo.GetDetail(updateModel.Id);
                if (string.IsNullOrEmpty(updateModel.Email) && updateModel.Email != checkUser[0].Email)
                {
                    checkEmailUser = userRepo.CheckEmailUser(updateModel.Email);
                    if (checkEmailUser.Count() != 0)
                    {
                        _logger.LogError("Email này đã được sử dụng");
                        Result = new ResultModel()
                        {
                            Message = "Not Found",
                            Code = 404,
                        };
                        return Result;
                    }
                }
                if (checkUser.Count() == 0)
                {
                    _logger.LogError("Tài khoản không tồn tại");
                    Result = new ResultModel()
                    {
                        Message = "Not Found",
                        Code = 404,
                    };
                    return Result;
                }

                UserUpdateModel us = new UserUpdateModel()
                {
                    Id = updateModel.Id,
                    Email = updateModel.Email,
                    IsActive = updateModel.IsActive,
                    FullName = updateModel.FullName,
                    ModifiedBy = _userInfo.Id,
                };
                var rs = await userRepo.UpdateUs(us);
                Result = new ResultModel()
                {
                    Data = rs,
                    Message = (rs == true ? "OK" : "Bad Request"),
                    Code = (rs == true ? 200 : 400),
                };
                return Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result;
            }
        }
        #endregion
        #region Delete
        public async Task<ResultModel> DeleteUser(int id, CurrentUserModel _userInfo)
        {
            try
            {
                var checkUser = userRepo.GetDetail(id);
                if (checkUser.Count() == 0)
                {
                    _logger.LogError("Tài khoản không tồn tại");
                    Result = new ResultModel()
                    {
                        Message = "Not Found",
                        Code = 404,
                    };
                    return Result;
                }
                var rs = await userRepo.DeleteUs(id, _userInfo);
                Result = new ResultModel()
                {
                    Data = rs,
                    Message = (rs == true ? "OK" : "Bad Request"),
                    Code = (rs == true ? 200 : 400),
                };
                return Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result;
            }
        }
        #endregion
        #region Change PassWord login
        public async Task<ResultModel> ChangePassWordService(ChangePassWordLoginModel input)
        {
            try
            {
                string salt = "";
                string hashedPassword = "";
                if (input != null)
                {
                    var data = userRepo.GetDetail(input.Id);
                    if (data == null)
                    {
                        Result = new ResultModel()
                        {
                            Message = "Tài khoản này không tồn tại",
                            Code = 403,
                        };
                        return Result;
                    }
                    //var user = userRepo.CheckUser(data[0].UserName);
                    //if (user == null)
                    //{
                    //    return false;
                    //}
                    var checkPass = Crypto.VerifyHashedPassword(data[0].Password, input.PassWordOld + data[0].SaltKey);
                    if (!checkPass)
                    {
                        Result = new ResultModel()
                        {
                            Message = "Mật khẩu cũ không đúng",
                            Code = 400,
                        };
                        return Result;
                    }
                    if (input.PassWordNew != input.ConfirmPassWordNew)
                    {
                        _logger.LogError("Xác nhận mật khẩu không chính xác");
                        Result = new ResultModel()
                        {
                            Message = "Xác nhận mật khẩu không chính xác",
                            Code = 400,
                        };
                        return Result;
                    }
                    var pass = input.PassWordNew;
                    salt = Crypto.GenerateSalt();
                    var password = input.PassWordNew + salt;
                    hashedPassword = Crypto.HashPassword(password);
                }
                ChangePassWordLoginSuccessModel us = new ChangePassWordLoginSuccessModel()
                {
                    Id = input.Id,
                    PassWordNew = hashedPassword,
                    SaltKey = salt,
                };
                var rs = await userRepo.ChangePassWordRepo(us);
                Result = new ResultModel()
                {
                    Data = rs,
                    Message = (rs == true ? "OK" : "Bad Request"),
                    Code = (rs == true ? 200 : 400),
                };
                return Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result;
            }
        }
        #endregion
    }
}
