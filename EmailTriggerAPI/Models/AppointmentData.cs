namespace EmailTriggerAPI.Models
{
    public class AppointmentData
    {
        //public DateTime CreatedDate { get; set; }
        //public int StoreCustomFit { get; set; }
        //public int StoreGoodsDelivery { get; set; }
        //public int WebBookingCustomFit { get; set; }

        
            public long Id { get; set; }
            public string TemplateKey { get; set; }
            public string TemplateDescription { get; set; }
            public string Subject { get; set; }
            public string EmailBodyHTMLPath { get; set; }
            public string ToEmail { get; set; }
            public string CCEmail { get; set; }
            public string BCCEmail { get; set; }
            public string ReplyToEmail { get; set; }
            public string SQLScript { get; set; }
            public string SourceDB { get; set; }
            public byte Status { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int? UpdatedBy { get; set; }
            public DateTime? UpdatedOn { get; set; }
        
    }
}
