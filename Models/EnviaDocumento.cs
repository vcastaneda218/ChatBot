using static System.Net.Mime.MediaTypeNames;

namespace ChatBotWS.Models
{
    public class EnviaDocumento
    {
        public string? messaging_product { get; set; }
        public string? to { get; set; }
        public string? type { get; set; }
        public Document? document { get; set; }
    }

    public class Document()
    {
       public string id { get; set; }   
       public string link { get; set; }
       public string caption { get; set; }
       public string filename { get; set; }

    }
}