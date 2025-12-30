
using EquipmentShop.Core.Entities;

namespace EquipmentShop.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendOrderConfirmationAsync(Order order);
        Task SendOrderShippedAsync(Order order);
        Task SendOrderDeliveredAsync(Order order);
        Task SendPasswordResetAsync(string email, string resetLink);
        Task SendWelcomeEmailAsync(string email, string userName);
        Task SendNewsletterAsync(List<string> emails, string subject, string content);
        Task SendContactFormAsync(string name, string email, string message);
    }

    public class EmailMessage
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; } = true;
        public List<EmailAttachment> Attachments { get; set; } = new();
    }

    public class EmailAttachment
    {
        public string FileName { get; set; } = string.Empty;
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "application/octet-stream";
    }
}
