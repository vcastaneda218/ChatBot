using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using ChatBotWS.Data;
using ChatBotWS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Packaging;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;

namespace ChatBotWS.Controllers
{
    public class RecibeController
    {
        public RecibeController()
        {
            TestContext TestContext = new TestContext();
            EnviaController EnviaController = new EnviaController(null);
        }

        
        //RECIBIMOS LOS DATOS DE VALIDACION VIA GET
        [HttpGet]
        //DENTRO DE LA RUTA webhook
        [Route("Webhook")]
        //RECIBIMOS LOS PARAMETROS QUE NOS ENVIA WHATSAPP PARA VALIDAR NUESTRA URL

        public string Webhook(
            [FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.challenge")] string challenge,
            [FromQuery(Name = "hub.verify_token")] string verify_token
        )
        {
            //SI EL TOKEN ES hola (O EL QUE COLOQUEMOS EN FACEBOOK)
            if (verify_token.Equals("hola"))
            {
                return challenge;
            }
            else
            {
                return "";
            }
        }
        //RECIBIMOS LOS DATOS DE VIA POST
        [HttpPost]
        [Route("webhook")]
        //DENTRO DE LA RUTA webhook
        //RECIBIMOS LOS DATOS Y LOS GUARDAMOS EN EL MODELO WebHookResponseModel

        public async Task<dynamic> datos([FromBody] WebHookResponseModel entry)
        {
            //OBTENEMOS EL MENSAJE RECIBIDO
            string Mensaje_recibido = "";
            string id_wa = "";
            string telefono_wa = "";
            string respuesta = "";
            string tipo = "";
            Mensaje Newmsj = new Mensaje();


            if (entry.entry.Count() > 0)
            {

                if (entry.entry[0].changes[0].value.messages == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NoContent); ;
                }
                else
                {

                    if (entry.entry[0].changes[0].value.messages[0].text != null && !entry.entry[0].changes[0].value.messages[0].text.body.IsNullOrEmpty())
                    {
                        mensaje_recibido = entry.entry[0].changes[0].value.messages[0].text.body;
                        switch (mensaje_recibido.ToUpper())
                        {
                            case "HOLA":
                               respuesta = "¡Hola! 🤓 Soy CabalBot tu Asistente Virtual. \r\n\r\n " +
                                    "¿En qué puedo ayudarte?\r\n\r\n1️⃣ Consultar Menu  😎\r\n2️⃣ Realizar Pedido 💳\r\n3️⃣ Consultar recibo 📝\r\n4️⃣ Seguimiento de pedido 📅\r\n5️⃣ Tengo otra duda 🤔";
                                break;

                            case "1":
                                respuesta = "Seleccionaste la opcion 1 espera un momento por favor";
                                break;
                            case "2":
                                respuesta = "Seleccionaste la opcion 2 espera un momento por favor";
                                break;
                            case "3":
                                respuesta = "Seleccionaste la opcion 3 espera un momento por favor";
                                break;
                            case "4":
                                respuesta = "Seleccionaste la opcion 4 espera un momento por favor";
                                break;
                            case "5":
                                respuesta = "Seleccionaste la opcion 5 espera un momento por favor";
                                break;

                            default:
                                respuesta = "No entiendo lo que me dices por favor selecciona una opcion";
                                break;

                        }
                    }
                    else
                    {
                        if(entry.entry[0].changes[0].value.messages[0].type == "image")
                        {


                            using (HttpClient client = new HttpClient())
                            {
                                var imgid = entry.entry[0].changes[0].value.messages[0].image.id;
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "EAANLmy5Lm2YBO1lLEwZB4vCwOuNfZCzPnW7gNXfWYXthTAJS5ZCeThOxuuLBXUw1XZCm83wr07bNsTh3skrTBpTDtOX9deLX9o9ZB16RCVvubD9wGjzTKZBljUhnbYomJZAKWHauCXwgs09Y7xvURtiJTijb6pGTNfD27VfN5vLZCGOiehwZALicl3oCTSM3Lrq5u");
                                var JSON = await client.GetStringAsync("https://graph.facebook.com/v18.0/" + imgid);
                                
                                if (!string.IsNullOrEmpty(JSON))
                                {
                                    var ImgResponse = JsonConvert.DeserializeObject<GetImageInfo>(JSON);
                                    var ImageUrl = ImgResponse.url;
                                    var FileName = AppDomain.CurrentDomain.BaseDirectory + "/Images/Received/" + imgid + ".jpg";
                                   

                                    using (HttpClient clientimg = new HttpClient())
                                    {
                                        clientimg.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "EAANLmy5Lm2YBO1lLEwZB4vCwOuNfZCzPnW7gNXfWYXthTAJS5ZCeThOxuuLBXUw1XZCm83wr07bNsTh3skrTBpTDtOX9deLX9o9ZB16RCVvubD9wGjzTKZBljUhnbYomJZAKWHauCXwgs09Y7xvURtiJTijb6pGTNfD27VfN5vLZCGOiehwZALicl3oCTSM3Lrq5u");
                                        clientimg.DefaultRequestHeaders.Add("User-Agent", "curl/7.64.1");
                                        using (var s = await clientimg.GetStreamAsync(ImageUrl))
                                        {

                                            //EXTRAEMOS EL ID UNICO DEL MENSAJE
                                            if (!String.IsNullOrEmpty(entry.entry[0].changes[0].value.messages[0].id))
                                                id_wa = entry.entry[0].changes[0].value.messages[0].id;

                                            //ESTRAEMOS EL NUMERO DE TELEFONO DEL CUAL RECIBIMOS EL MENSAJE
                                            if (!String.IsNullOrEmpty(entry.entry[0].changes[0].value.messages[0].from))
                                                telefono_wa = entry.entry[0].changes[0].value.messages[0].from;

                                            if (!String.IsNullOrEmpty(entry.entry[0].changes[0].value.messages[0].type))
                                            {
                                                tipo = entry.entry[0].changes[0].value.messages[0].type;
                                            }

                                            Newmsj.NumeroEmisor = telefono_wa;
                                            Newmsj.WaId = id_wa;
                                            Newmsj.NumeroReceptor = "8124282594";
                                            Newmsj.Mensaje1 = imgid + ".jpg";
                                            Newmsj.Respuesta = "";
                                            Newmsj.FechaHora = DateTime.Now;
                                            Newmsj.Tipo = tipo;

                                            TestContext.Mensajes.Add(Newmsj);
                                            var resSave = await TestContext.SaveChangesAsync();

                                            //var blobServiceClient = new BlobServiceClient(
                                            //new Uri("https://navicol.blob.core.windows.net"),
                                            //new StorageSharedKeyCredential("navicol",
                                            //"7GfdRduKrEz3nHU+zdqu/61Mgw6XMiB8Y6XQXnml7bCe+f9B4PCGro/miM0Q1xrs1mPMex89JBZR+AStOrD9Pw==")
                                            //);

                                            //var containerClient = blobServiceClient.GetBlobContainerClient("wsimages");
                                            //var blobClient = containerClient.GetBlobClient(imgid + ".jpg");
                                            //var resUpl = await blobClient.UploadAsync(s, overwrite: true);

                                            //----------------------------------------------------------------
                                            //Get the object used to communicate with the server.
                                            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://162.241.2.168/WEBSITES/chatboot.cabal.com.co/Images/Received/" + imgid +".jpg");
                                            request.Method = WebRequestMethods.Ftp.UploadFile;

                                            // This example assumes the FTP site uses anonymous logon.
                                            request.Credentials = new NetworkCredential("chatwsp@chatbot.cabal.com.co", "ddne+}k=6gSF");

                                            // Copy the contents of the file to the request stream.

                                            using (Stream requestStream = await request.GetRequestStreamAsync())
                                            {
                                                await s.CopyToAsync(requestStream);
                                                WebResponse ftpresponse = await request.GetResponseAsync();

                                            }

                                        }
                                    }

                                }
                                else
                                {
                                       
                                }
                                
                            }
                        }

                        if (entry.entry[0].changes[0].value.messages[0].type == "document")
                        {

                            using (HttpClient client = new HttpClient())
                            {
                                var documentid = entry.entry[0].changes[0].value.messages[0].document.id;

                                //if ( == "application/pdf")
                                //{
                                //   documentid = entry.entry[0].changes[0].value.messages[0].document.id + ".pdf"
                                //}
                                //else
                                //{
                                //    documentid = entry.entry[0].changes[0].value.messages[0].document.id + ".docx"
                                //}
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "EAANLmy5Lm2YBO1lLEwZB4vCwOuNfZCzPnW7gNXfWYXthTAJS5ZCeThOxuuLBXUw1XZCm83wr07bNsTh3skrTBpTDtOX9deLX9o9ZB16RCVvubD9wGjzTKZBljUhnbYomJZAKWHauCXwgs09Y7xvURtiJTijb6pGTNfD27VfN5vLZCGOiehwZALicl3oCTSM3Lrq5u");
                                var JSON = await client.GetStringAsync("https://graph.facebook.com/v18.0/" + documentid);

                                if (!string.IsNullOrEmpty(JSON))
                                {
                                    var DocResponse = JsonConvert.DeserializeObject<GetImageInfo>(JSON);
                                    var DocUrl = DocResponse.url;
                                    //var FileName = documentid = entry.entry[0].changes[0].value.messages[0].document.filename;
                                    var filename = documentid + entry.entry[0].changes[0].value.messages[0].document.filename;

                                    using (HttpClient clientdoc = new HttpClient())
                                    {
                                        clientdoc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "EAANLmy5Lm2YBO1lLEwZB4vCwOuNfZCzPnW7gNXfWYXthTAJS5ZCeThOxuuLBXUw1XZCm83wr07bNsTh3skrTBpTDtOX9deLX9o9ZB16RCVvubD9wGjzTKZBljUhnbYomJZAKWHauCXwgs09Y7xvURtiJTijb6pGTNfD27VfN5vLZCGOiehwZALicl3oCTSM3Lrq5u");
                                        clientdoc.DefaultRequestHeaders.Add("User-Agent", "curl/7.64.1");
                                        using (var s = await clientdoc.GetStreamAsync(DocUrl))
                                        {

                                            //EXTRAEMOS EL ID UNICO DEL MENSAJE
                                            if (!String.IsNullOrEmpty(entry.entry[0].changes[0].value.messages[0].id))
                                                id_wa = entry.entry[0].changes[0].value.messages[0].id;

                                            //ESTRAEMOS EL NUMERO DE TELEFONO DEL CUAL RECIBIMOS EL MENSAJE
                                            if (!String.IsNullOrEmpty(entry.entry[0].changes[0].value.messages[0].from))
                                                telefono_wa = entry.entry[0].changes[0].value.messages[0].from;

                                            if (!String.IsNullOrEmpty(entry.entry[0].changes[0].value.messages[0].type))
                                            {
                                                tipo = entry.entry[0].changes[0].value.messages[0].type;
                                            }

                                            Newmsj.NumeroEmisor = telefono_wa;
                                            Newmsj.WaId = id_wa;
                                            Newmsj.NumeroReceptor = "8124282594";
                                            Newmsj.Mensaje1 = filename;
                                            Newmsj.Respuesta = "";
                                            Newmsj.FechaHora = DateTime.Now;
                                            Newmsj.Tipo = tipo;

                                            TestContext.Mensajes.Add(Newmsj);
                                            var resSave = await TestContext.SaveChangesAsync();

                                            //var blobServiceClient = new BlobServiceClient(
                                            //new Uri("https://navicol.blob.core.windows.net"),
                                            //new StorageSharedKeyCredential("navicol",
                                            //"7GfdRduKrEz3nHU+zdqu/61Mgw6XMiB8Y6XQXnml7bCe+f9B4PCGro/miM0Q1xrs1mPMex89JBZR+AStOrD9Pw==")
                                            //);

                                            //var containerClient = blobServiceClient.GetBlobContainerClient("wsimages");
                                            //var blobClient = containerClient.GetBlobClient(imgid + ".jpg");
                                            //var resUpl = await blobClient.UploadAsync(s, overwrite: true);

                                            //----------------------------------------------------------------
                                            //Get the object used to communicate with the server.
                                            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://162.241.2.168/WEBSITES/chatboot.cabal.com.co/Documents/Received/" + filename );
                                            request.Method = WebRequestMethods.Ftp.UploadFile;

                                            // This example assumes the FTP site uses anonymous logon.
                                            request.Credentials = new NetworkCredential("chatwsp@chatbot.cabal.com.co", "ddne+}k=6gSF");

                                            // Copy the contents of the file to the request stream.

                                            using (Stream requestStream = await request.GetRequestStreamAsync())
                                            {
                                                await s.CopyToAsync(requestStream);
                                                WebResponse ftpresponse = await request.GetResponseAsync();

                                            }

                                        }
                                    }

                                }
                                else
                                {

                                }

                            }
                        }

                            return new HttpResponseMessage(HttpStatusCode.NoContent);
                    }

                    //EXTRAEMOS EL ID UNICO DEL MENSAJE
                    if (!String.IsNullOrEmpty(entry.entry[0].changes[0].value.messages[0].id))
                        id_wa = entry.entry[0].changes[0].value.messages[0].id;

                    //ESTRAEMOS EL NUMERO DE TELEFONO DEL CUAL RECIBIMOS EL MENSAJE
                    if (!String.IsNullOrEmpty(entry.entry[0].changes[0].value.messages[0].from))
                        telefono_wa = entry.entry[0].changes[0].value.messages[0].from;

                    if (!String.IsNullOrEmpty(entry.entry[0].changes[0].value.messages[0].type))
                    {
                        tipo = entry.entry[0].changes[0].value.messages[0].type;
                    }



                        ////INICIALIZAMOS EL BOT DE RIVESCRIPT<  bn
                        //var bot = new RiveScript.RiveScript(true);
                        ////CARGAMOS EL ARCHIVO DONDE ESTA LA CONFIGURACION DE LA IA
                        //bot.loadFile("negocio.rive");
                        //bot.sortReplies();
                        ////OBTENEMOS LA RESPUESTA DEPENDIENDO DEL MENSAJE RECIBIDO
                        //respuesta = bot.reply("local-user", mensaje_recibido);

                    Newmsj.NumeroEmisor = telefono_wa;
                    Newmsj.WaId = id_wa;
                    Newmsj.NumeroReceptor = "8124282594";
                    Newmsj.Mensaje1 = mensaje_recibido;
                    Newmsj.FechaHora = DateTime.Now;
                    Newmsj.Tipo = tipo;

                    var contact = TestContext.Contactos.FirstOrDefaultAsync(x => x.Numero == telefono_wa).Result;

                    if (contact != null)
                    {
                        
                        if (contact.Chatbot == 1)
                        {
                            Newmsj.Respuesta = respuesta;
                        }
                        else
                        {
                            Newmsj.Respuesta = "";
                        }
                    }
                    else
                    {
                        Newmsj.Respuesta = respuesta;
                    }

                    TestContext.Mensajes.Add(Newmsj);
                    TestContext.SaveChanges();
                    if (contact != null)
                    {
                        if (contact.Chatbot == 1)
                        {
                            await EnviaController.Envia(telefono_wa, respuesta);
                        }
                    }
                    else
                    {
                        await EnviaController.Envia(telefono_wa, respuesta);
                    }

                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                    return response;
                }

            }

            else
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
        }
    }
}
