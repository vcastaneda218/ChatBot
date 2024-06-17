using ChatBotWS.Data;
using ChatBotWS.Models;
using Microsoft.AspNetCore.Cors;
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
        TestContext tstcontxt = new TestContext();
        EnviaController enviacontrol = new EnviaController();

        //RECIBIMOS LOS DATOS DE VALIDACION VIA GET
        [HttpGet]
        //DENTRO DE LA RUTA webhook
        [Route("webhook")]
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
            string mensaje_recibido = "";
            string id_wa = "";
            string telefono_wa = "";
            string respuesta = "";


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
                                    var FileName = AppDomain.CurrentDomain.BaseDirectory + "/Images/" + imgid + ".jpg";

                                    using (HttpClient clientimg = new HttpClient())
                                    {
                                        clientimg.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "EAANLmy5Lm2YBO1lLEwZB4vCwOuNfZCzPnW7gNXfWYXthTAJS5ZCeThOxuuLBXUw1XZCm83wr07bNsTh3skrTBpTDtOX9deLX9o9ZB16RCVvubD9wGjzTKZBljUhnbYomJZAKWHauCXwgs09Y7xvURtiJTijb6pGTNfD27VfN5vLZCGOiehwZALicl3oCTSM3Lrq5u");
                                        clientimg.DefaultRequestHeaders.Add("User-Agent", "curl/7.64.1");
                                        using (var s = await clientimg.GetStreamAsync(ImageUrl))
                                        {

                                            File.WriteAllText("C:/Users/vicoc/source/repos/ChatBotWS/Images/test.txt", "Test");

                                            using (FileStream outputFileStream = new FileStream(FileName, FileMode.CreateNew))
                                            {
                                                await s.CopyToAsync(outputFileStream);
                                            }
                                            //// Get the object used to communicate with the server.
                                            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://216.225.195.85");
                                            //request.Method = WebRequestMethods.Ftp.UploadFile;

                                            //// This example assumes the FTP site uses anonymous logon.
                                            //request.Credentials = new NetworkCredential("mctechnologies.onlin_caf26q6m0gd", "Hugoca13@");

                                            //// Copy the contents of the file to the request stream.

                                            //    using (Stream requestStream = request.GetRequestStream())
                                            //    {
                                            //        await s.CopyToAsync(requestStream);
                                            //        using (FtpWebResponse ftpresponse = (FtpWebResponse)request.GetResponse())
                                            //        {
                                            //            Console.WriteLine($"Upload File Complete, status {ftpresponse.StatusDescription}");
                                            //        }
                                            //    }


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

                   

                    ////INICIALIZAMOS EL BOT DE RIVESCRIPT<  bn
                    //var bot = new RiveScript.RiveScript(true);
                    ////CARGAMOS EL ARCHIVO DONDE ESTA LA CONFIGURACION DE LA IA
                    //bot.loadFile("negocio.rive");
                    //bot.sortReplies();
                    ////OBTENEMOS LA RESPUESTA DEPENDIENDO DEL MENSAJE RECIBIDO
                    //respuesta = bot.reply("local-user", mensaje_recibido);
                    
                    Mensaje Newmsj = new Mensaje();
                    Newmsj.NumeroEmisor = telefono_wa;
                    Newmsj.WaId = id_wa;
                    Newmsj.NumeroReceptor = "8124282594";
                    Newmsj.Mensaje1 = mensaje_recibido;
                    Newmsj.FechaHora = DateTime.Now;


                    var contact = tstcontxt.Contactos.FirstOrDefaultAsync(x => x.Numero == telefono_wa).Result;

                    if (contact != null)
                    {
                        
                        if (contact.Chatbot == true)
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
                        Newmsj.Respuesta = "";
                    }

                    tstcontxt.Mensajes.Add(Newmsj);
                    tstcontxt.SaveChanges();
                    if (contact != null)
                    {
                        if (contact.Chatbot == true)
                        {
                            enviacontrol.Envia(telefono_wa, respuesta);
                        }
                    }
                    else
                    {
                        enviacontrol.Envia(telefono_wa, respuesta);
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
