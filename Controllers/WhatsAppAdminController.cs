using ChatBotWS.Data;
using ChatBotWS.Models;
using ChatBotWS.Models.WhatsAppAdmin;
using Grpc.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using System.Net;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatBotWS.Controllers
{
    [ApiController]
    public class WhatsAppAdminController : ControllerBase
    {
        TestContext tstcontxt = new TestContext();
        EnviaController enviacontrol = new EnviaController();


        [Route("api/[controller]/GetChatList")]
        [EnableCors("AllowAny")]
        [HttpGet]
        public ActionResult<GetChatListMessages>  GetChatList()
        {
            List<GetChatListMessages> chatlistMessages = new List<GetChatListMessages>();
            List<GetChatList> chatlist = new List<GetChatList>();
            FormattableString query =  $"SELECT (NumeroEmisor),Max(FechaHora)As FechaHora,Max(MensajeId)As MensajeId FROM Mensajes GROUP BY NumeroEmisor  ORDER BY Max(FechaHora) DESC";
            chatlist = tstcontxt.Database.SqlQuery<GetChatList>(query).ToList();

            foreach(var item in chatlist)
            {
                GetChatListMessages chatlstMessage = new GetChatListMessages();
                chatlstMessage.NumeroEmisor = item.NumeroEmisor;
                chatlstMessage.FechaHora = item.FechaHora;
                chatlstMessage.MensajeId = item.MensajeId;

                var contact = tstcontxt.Contactos.Where(x => x.Numero == item.NumeroEmisor && x.Activo == 1).FirstOrDefault();
                if (contact != null)
                {

                    chatlstMessage.NombreContacto = contact.Nombre;
                }


                var lastmessage= tstcontxt.Mensajes.Where(x => x.NumeroEmisor == item.NumeroEmisor).OrderByDescending(s => s.FechaHora).FirstOrDefault();
               if (String.IsNullOrEmpty(lastmessage.Respuesta))
                {
                    chatlstMessage.Mensaje = lastmessage.Mensaje1;
                }
                else
                {
                    chatlstMessage.Mensaje = lastmessage.Respuesta;
                }

                chatlistMessages.Add(chatlstMessage);
            } 
             
            return Ok(chatlistMessages);
        }


        [Route("api/[controller]/GetChatListByNumber/{numeroEmisor?}")]
        [EnableCors("AllowAny")]
        [HttpGet]
        public ActionResult<GetChatListByNumber> GetChatListByNumber(string numeroEmisor)
        {
            var MessageListByNumber = new GetChatListByNumber();


            var Contacto = tstcontxt.Contactos.Where(x => x.Numero == numeroEmisor && x.Activo == 1).FirstOrDefault();

            if(Contacto != null)
            {
                MessageListByNumber.NombreContacto = Contacto.Nombre;
                MessageListByNumber.Chatbot = Convert.ToBoolean(Contacto.Chatbot);
            }
           
            var messagelist = tstcontxt.Mensajes.Where(x => x.NumeroEmisor == numeroEmisor).OrderBy(s => s.FechaHora).ToList();
            MessageListByNumber.Mensajes = messagelist;

            return MessageListByNumber;
        }


        [Route("api/[controller]/AddContact")]
        [HttpPost]
        [EnableCors("AllowAny")]
    
        public async Task<ActionResult> AddContact(Contacto contacto)
        {
            tstcontxt.Contactos.Add(contacto);
            contacto.FechaCreacion = DateTime.Now;
            contacto.Activo = 1;
            await tstcontxt.SaveChangesAsync();

            return Ok(contacto.ContactoId);
        }


        [Route("api/[controller]/AddToFav/{numero?}")]
        [EnableCors("AllowAny")]
        [HttpPut]
        public async Task<ActionResult> AddToFav(string numero)
        {
            if (String.IsNullOrEmpty(numero))
            {
                return BadRequest();
            }

            var contacto = await tstcontxt.Contactos.FirstOrDefaultAsync(x => x.Numero == numero && x.Activo == 1);

            if (contacto == null)
            {
                return NotFound();
            }

            tstcontxt.Entry(contacto).State = EntityState.Modified;

            try
            {
                if(contacto.Favorito == 1)
                {
                    contacto.Favorito = 1;
                }
                else
                {
                    contacto.Favorito = 1;
                }
                await tstcontxt.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok();
        }



        [Route("api/[controller]/AddTag/{numero?}/{etiqueta?}")]
        [EnableCors("AllowAny")]
        [HttpPut]
        public async Task<ActionResult> AddTag(string numero,string etiqueta)
        {
            if (String.IsNullOrEmpty(numero))
            {
                return BadRequest();
            }

            var contacto = await tstcontxt.Contactos.FirstOrDefaultAsync(x => x.Numero == numero && x.Activo == 1);

            if (contacto == null)
            {
                return NotFound();
            }

            tstcontxt.Entry(contacto).State = EntityState.Modified;

            try
            {
                contacto.Etiqueta = etiqueta;
                await tstcontxt.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok();
        }


        //GetListContactos 
        [Route("api/[controller]/GetContactos")]
        [EnableCors("AllowAny")]
        [HttpGet]
        public async Task<ActionResult<List<Contacto>>> GetContactos()
        {
            return await tstcontxt.Contactos.Where(x => x.Activo == 1).OrderBy(s => s.FechaCreacion).ToListAsync();
        }

        [Route("api/[controller]/GetContactoById/{numero?}")]
        [EnableCors("AllowAny")]
        [HttpGet]
        public async Task<ActionResult<Contacto>> GetContactoById(string numero)
        {
            return await tstcontxt.Contactos.Where(x => x.Numero == numero && x.Activo == 1).FirstAsync();
        }


        //GetListFavoritos 
        [Route("api/[controller]/GetFavoritos")]
        [EnableCors("AllowAny")]
        [HttpGet]
        public async Task<ActionResult<List<Contacto>>> GetFavoritos()
        {
            return await tstcontxt.Contactos.Where(x => x.Activo == 1 && x.Favorito == 1).OrderBy(s => s.FechaCreacion).ToListAsync();
        }

        [Route("api/[controller]/EnviarMensajeTexto")]
        [EnableCors("AllowAny")]
        [HttpPost]
        public async Task<ActionResult> EnviarMensajeTexto([FromBody] SendMessage message)
        {

            try
            {

                Mensaje Newmsj = new Mensaje();
                Newmsj.NumeroEmisor = message.number;
                Newmsj.WaId = "my_id_wa";
                Newmsj.NumeroReceptor = "8124282594";
                Newmsj.Mensaje1 = "";
                Newmsj.Respuesta = message.message;
                Newmsj.FechaHora = DateTime.Now;
                Newmsj.Tipo = "text";


                tstcontxt.Mensajes.Add(Newmsj);
                tstcontxt.SaveChanges();


                var MyUrl = "https://graph.facebook.com/v18.0/";
                string Token = "EAANLmy5Lm2YBO1lLEwZB4vCwOuNfZCzPnW7gNXfWYXthTAJS5ZCeThOxuuLBXUw1XZCm83wr07bNsTh3skrTBpTDtOX9deLX9o9ZB16RCVvubD9wGjzTKZBljUhnbYomJZAKWHauCXwgs09Y7xvURtiJTijb6pGTNfD27VfN5vLZCGOiehwZALicl3oCTSM3Lrq5u";
                string IdTel = "225714317301456";
                string Telefono = message.number;


                Envia request = new Envia();
                request.messaging_product = "whatsapp";
                request.to = message.number;
                request.type = "text";
                request.text = new Text { body = message.message };

                string content = JsonConvert.SerializeObject(request);
                var body = new StringContent(content, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(MyUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var url = MyUrl + IdTel + "/messages";
                var response = await client.PostAsync(url, body);
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(response.StatusCode);
                }

                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("api/[controller]/EnviarMensajeImg")]
        [EnableCors("AllowAny")]
        [HttpPost]
        public async Task<ActionResult> EnviarMensajeImg([FromForm]IFormFile file,[FromForm]string number)
        {
            
            var imgid = number + "-" + DateTime.Now.ToString("ddMMyyyy-HHmmss") + "." + file.ContentType.Replace("image/", "");

            try
            {
                //Get the object used to communicate with the server.
                   FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://162.241.2.168/WEBSITES/chatboot.cabal.com.co/Images/Sended/" + imgid);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential("chatwsp@chatbot.cabal.com.co", "ddne+}k=6gSF");

                // Copy the contents of the file to the request stream.

                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await file.CopyToAsync(requestStream);
                    WebResponse ftpresponse = await request.GetResponseAsync();

                }


                Mensaje Newmsj = new Mensaje();
                Newmsj.NumeroEmisor = number;
                Newmsj.WaId = "my_id_wa";
                Newmsj.NumeroReceptor = "8124282594";
                Newmsj.Mensaje1 = "";
                Newmsj.Respuesta = imgid;
                Newmsj.FechaHora = DateTime.Now;
                Newmsj.Tipo = "image";


                tstcontxt.Mensajes.Add(Newmsj);
                tstcontxt.SaveChanges();



                var MyUrl = "https://graph.facebook.com/v18.0/";
                string Token = "EAANLmy5Lm2YBO1lLEwZB4vCwOuNfZCzPnW7gNXfWYXthTAJS5ZCeThOxuuLBXUw1XZCm83wr07bNsTh3skrTBpTDtOX9deLX9o9ZB16RCVvubD9wGjzTKZBljUhnbYomJZAKWHauCXwgs09Y7xvURtiJTijb6pGTNfD27VfN5vLZCGOiehwZALicl3oCTSM3Lrq5u";
                string IdTel = "225714317301456";



                EnviaImagen requestimg = new EnviaImagen();
                requestimg.messaging_product = "whatsapp";
                requestimg.to = number;
                requestimg.type = "image";
                requestimg.image = new Image { link = "http://www.chatbot.cabal.com.co/WEBSITES/chatboot.cabal.com.co/Images/Sended/" + imgid };

                string content = JsonConvert.SerializeObject(requestimg);
                var body = new StringContent(content, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(MyUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var url = MyUrl + IdTel + "/messages";
                var response = await client.PostAsync(url, body);

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(response.ReasonPhrase);
                }

                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("api/[controller]/EnviarMensajeDoc")]
        [EnableCors("AllowAny")]
        [HttpPost]
        public async Task<ActionResult> EnviarMensajeDoc([FromForm] IFormFile file, [FromForm] string number)
        {

            var docid = "";

            if(file.ContentType == "application/pdf")
            {
                docid = number + "-" + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".pdf";
            }
            else
            {
                docid = number + "-" + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".docx";
            }

            try
            {
                //Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://162.241.2.168/WEBSITES/chatboot.cabal.com.co/Documents/Sended/" + docid);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential("chatwsp@chatbot.cabal.com.co", "ddne+}k=6gSF");

                // Copy the contents of the file to the request stream.

                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await file.CopyToAsync(requestStream);
                    WebResponse ftpresponse = await request.GetResponseAsync();

                }


                Mensaje Newmsj = new Mensaje();
                Newmsj.NumeroEmisor = number;
                Newmsj.WaId = "my_id_wa";
                Newmsj.NumeroReceptor = "8124282594";
                Newmsj.Mensaje1 = "";
                Newmsj.Respuesta = docid;
                Newmsj.FechaHora = DateTime.Now;
                Newmsj.Tipo = "document";


                tstcontxt.Mensajes.Add(Newmsj);
                tstcontxt.SaveChanges();



                var MyUrl = "https://graph.facebook.com/v18.0/";
                string Token = "EAANLmy5Lm2YBO1lLEwZB4vCwOuNfZCzPnW7gNXfWYXthTAJS5ZCeThOxuuLBXUw1XZCm83wr07bNsTh3skrTBpTDtOX9deLX9o9ZB16RCVvubD9wGjzTKZBljUhnbYomJZAKWHauCXwgs09Y7xvURtiJTijb6pGTNfD27VfN5vLZCGOiehwZALicl3oCTSM3Lrq5u";
                string IdTel = "225714317301456";



                EnviaDocumento requestdoc = new EnviaDocumento();
                requestdoc.messaging_product = "whatsapp";
                requestdoc.to = number;
                requestdoc.type = "document";
                requestdoc.document = new Document { 
                    link = "http://www.chatbot.cabal.com.co/WEBSITES/chatboot.cabal.com.co/Documents/Sended/" + docid,
                    filename = docid              
                };

                string content = JsonConvert.SerializeObject(requestdoc);
                var body = new StringContent(content, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(MyUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var url = MyUrl + IdTel + "/messages";
                var response = await client.PostAsync(url, body);

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(response.ReasonPhrase);
                }

                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("api/[controller]/AddContacto")]
        [EnableCors("AllowAny")]
        [HttpPost]
        public async Task<ActionResult<Contacto>> AddContacto([FromBody] Contacto contacto)
        {

            contacto.Etiqueta = "Asignado";
            contacto.Chatbot = 0;
            tstcontxt.Contactos.Add(contacto);
            await tstcontxt.SaveChangesAsync();

            return Ok( new { id = contacto.ContactoId });
        }


        [Route("api/[controller]/GetHostName")]
        [EnableCors("AllowAny")]
        [HttpGet]
        public ActionResult<string> GetHostName()
        {


            return Ok(AppDomain.CurrentDomain.BaseDirectory);
        }

        //[Route("api/[controller]/Test")]
        //[EnableCors("AllowAny")]
        //[HttpPost]
        //public async Task<ActionResult> Test(IFormFile file)
        //{
        //    // Get the object used to communicate with the server.
        //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://216.225.195.85/Images/");
        //    request.Method = WebRequestMethods.Ftp.UploadFile;

        //    // This example assumes the FTP site uses anonymous logon.
        //    request.Credentials = new NetworkCredential("mctechnologies.onlin_caf26q6m0gd", "Hugoca13@");

        //    // Copy the contents of the file to the request stream.

        //    using (Stream requestStream = await request.GetRequestStreamAsync())
        //    {
        //        await file.CopyToAsync(requestStream);
        //        WebResponse ftpresponse = await request.GetResponseAsync();

        //    }

        //    return Ok();
        //}

    }
}
