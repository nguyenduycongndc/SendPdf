namespace SendMailPDF.Common
{
    public static class ResUnAuthorized
    {
        public static ResultModel Unauthor()
        {
            var detailUs = new ResultModel()
            {
                Message = "Unauthorized",
                Code = 401,
            };
            return detailUs;
        }
        public static ResultImportModel UnauthorImport()
        {
            var detailUs = new ResultImportModel()
            {
                Message = "Unauthorized",
                Code = 401,
            };
            return detailUs;
        }
    }
    
}
