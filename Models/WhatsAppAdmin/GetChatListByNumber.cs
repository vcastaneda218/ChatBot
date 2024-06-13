﻿namespace ChatBotWS.Models.WhatsAppAdmin
{
    public class GetChatListByNumber
    {
        public string NumeroEmisor { get; set; } = string.Empty;
        public string NombreContacto { get; set; } = string.Empty;
        public bool ?Chatbot { get; set; }
        public List<Mensaje>? Mensajes { get; set; }  
    }
}