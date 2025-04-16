//using System;
//using System.Collections.Generic;
//using System.Data;

//using Microsoft.Data.SqlClient;
//using System.IO;
//using System.Text;
//using Microsoft.Extensions.Configuration;
//using EmailTriggerAPI.Models;
//namespace EmailTriggerAPI.Service
//{
//    public class AppointmentService
//    {
//        private readonly IConfiguration _configuration;

//        public AppointmentService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }
//        public List<AppointmentData> GetDynamicTemplateDetails()
//        {
//            var connectionString = _configuration.GetConnectionString("DefaultConnection");
//            var rows = new List<AppointmentData>();

//            using (SqlConnection conn = new SqlConnection(connectionString))
//            {
//                conn.Open();
//                using (SqlCommand cmd = new SqlCommand("GetDynamicQueryDetails", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            rows.Add(new AppointmentData
//                            {
//                                Id = Convert.ToInt64(reader["Id"]),
//                                TemplateKey = reader["TemplateKey"].ToString(),
//                                TemplateDescription = reader["TemplateDescription"].ToString(),
//                                Subject = reader["Subject"].ToString(),
//                                EmailBodyHTMLPath = reader["EmailBodyHTMLPath"].ToString(),
//                                ToEmail = reader["ToEmail"].ToString(),
//                                CCEmail = reader["CCEmail"].ToString(),
//                                BCCEmail = reader["BCCEmail"]?.ToString(),
//                                ReplyToEmail = reader["ReplyToEmail"]?.ToString(),
//                                SQLScript = reader["SQLScript"].ToString(),
//                                SourceDB = reader["SourceDB"].ToString(),
//                                Status = Convert.ToByte(reader["Status"]),
//                                CreatedBy = Convert.ToInt32(reader["CreatedBy"]),
//                                CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
//                                UpdatedBy = reader["UpdatedBy"] != DBNull.Value ? Convert.ToInt32(reader["UpdatedBy"]) : (int?)null,
//                                UpdatedOn = reader["UpdatedOn"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedOn"]) : (DateTime?)null
//                            });
//                        }
//                    }
//                }
//            }

//            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Appointments.html");
//            string template = File.ReadAllText(templatePath);

//            var rowHtml = new StringBuilder();
//            foreach (var row in rows)
//            {
//                rowHtml.AppendLine($"<tr><td>{row.CreatedDate:yyyy-MM-dd}</td><td>{row.StoreCustomFit}</td><td>{row.StoreGoodsDelivery}</td><td>{row.WebBookingCustomFit}</td></tr>");
//            }

//            string emailBody = template.Replace("{{AppointmentRows}}", rowHtml.ToString());
//            return emailBody;
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using EmailTriggerAPI.Models;

namespace EmailTriggerAPI.Service
{
    public class AppointmentService
    {
        private readonly IConfiguration _configuration;

        public AppointmentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<AppointmentData> GetDynamicTemplateDetails()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var rows = new List<AppointmentData>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetDynamicQueryDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rows.Add(new AppointmentData
                            {
                                Id = Convert.ToInt64(reader["Id"]),
                                TemplateKey = reader["TemplateKey"].ToString(),
                                TemplateDescription = reader["TemplateDescription"].ToString(),
                                Subject = reader["Subject"].ToString(),
                                EmailBodyHTMLPath = reader["EmailBodyHTMLPath"].ToString(),
                                ToEmail = reader["ToEmail"].ToString(),
                                CCEmail = reader["CCEmail"].ToString(),
                                BCCEmail = reader["BCCEmail"]?.ToString(),
                                ReplyToEmail = reader["ReplyToEmail"]?.ToString(),
                                SQLScript = reader["SQLScript"].ToString(),
                                SourceDB = reader["SourceDB"].ToString(),
                                Status = Convert.ToByte(reader["Status"]),
                                CreatedBy = Convert.ToInt32(reader["CreatedBy"]),
                                CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                                UpdatedBy = reader["UpdatedBy"] != DBNull.Value ? Convert.ToInt32(reader["UpdatedBy"]) : (int?)null,
                                UpdatedOn = reader["UpdatedOn"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedOn"]) : (DateTime?)null
                            });
                        }
                    }
                }
            }

            return rows;
        }

        public string GetEmailBodyFromTemplate(string templateFilePath)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", templateFilePath);

            if (File.Exists(fullPath))
            {
                return File.ReadAllText(fullPath);
            }

            return "<p>Template not found.</p>";
        }
    }
}
