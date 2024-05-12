namespace ChatBotWS.Models
{
    public class Datos
    {
    }
    public class WebHookResponseModel
    {
        public Entry[] entry { get; set; }
    }
    public class Entry
    {
        public Change[] changes { get; set; }
    }
    public class Change
    {
        public Value value { get; set; }
    }
    public class Value
    {
        public int ad_id { get; set; }
        public long form_id { get; set; }
        public long leadgen_id { get; set; }
        public int created_time { get; set; }
        public long page_id { get; set; }
        public int adgroup_id { get; set; }
        public Messages[] messages { get; set; }
    }
    public class Messages
    {
        public string id { get; set; }
        public string from { get; set; }

        public string type { get; set; }
        public TextRecibe text { get; set; }
        public ImgRecibe image { get; set; }
    }

    public class ImgRecibe
    { 
        public string caption { get; set; }
        public string mime_type { get; set; }
        public string sha256 { get; set; }
        public string id { get; set; }
    }
    public class TextRecibe
    {
        public string body { get; set; }
    }

}
