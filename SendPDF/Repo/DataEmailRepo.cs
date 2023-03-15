using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SendMailPDF.Common;
using SendMailPDF.Data;
using SendMailPDF.Models;
using SendMailPDF.Repo.Interface;
using System.Runtime.ConstrainedExecution;

namespace SendMailPDF.Repo
{
    public class DataEmailRepo : IDataEmailRepo
    {
        private readonly SqlDbContext _context;

        public DataEmailRepo(SqlDbContext context)
        {
            _context = context;
        }
        public DataEmail CheckDataEmail()
        {
            //List<DataEmail> data;
            string sql = "EXEC SP_CHECK_DATA_EMAIL";
            var data = _context.DataEmail.FromSqlRaw<DataEmail>(sql).ToList().FirstOrDefault();
            return data;
        }
        public DataEmail CheckDataEmailAuto()//AutoSendMail
        {
            //List<DataEmail> data;
            string sql = "EXEC SP_CHECK_DATA_EMAIL_AUTO";
            var data = _context.DataEmail.FromSqlRaw<DataEmail>(sql).ToList().FirstOrDefault();
            return data;
            //return null;
        }
        public async Task<bool> CrUpDataEmail(DataEmailModel dataEmailModel, CurrentUserModel _userInfo)
        {
            try
            {
                string sql = "EXECUTE SP_CRUP_DATA_EMAIL @subject, @body, @created_by, @checkauto";

                List<SqlParameter> parms = new List<SqlParameter>
            {
                // Create parameters    
                    new SqlParameter { ParameterName = "@subject", Value = dataEmailModel.subject },
                    new SqlParameter { ParameterName = "@body", Value = dataEmailModel.body },
                    new SqlParameter { ParameterName = "@created_by", Value = _userInfo.Id },
                    new SqlParameter { ParameterName = "@checkauto", Value = dataEmailModel.checkauto },
                };

                var dt = _context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataEmail DataEmailDetail()
        {
            try
            {
                string sql = "EXECUTE SP_DATA_EMAIL_DETAIL";
                var data = _context.DataEmail.FromSqlRaw<DataEmail>(sql).ToList().FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
