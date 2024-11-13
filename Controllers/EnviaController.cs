using ChatBotWS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ChatBotWS.Controllers
{
    public class EnviaController : Controller
    {

        private readonly string Token;
        private readonly string MyUrl;
        private readonly string IdPhone;

        public EnviaController(IConfiguration Config)
        {
            Token = Config.GetValue("GrandParent_Key:Parent_Key", "1");
            MyUrl = Config.GetValue("GrandParent_Key:Parent_Key", "2");
            IdPhone = Config.GetValue("GrandParent_Key:Parent_Key", "3");
        }

        [HttpGet]
        [Route("Envia")]
        [EnableCors("AllowAny")]
        public async Task<string?> Envia(string PhoneNumber, string Resp)
        {

            try
            {

                string IdTel = IdPhone;
                string Telefono = PhoneNumber;

                Envia Request = new Envia();
                Request.messaging_product = "whatsapp";
                Request.to = PhoneNumber;
                Request.type = "text";
                Request.text = new Text { body = Resp };

                string Content = JsonConvert.SerializeObject(Request);
                var Body = new StringContent(Content, Encoding.UTF8, "application/json");
                var Client = new HttpClient();
                Client.BaseAddress = new Uri(MyUrl);
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var Url = MyUrl + IdTel + "/messages";
                var Response = await Client.PostAsync(Url, Body);
                if (!Response.IsSuccessStatusCode)
                {
                    return null;
                }

                var Result = await Response.Content.ReadAsStringAsync();

                return Result;
            }
            catch
            {
                return null;
            }

        }

    }
}
