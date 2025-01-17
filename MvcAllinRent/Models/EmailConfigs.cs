namespace MvcAllinRent.Models
{
    public class EmailConfigs
    {
        public string SmtpHost { get; set; } = null!;
        public int SmtpPort { get; set; }
        public string SmtpEmail { get; set; } = null!;
        public string SmtpPassword { get; set; } = null!;
    }
}
