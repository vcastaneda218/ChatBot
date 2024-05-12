namespace ChatBotWS.Models.WhatsAppAdmin
{
    public class GetChatList
    {
        public string NumeroEmisor { get; set; } = null!;
        public DateTime FechaHora { get; set; }
        public int MensajeId { get; set; }
    }
}
