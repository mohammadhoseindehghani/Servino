namespace Servino.Presentation.RazorPagesUI.Configurations;

public class SiteSettings
{
    public ConnectionStringsSettings ConnectionStrings { get; set; }
    public RedisSettings Redis { get; set; }
    public SmsSettings Sms { get; set; }
}

