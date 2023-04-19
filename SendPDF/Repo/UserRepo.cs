using DocumentFormat.OpenXml.InkML;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SendMailPDF.Data;
using SendMailPDF.Models;
using SendMailPDF.Repo.Interface;

namespace SendMailPDF.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly SqlDbContext context;

        public UserRepo(SqlDbContext context)
        {
            this.context = context;
        }
        #region Search
        public async Task<List<Users>> GetAll(SearchUserModel searchUserModel)
        {
            try
            {
                List<Users> listData;
                var list = context.Users.ToList().Where(x => (x.UserName.ToLower().Contains(searchUserModel.UserName.ToLower()) || string.IsNullOrEmpty(searchUserModel.UserName))
                                              && (searchUserModel.IsActive == -1 || (searchUserModel.IsActive == 1 ? x.IsActive == 1 : x.IsActive == 0))).Select(x => new Users()
                                              {
                                                  Id = x.Id,
                                                  UserName = x.UserName,
                                                  FullName = x.FullName,
                                                  IsActive = x.IsActive,
                                              }).OrderBy(x => x.Id).ToList();
                listData = list.Skip(searchUserModel.StartNumber).Take(searchUserModel.PageSize).ToList();
                return listData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //try
            //{
            //    List<Users> list;
            //    string sql = "EXECUTE SP_SEARCH_USER @user_name, @is_active, @start_number, @page_size";

            //    List<SqlParameter> parms = new List<SqlParameter>
            //    {
            //        new SqlParameter { ParameterName = "@user_name", Value = searchUserModel.UserName },
            //        new SqlParameter { ParameterName = "@is_active", Value = searchUserModel.IsActive },
            //        new SqlParameter { ParameterName = "@start_number", Value = searchUserModel.StartNumber },
            //        new SqlParameter { ParameterName = "@page_size", Value = searchUserModel.PageSize }
            //    };
            //    list = await context.Users.FromSqlRaw<Users>(sql, parms.ToArray()).ToListAsync();

            //    return list;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        #endregion
        //public async Task<bool> CreateUs(Users user, UsersRoles usersRoles)
        #region Create
        public async Task<bool> CreateUs(UserCreateModel user)
        {
            try
            {
                string sql = "EXECUTE SP_CREATE_USER @user_name, @email_address, @role_id, @password, @salt, @created_by";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@user_name", Value = user.UserName },
                    new SqlParameter { ParameterName = "@email_address", Value = user.EmailAddress },
                    new SqlParameter { ParameterName = "@role_id", Value = user.RoleId },
                    new SqlParameter { ParameterName = "@password", Value = user.PassWord },
                    new SqlParameter { ParameterName = "@salt", Value = user.SaltKey },
                    new SqlParameter { ParameterName = "@created_by", Value = user.CreatedBy },
                };

                var dt = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Check
        public List<Users> CheckUser(string userName)
        {
            List<Users> list;
            string sql = "EXECUTE SP_CHECK_USER @user_name";


            List<SqlParameter> parms = new List<SqlParameter>
            { 
                // Create parameters    
                new SqlParameter { ParameterName = "@user_name", Value = userName },
            };
            list = context.Users.FromSqlRaw<Users>(sql, parms.ToArray()).ToList();
            return list;
        }
        public async Task<List<Roles>> CheckRoles(int RolesId)
        {
            List<Roles> list;
            string sql = "EXECUTE SP_CHECK_ROLES @roles_id";


            List<SqlParameter> parms = new List<SqlParameter>
            { 
                // Create parameters    
                new SqlParameter { ParameterName = "@roles_id", Value = RolesId },
            };
            list = await context.Roles.FromSqlRaw<Roles>(sql, parms.ToArray()).ToListAsync();
            return list;
        }
        public List<Users> GetDetail(int id)
        {
            List<Users> list;
            string sql = "EXECUTE SP_DETAIL_USER @user_id";


            List<SqlParameter> parms = new List<SqlParameter>
            { 
                // Create parameters    
                new SqlParameter { ParameterName = "@user_id", Value = id },
            };
            list = context.Users.FromSqlRaw<Users>(sql, parms.ToArray()).ToList();
            return list;
        }

        public List<Users> CheckEmailUser(string email)
        {
            List<Users> list;
            string sql = "EXECUTE SP_CHECK_EMAIL_USER @email";


            List<SqlParameter> parms = new List<SqlParameter>
            { 
                // Create parameters    
                new SqlParameter { ParameterName = "@email", Value = email },
            };
            list = context.Users.FromSqlRaw<Users>(sql, parms.ToArray()).ToList();
            return list;
        }
        public List<Users> CheckAllEmailUser()//AutoSendMail
        {
            //return null;
            List<Users> list;
            string sql = "EXEC SP_CHECK_ALL_EMAIL_USER";
            list = context.Users.FromSqlRaw<Users>(sql).ToList();
            return list;
        }

        public List<Users> CheckOTP(checkOTPModel checkOTPModel)
        {
            List<Users> list;
            string sql = "EXECUTE SP_CHECK_OTP @email, @otp";


            List<SqlParameter> parms = new List<SqlParameter>
            { 
                // Create parameters    
                new SqlParameter { ParameterName = "@email", Value = checkOTPModel.Email },
                new SqlParameter { ParameterName = "@otp", Value = checkOTPModel.OTP },
            };
            list = context.Users.FromSqlRaw<Users>(sql, parms.ToArray()).ToList();
            return list;
        }
        public async Task<List<Users>> CheckAllUser()
        {
            List<Users> list;
            string sql = "EXEC SP_CHECK_ALL_USER";
            list = await context.Users.FromSqlRaw<Users>(sql).ToListAsync();
            return list;
        }
        #endregion
        #region Update
        public async Task<bool> UpdateUs(UserUpdateModel user)
        {
            try
            {
                string sql = "EXECUTE SP_UPDATE_USER @id, @full_name, @email, @is_active, @modified_by";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    new SqlParameter { ParameterName = "@id", Value = user.Id },
                    new SqlParameter { ParameterName = "@full_name", Value = user.FullName },
                    new SqlParameter { ParameterName = "@email", Value = user.Email },
                    new SqlParameter { ParameterName = "@is_active", Value = user.IsActive },
                    new SqlParameter { ParameterName = "@modified_by", Value = user.ModifiedBy },
                }; 

                var dt = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Delete
        public async Task<bool> DeleteUs(int id, CurrentUserModel _userInfo)
        {
            try
            {
                string sql = "EXECUTE SP_DELETE_USER @id, @deleted_by";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@deleted_by", Value = _userInfo.Id },
                }; 

                var dt = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region send OTP
        public async Task<bool> UpdateOTPUs(UserUpdateOTPModel userUpdateOTPModel)
        {
            try
            {
                string sql = "EXECUTE SP_UPDATE_USER_OTP @email, @otp, @expdate";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@email", Value = userUpdateOTPModel.Email },
                    new SqlParameter { ParameterName = "@otp", Value = int.Parse(userUpdateOTPModel.OTP) },
                    new SqlParameter { ParameterName = "@expdate", Value = userUpdateOTPModel.Expdate },
                };

                var dt = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ForgotPassWord
        public async Task<bool> ForgotPassWordUs(ChangePassWordModel changePassWordModel)
        {
            try
            {
                string sql = "EXECUTE SP_FORGOT_PASSWORD_USER @email, @password, @salt";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@email", Value = changePassWordModel.Email },
                    new SqlParameter { ParameterName = "@password", Value = changePassWordModel.NewPassWord },
                    new SqlParameter { ParameterName = "@salt", Value = changePassWordModel.SaltKey },
                };

                var dt = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region change password
        public async Task<bool> ChangePassWordRepo(ChangePassWordLoginSuccessModel changePassWordLoginSuccessModel)
        {
            string sql = "EXECUTE SP_CHANGE_PASSWORD_LOGIN @id, @passwordnew, @salt";

            List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@id", Value = changePassWordLoginSuccessModel.Id },
                    new SqlParameter { ParameterName = "@passwordnew", Value = changePassWordLoginSuccessModel.PassWordNew },
                    new SqlParameter { ParameterName = "@salt", Value = changePassWordLoginSuccessModel.SaltKey },
                };

            var dt = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
            return true;
        }
        #endregion
        #region EditInformation
        public async Task<bool> EditInformationUs(UserEditInformationModel user)
        {
            try
            {
                string sql = "EXECUTE SP_EDIT_INFORMATION_USER @id, @email, @email_password, @modified_by";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@id", Value = user.Id },
                    new SqlParameter { ParameterName = "@email", Value = user.Email },
                    new SqlParameter { ParameterName = "@email_password", Value = user.EmailPassword },
                    new SqlParameter { ParameterName = "@modified_by", Value = user.ModifiedBy },
                };

                var dt = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
