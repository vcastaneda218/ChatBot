namespace ChatBotWS.Models
{
    public class EnviaImagen
    {
        public string? messaging_product { get; set; }
        public string? to { get; set; }
        public string? type { get; set; }
        public Image? image { get; set; }

    }

    public class Image
    {
        public string? link { get; set; }
    }

}

