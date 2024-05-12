namespace ChatBotWS.Models.WhatsAppAdmin
{
    public class GetChatListMessages
    {
        public string NumeroEmisor { get; set; } = null!;
        public DateTime FechaHora { get; set; }
        public int MensajeId { get; set; }
        public string Mensaje { get; set; } = null!;
    }
}
