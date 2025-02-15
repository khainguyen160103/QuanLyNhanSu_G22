﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace BTL_NMCNPM
{
    public partial class FrmDoiMatKhau : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }
        public static bool IsValidPassword(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }
            if (!Regex.IsMatch(password, "[a-z]"))
            {
                return false;
            }
            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                return false;
            }
            if (!Regex.IsMatch(password, "[0-9]"))
            {
                return false;
            }
            return true;
        }
        protected string checkvalid(string MKcu , string MKmoi , string MKnhaplai )
        {
            string message = "";
            string MS_001 = "Vui lòng nhập đủ thông tin";
            string MS_002 = "Mật khẩu cần có 8 ký tự trở lên, bao gồm chữ số, chữ thường, chữ hoa";
            string MS_003 = "Mật khẩu nhập lại không đúng";
            string MS_004 = "Mật khẩu cũ không đúng";

            string taikhoan = (string)Session["Taikhoan"];

            if (MKcu == "" || MKmoi == "" || MKnhaplai=="")
            {
                message = MS_001;
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    string query = "SELECT COUNT(*) FROM tbl_TAIKHOAN WHERE sTaikhoan=@taikhoan and sMatkhau = @matkhau";
                    using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                    {
                        cmd.Parameters.AddWithValue("@taikhoan", taikhoan);
                        cmd.Parameters.AddWithValue("@matkhau" , MKcu);
                        int count = (int)cmd.ExecuteScalar();

                        Session["check"] = count;
                        if (count == 0)
                        {
                            message = MS_004;
                        }
                        else
                        {
                            if (IsValidPassword(MKmoi) == false)
                            {
                                message = MS_002;
                            }
                            else
                            {
                                if (MKnhaplai != MKmoi)
                                {
                                    message = MS_003;
                                }
                            }
                        }
                    }
                }
            }
            return message;
        }
        protected void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            string MS_001 = "Vui lòng nhập đủ thông tin";
            string MS_002 = "Mật khẩu cần có 8 ký tự trở lên, bao gồm chữ số, chữ thường, chữ hoa";
            string MS_003 = "Mật khẩu nhập lại không đúng";
            string MS_004 = "Mật khẩu cũ không đúng";

            string matkhaucu = txtMatKhauCu.Value;
            string matkhaumoi = txtMatKhauMoi.Value;
            string nhaplaimatkhau = txtNhapLaiMatKhauMoi.Value;
            string taikhoan = (string)Session["Taikhoan"];
            

            string message = checkvalid(matkhaucu , matkhaumoi ,nhaplaimatkhau);
            if (message == "")
            {
                string connectString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
                using (SqlConnection sqlcon = new SqlConnection(connectString))
                {
                    sqlcon.Open();
                    string query = "UPDATE tbl_TAIKHOAN set sMatkhau = @matkhaumoi where sTaikhoan = @taikhoan";
                    using (SqlCommand cmd = new SqlCommand(query,sqlcon))
                    {
                        cmd.Parameters.AddWithValue("@matkhaumoi" , matkhaumoi);
                        cmd.Parameters.AddWithValue("@taikhoan" , taikhoan);

                        cmd.ExecuteNonQuery();
                        
                    }
                }
                Response.Redirect("FrmDangNhap.aspx");
            }else if (message == MS_002)
            {
                messageMK2.InnerText = MS_002;
                messageMK3.InnerText = "";
            }else if (message == MS_003)
            {
                messageMK3.InnerText = MS_003;
                messageMK1.InnerText = "";
                messageMK2.InnerText = "";
            }
            else if(message == MS_004)
            {
                messageMK1.InnerText = MS_004;
                messageMK2.InnerText = "";
                messageMK3.InnerText = "";
            }
            else
            {
                messageMK3.InnerText = MS_001;
                messageMK2.InnerText = "";
                messageMK1.InnerText = "";
            }

            
        }
    }
}