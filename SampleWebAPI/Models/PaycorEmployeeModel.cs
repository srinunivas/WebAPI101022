namespace SampleWebAPI.Models
{
    public class PaycorEmployeeModel
    {
        public string id { get; set; }
        public int? employeeNumber { get; set; }
        public int? badgeNumber { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public PaycorEmailModel email { get; set; }

        public PaycorPhoneNumberResponse phones { get; set; }
        public PaycorPayRatesModel payRates { get; set; }

        
    }


    public class PaycorEmailModel
    {
        public string type { get; set; }
        public string emailAddress { get; set; }
    }

    public class PaycorPhoneNumberResponse
    {
        public string countryCode { get; set; }
        public int areaCode { get; set; }
        public string phoneNumber { get; set; }
        public string type { get; set; }

    }

    public class PaycorPayRatesModel
    {
        public string id { get; set; }
        public int sequenceNumber { get; set; }
        public double payRate { get; set; }
        public double annualPayRate { get; set; }
        public int annualHours { get; set; }
        public string type { get; set; }
        public string reason { get; set; }
        public string notes { get; set; }
        public PayRateEmployees employees { get; set; }
    }
    public class PayRateEmployees
    {
        public string id { get; set; }
        public string url { get; set; }
    }
}
