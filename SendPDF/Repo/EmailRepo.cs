using DocumentFormat.OpenXml.InkML;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SendMailPDF.Data;
using SendMailPDF.Models;
using SendMailPDF.Repo.Interface;

namespace SendMailPDF.Repo
{
    public class EmailRepo : IEmailRepo
    {
        private readonly SqlDbContext _context;

        public EmailRepo(SqlDbContext context)
        {
            _context = context;
        }
        //public List<Email> GetAllEmail()
        //{
        //    return _context.Email.ToList();
        //}
        #region Search
        public async Task<List<Email>> GetAllEmail(EmailSearchModel emailSearchModel)
        {
            List<Email> list;
            try
            {
                string sql = "EXECUTE SP_ALL_EMAIL @email_address, @start_number, @page_size";
                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    new SqlParameter { ParameterName = "@email_address", Value = emailSearchModel.email_address },
                    new SqlParameter { ParameterName = "@start_number", Value = emailSearchModel.StartNumber },
                    new SqlParameter { ParameterName = "@page_size", Value = emailSearchModel.PageSize }
                };
                list = await _context.Email.FromSqlRaw<Email>(sql, parms.ToArray()).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region check
        public List<Email> CheckEmail(string email)
        {
            List<Email> list;
            string sql = "EXECUTE SP_CHECK_EMAIL @email_address";


            List<SqlParameter> parms = new List<SqlParameter>
            { 
                // Create parameters    
                new SqlParameter { ParameterName = "@email_address", Value = email },
            };
            list = _context.Email.FromSqlRaw<Email>(sql, parms.ToArray()).ToList();
            return list;
        }
        public async Task<List<Email>> CheckAllEmail()
        {
            //return null;
            List<Email> list;
            string sql = "EXEC SP_CHECK_ALL_EMAIL";
            list = _context.Email.FromSqlRaw<Email>(sql).ToList();
            return list;
        }
        #endregion
        #region Create
        public async Task<bool> CreateEmailR(EmailCrModel cre)
        {
            try
            {
                string sql = "EXECUTE SP_CREATE_EMAIL @email_address, @cc";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@email_address", Value = cre.email_address },
                    new SqlParameter { ParameterName = "@cc", Value = cre.cc },
                };

                var dt = await _context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Detail
        public List<Email> GetDetailEmailR(int id)
        {
            List<Email> list;
            string sql = "EXECUTE SP_DETAIL_EMAIL @email_id";


            List<SqlParameter> parms = new List<SqlParameter>
            { 
                // Create parameters    
                new SqlParameter { ParameterName = "@email_id", Value = id },
            };
            list = _context.Email.FromSqlRaw<Email>(sql, parms.ToArray()).ToList();
            return list;
        }
        #endregion
        #region Delete
        public async Task<bool> DeleteEmailR(int id, CurrentUserModel _userInfo)
        {
            try
            {
                string sql = "EXECUTE SP_DELETE_EMAIL @id, @deleted_by";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@deleted_by", Value = _userInfo.Id },
                };

                var dt = await _context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Update
        public async Task<bool> UpdateEmail(EmailUpdateModel emailModel)
        {
            try
            {
                string sql = "EXECUTE SP_UPDATE_EMAIL @id, @email_address, @cc";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@id", Value = emailModel.Id },
                    new SqlParameter { ParameterName = "@email_address", Value = emailModel.email_address },
                    new SqlParameter { ParameterName = "@cc", Value = emailModel.cc },
                };

                var dt = await _context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
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
