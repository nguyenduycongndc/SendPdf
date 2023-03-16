using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SendMailPDF.Models
{
    public class DataFilePDF
    {
        public string? No { get; set; }

        public string? Departments { get; set; }//Khoa, phòng


        public string? EmployeeCode { get; set; }//Mã nhân viên


        public string? EmployeeName { get; set; }//Họ tên


        public string? EmailAddress { get; set; }//Địa chỉ email

        public string? allowance { get; set; }//Lương và các khoản phụ cấp


        public string? TNTT { get; set; }//TNTT
        public string? NCLD { get; set; }//NCLĐ
        public string? ConferenceVC { get; set; }//Hội nghị VC
        public string? TND { get; set; }//Tết nguyên đán
        public string? GMDX { get; set; }//Gặp mặt đầu xuân
        public string? AdditionalExpenses { get; set; }//Chi bổ sung thu nhập năm 2022
        public string? AdditionalRecall { get; set; }//Thu hồi bổ sung TN năm 2022 
        public string? ServiceExamination { get; set; }//DV khám chọn, khám yêu cầu
        public string? TNT { get; set; }//Trích nhà thuốc
        public string? TrainingClassTeacherMoney { get; set; }//Tiền giảng viên lớp tập huấn
        public string? MoneySAT { get; set; }//Tiền thứ 7 
        public string? MoneySUN { get; set; }//Tiền chủ nhật
        public string? OvertimePay { get; set; }//Tiền làm thêm giờ (Khoa DDLS&TC)
        public string? RequestSurgery { get; set; }//Mổ yêu cầu 
        public string? Dv247 { get; set; }//DV 24/7
        public string? VoluntarySurgery { get; set; }//Kỹ thuật dịch vụ mổ tự nguyện 
        public string? DvSurgeryPainRelief { get; set; }//DV giảm đau sau mổ 
        public string? PCDT { get; set; }//PC điện thoại 
        public string? PCK { get; set; }//Các loại PC khác  (xăng xe, công tác phí) 
        public string? TotalIncome { get; set; }//Tổng thu nhập 
        public string? CLTATT { get; set; }//Chênh lệch tiền ăn tính thuế (*)
        public string? PCUDN { get; set; }//Phụ cấp ưu đãi nghề
        public string? PCDH { get; set; }//PC độc hại, PC khu vực
        public string? TotalTaxableIncome { get; set; }//Tổng thu nhập chịu thuế (22)-(3)+(23)-(24)-(25)
        public string? Insurance { get; set; }//Các khoản BH 
        public string? TemporarilyWithdrawn { get; set; }//Đã tạm thu 10% thuế TNCN
    }
}
