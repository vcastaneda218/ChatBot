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
        [HttpGet]
        [Route("Envia")]
        [EnableCors("AllowAny")]
        public async Task<string?> Envia(string telefono,string resp)
        {

            try
            {
                var MyUrl = "https://graph.facebook.com/v18.0/";
                string Token = "EAANLmy5Lm2YBO1lLEwZB4vCwOuNfZCzPnW7gNXfWYXthTAJS5ZCeThOxuuLBXUw1XZCm83wr07bNsTh3skrTBpTDtOX9deLX9o9ZB16RCVvubD9wGjzTKZBljUhnbYomJZAKWHauCXwgs09Y7xvURtiJTijb6pGTNfD27VfN5vLZCGOiehwZALicl3oCTSM3Lrq5u";
                string IdTel = "225714317301456";
                string Telefono = telefono;


                Envia request = new Envia();
                request.messaging_product = "whatsapp";
                request.to = telefono;
                request.type = "text";
                request.text = new Text { body = resp };

                string content = JsonConvert.SerializeObject(request);
                var body = new StringContent(content, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(MyUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var url = MyUrl + IdTel + "/messages";
                var response = await client.PostAsync(url, body);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch
            {
                return null;
            }

        }

    }
}
