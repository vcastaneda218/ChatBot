using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ChatBotWS.Models
{
    public class Guarda
    {
        
        public int MensajeId { get; set; }

        public string NumeroEmisor { get; set; }

        public string WaId { get; set; }

        public string NumeroReceptor { get; set; }

        public string Mensaje {  get; set; }    

        public string Respuesta { get; set; }   

        public DateTime FechaHora { get; set; }
    }
}
