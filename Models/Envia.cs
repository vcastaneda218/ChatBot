namespace ChatBotWS.Models
{
    public class Envia
    {

        public string messaging_product { get; set; }
        public string to { get; set; }
        public string type { get; set; }
        public Text text { get; set; }

    }

   public class Text
    {
        public string body { get; set; }
    }

    public class GetImageInfo
    {
        public string url { get; set; }
        public string mime_type { get; set; }
        public string sha256 { get; set; }
        public string id { get; set; }
        public string messaging_product { get; set; }

    }

}
