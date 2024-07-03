namespace ChatBotWS.Models.WhatsAppAdmin
{
    public class SendImage
    {
        public string number { get; set; } = string.Empty;

        public FormFile file { get; set; } 
    }
}
