namespace MetricsAgent.Models.Options
{
    public class ServiceSettings
    {
        public MailServiceSettings MailServiceSettings { get; set; }
        public SmsServiceSettings SmsServiceSettings { get; set; }
    }

    public class MailServiceSettings
    {
        public string Host { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class SmsServiceSettings
    {
        public string Host { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

    }

}
