using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using TiendaOnline.Clases;
using System.IO;
using MimeKit;
using MailKit.Security;

namespace TiendaOnline.Controllers
{
    public class EmailController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        public static GoogleAuthorizationCodeFlow credentialClienteNormal;
        public static GoogleAuthorizationCodeFlow credentialSuperAdmin;

        string clientID = "844514231328-vp1hd8k848790ovm35rad8gobj10lkns.apps.googleusercontent.com";
        string clientSecret = "pNuUcqdhNJAH4RHmec-_YDvB";

        //local
        string redirectUri = "http://localhost:54426/Email/EnviarEmail";


        void CrearCredencialNormal()
        {
            if (credentialClienteNormal == null)
            {
                var clientSecrets = new ClientSecrets
                {
                    ClientId = clientID,
                    ClientSecret = clientSecret
                };



                credentialClienteNormal = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = clientSecrets,
                    Scopes = new[] { "https://mail.google.com/", "https://www.googleapis.com/auth/userinfo.email", "https://www.googleapis.com/auth/userinfo.profile" }
                });

            }
        }

        //solo para documentos
        public ActionResult RedirectCentral(int? id, int? tiendaId,
            string nombreDocumento, int? usuarioId)
        {

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            tiendaId = (int)Session["TiendaId"];
            usuarioId = (int)Session["Id"];

            try
            {

                CrearCredencialNormal();

                AuthorizationCodeRequestUrl url = credentialClienteNormal.CreateAuthorizationCodeRequest(redirectUri);
                url.State = tiendaId + "|" + id + "|" + nombreDocumento + "|" + usuarioId;

                return new RedirectResult(url.Build().ToString());

            }
            catch (Exception e)
            {

                TempData["msgEmail"] = "Aviso: Sistema de correo electrónico deshabilitado temporalmente.";

                //ReporteErrores.CrearReporteError(db,
                //"Catch antes de seleccionar email. Pista: " + pista + " MensajeException= " + e.Message + " ExeptionFULL= " + e.ToString());
                return RedirectToAction("ResultadoMail", new { msgTest = "ad1" });
            }
        }

        public async Task<ActionResult> EnviarEmail(string code, string state)
        {

            string mensajeExtra = "";

            try
            {
                TokenResponse token = null;

                string[] splitState = state.Split('|');

                int idTienda = 0;
                int idUsuarioTienda = 0;
                int idUsuario = 0;
                int idDocumento = 0;
                string nombreDocumentoPDF = "";
                string nombreDestinatarioDefault = "-";

                Tienda tienda = null;

                //OBTENER VARIABLES
                idTienda = Convert.ToInt32(splitState[0]);
                idDocumento = Convert.ToInt32(splitState[1]);

                //Puede ser "", Nombre del pdf incluye idEmpresa: 15/nombrePDF.pdf
                nombreDocumentoPDF = splitState[2];
                idUsuarioTienda = Convert.ToInt32(splitState[3]);


                if (tienda == null)
                    tienda = db.Tienda.Where(e => e.Id == idTienda).FirstOrDefault();

                Usuario usuario = db.Usuarios.Where(u => u.Id == idUsuario).FirstOrDefault();

                //FuncionesGlobalesControllers.RefreshSession(this, empresa, user);

                try
                {
                    if (credentialClienteNormal == null)
                        mensajeExtra += "null1";

                    CrearCredencialNormal();

                    if (credentialClienteNormal == null)
                        mensajeExtra += "null2";

                    token = await credentialClienteNormal.ExchangeCodeForTokenAsync("", code, redirectUri, CancellationToken.None);
                }
                catch (Exception e)
                {
                    if (code == null)
                        code = "null";

                    //ReporteErrores.CrearReporteError(db,
                    //    "idEmpresa: " + idEmpresa
                    //    + "pista: " + pista
                    //    + " nombreDocumento: " + nombreDocumentoPDF
                    //    + " authCode: " + code
                    //    + " msj extra: " + mensajeExtra
                    //    + " \n redirectUri: " + redirectUri
                    //    + " \n sumario error: " + e.Message
                    //    + " \n error completo: " + e.ToString()
                    //    );
                    //TempData["msgEmail"] = "Aviso: No se pudo completar la acción. Sistema de correo electrónico no disponible.";
                    //return RedirectToAction("ResultadoMail", new { tipo = (int)tipo, msgTest = "5" });
                }

                LoginConToken(token, null, TipoTokenLogin.EnviaEmailNormal);

                Email email = new Email();
                email.IdTienda = tienda.Id;
                email.Token = token.AccessToken;
                //email.EmailDesde = emailUsuario;
                email.EmailPara = "";
                email.ClienteId = idUsuario;
                email.NombreDestinatario = nombreDestinatarioDefault;
                email.Tema = "";
                email.Mensaje = "";

                email.IdDocumento = 0;
                email.CodigoDocumento = "";
                email.Cotizacion = null;

                email.IdUsuario = idUsuarioTienda;
                email.NombreDocumento = nombreDocumentoPDF;
                email.PathDocumentoPDF = "";

                string nombreCarpeta = "";

                nombreCarpeta = "PDFEmpresas";

                Cotizacion cotizacion = tienda.Cotizaciones.Where(c => c.Id == idDocumento).FirstOrDefault();

                email.ClienteId = cotizacion.Usuario.Id;

                email.IdDocumento = cotizacion.Id;
                email.CodigoDocumento = cotizacion.Codigo;

                email.EmailPara = cotizacion.Usuario.Email;
                email.NombreDestinatario = cotizacion.Usuario.NombreCompleto;
                email.Cotizacion = cotizacion;
                email.Tema = cotizacion.Referencia + " " + cotizacion.Codigo;

                email.PathDocumentoPDF = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/img/" + nombreCarpeta + "/" + idTienda), nombreDocumentoPDF);

                ViewBag.IdTienda = idTienda;
                ViewBag.NombreCarpeta = nombreCarpeta;

                return View(email);

            }
            catch (Exception e)
            {
                TempData["msgEmail"] = "Aviso: Sistema de correo electrónico deshabilitado temporalmente.";

                //ReporteErrores.CrearReporteError(db,
                //"Catch antes de seleccionar email. Pista: " + pista + " MensajeException= " + e.Message + " ExeptionFULL= " + e.ToString());
                return RedirectToAction("ResultadoMail", new { msgTest = "ad1" });
            }
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EnviarEmail(Email model, string[] taginputcc, HttpPostedFileBase[] files)
        {
            /*
            string pathServer = Server.MapPath("~/App_Data/CotizacionesProject-77144679de36.p12");
            X509Certificate2 cert = new X509Certificate2(pathServer, "notasecret", X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(new ServiceAccountCredential
                .Initializer("richard02@cotizacionesproject-202705.iam.gserviceaccount.com")
            {
                // Note: other scopes can be found here: https://developers.google.com/gmail/api/auth/scopes
                Scopes = new[] { "https://mail.google.com/", "https://www.googleapis.com/auth/userinfo.profile" },
                User = model.EmailDesde
            }.FromCertificate(cert));*/

            //Recibo la lista de emails CC
            List<string> emailscc = taginputcc[0].Split(',').ToList<string>();

            if (taginputcc.Length > 1)
            {
                //es mayor a 1 cuando se envia email masivo
                emailscc = taginputcc.ToList();
            }

            string nombreCaperta = "";
            Cotizacion cotizacion = null;

            Tienda tienda = db.Tienda.Where(e => e.Id == model.IdTienda).FirstOrDefault();

            nombreCaperta = "PDFEmpresas";
            cotizacion = tienda.Cotizaciones.Where(c => c.Id == model.IdDocumento).FirstOrDefault();

            string fullPathDocumento = null;

            if (model.NombreDocumento != null && model.NombreDocumento != "")
                fullPathDocumento = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/img/" + nombreCaperta + "/" + tienda.Id), model.NombreDocumento);



            bool emailEnviadoCorrectamente = false;
            FileStream stmcheck2Documento = null;

            try
            {

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    var message = new MimeMessage();

                    message.From.Add(new MailboxAddress(model.EmailDesde, model.EmailDesde));

                    bool destinatariosValidos = false;


                    if (model.EmailPara != "" && model.EmailPara != null)
                    {
                        message.To.Add(new MailboxAddress(model.NombreDestinatario, model.EmailPara));
                        destinatariosValidos = true;
                    }

                    string ccsString = "";
                    //**** Agregar emails a CC ****
                    if (!emailscc[0].Equals(""))
                    {
                        ccsString += " CCs: ";
                        foreach (var cc in emailscc)
                        {
                            ccsString += cc + ";";
                            message.Cc.Add(new MailboxAddress(cc, cc));
                            destinatariosValidos = true;
                        }
                    }

                    if (destinatariosValidos == false)
                    {
                        TempData["msgEmail"] = "Error: No se ingresaron destinatarios válidos.";
                        return RedirectToAction("ResultadoMail", new { msgTest = "a" });
                    }

                    message.Subject = model.Tema;
                    String mensaje = HttpUtility.HtmlDecode(model.Mensaje);
                    //mensaje = ContenidoEmail.PrepararMensajeAGuardarOEnviar(mensaje);

                    var body = new TextPart("html")
                    {
                        Text = mensaje
                    };

                    //----------
                    var multipart = new Multipart("mixed");

                    multipart.Add(body);

                    //********DOCUMENTO*************

                    if (fullPathDocumento != null)
                    {
                        try
                        {
                            stmcheck2Documento = System.IO.File.OpenRead(fullPathDocumento);
                        }
                        catch (Exception ex)
                        {
                            string m = ex.Message;
                        }

                        var attachmentCotizacion = new MimePart("file", "pdf");
                        try
                        {

                            attachmentCotizacion.Content = new MimeContent(stmcheck2Documento, ContentEncoding.Default);
                            attachmentCotizacion.ContentDisposition = new ContentDisposition(ContentDisposition.Attachment);
                            attachmentCotizacion.ContentTransferEncoding = ContentEncoding.Base64;
                            attachmentCotizacion.FileName = Path.GetFileName(fullPathDocumento);

                            multipart.Add(attachmentCotizacion);

                        }
                        catch
                        {
                            //logging as needed
                        }


                    }


                    if (files != null)
                    {
                        foreach (HttpPostedFileBase file in files)
                        {
                            if (file == null)
                                continue;

                            MimePart attachment = new MimePart(file.ContentType);
                            attachment.Content = new MimeContent(file.InputStream, ContentEncoding.Default);
                            attachment.ContentDisposition = new ContentDisposition(ContentDisposition.Attachment);
                            attachment.ContentTransferEncoding = ContentEncoding.Base64;
                            attachment.FileName = file.FileName;
                            multipart.Add(attachment);
                        }

                    }

                    message.Body = multipart;

                    try
                    {
                        client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                        client.Timeout = 12000;
                    }
                    catch (Exception e)
                    {
                        TempData["msgEmail"] = "Se encontraron problemas de conexión. Por favor, revisar su conexión a internet.";
                        return RedirectToAction("ResultadoMail", new { msgTest = "4" });
                        //ViewBag.Message = "El no se ha podido enviar. Por favor, revisar su conexión a internet";
                    }

                    try
                    {
                        var oauth2 = new SaslMechanismOAuth2(model.EmailDesde, model.Token);
                        client.Authenticate(oauth2);
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("ErrorSeguridad", "Email");
                    }

                    try
                    {

                        client.Send(message);
                        client.Disconnect(true);
                        client.Dispose();
                    }
                    catch (Exception e)
                    {
                        TempData["msgEmail"] = "Aviso: No se pudo comprobar si se envió el email correctamente. " +
                            "<br />Causa probable es tamaño de los archivos y el tiempo que tomó enviarlos. " +
                            "<br />Por favor comprobar envío en su correo electrónico." +
                            "<br />No se generará un registro de envío en el sistema. Puede crear uno manual de ser necesario.";
                        return RedirectToAction("ResultadoMail", new { msgTest = "3" });
                    }


                    //Thread.Sleep(5000);

                    //----------REGISTRO MAIL--------

                    //RegistroEnvio regEnvio = new RegistroEnvio(plantillaemail, true, "EMAIL",
                    //    model.EmailDesde, model.EmailPara + ccsString, catalogo, "");

                    //regEnvio.TerminarAsociacionRegistro(db, cotizacion);

                    emailEnviadoCorrectamente = true;


                }
            }
            catch (Exception e)
            {
                TempData["msgEmail"] = "Error manejando la solicitud: Un problema de conexión a Internet ha impedido enviar el Email, por favor, intente nuevamente.";
                return RedirectToAction("ResultadoMail", new { msgTest = "3" });
            }

            if (stmcheck2Documento != null)
                stmcheck2Documento.Close();

            //se eliminan los archivos enviados
            var basePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/img/" + nombreCaperta + "/"), tienda.Id.ToString());
            if (Directory.Exists(basePath))
            {

                string[] fileNames = Directory.GetFiles(basePath);
                foreach (string fileName in fileNames)
                {
                    if (fileName == fullPathDocumento)
                    {
                        System.IO.File.Delete(fileName);
                    }
                }
            }

            //Asegurar mantener login abierto
            Usuario usuario = db.Usuarios.Where(u => u.Id == model.IdUsuario).FirstOrDefault();
            if (usuario != null && Session["Id"] == null)
            {
                Session["Id"] = usuario.Id;
                Session["Nombre"] = usuario.NombreUsuario;
                Session["Rol"] = usuario.RolUsuario;
                Session["EmpresaId"] = tienda.Id;
                //FuncionesGlobalesControllers.RefreshSession(this, empresa, user);
            }


            if (emailEnviadoCorrectamente)
            {
                //ReporteErrores.CrearReporteError(db,
                //    "Email enviado correctamente. user: " +user.NombreUsuario + " empresa: " + empresa.Nombre );

                TempData["msgEmail"] = "Email enviado correctamente";
                return RedirectToAction("ResultadoMail", new { msgTest = "2" });
            }
            else
            {
                TempData["msgEmail"] = "Error manejando la solicitud: Un problema de conexión a Internet ha impedido enviar el Email, por favor, intente nuevamente.";
                return RedirectToAction("ResultadoMail", new { msgTest = "1" });
            }
        }

        public enum TipoTokenLogin { EnviaEmailNormal, LoginAvisosEmpresa, LoginAvisosSuperAdmin }
        void LoginConToken(TokenResponse token, Tienda tienda, TipoTokenLogin tipoLogin)
        {
            string emailLogeado = HTTPClientReInfoToken(token.AccessToken);

            if (tipoLogin == TipoTokenLogin.EnviaEmailNormal)
            {
                if (token.RefreshToken == "" || token.RefreshToken == null)
                {
                    //no necesito refresh token para nada en esta situacion
                }
                else
                {
                    EmailConRefreshToken.CrearEmailConRefresh(db, emailLogeado, token.AccessToken, token.RefreshToken);
                }
            }

            db.SaveChanges();
        }

        //obtener info del mail del cliente y saber que mail es
        //se llama al iniciar sesion desde superadmin
        public String HTTPClientReInfoToken(string accessToken)
        {

            using (var client = new HttpClient())
            {
                var postData = new List<KeyValuePair<string, string>>();
                //postData.Add(new KeyValuePair<string, string>("access_token", accessToken));

                HttpContent content = new FormUrlEncodedContent(postData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var responseResult = client.PostAsync("https://www.googleapis.com/oauth2/v3/tokeninfo?access_token=" + accessToken, content).Result;

                string result = responseResult.Content.ReadAsStringAsync().Result;
                dynamic data = JObject.Parse(result);

                //Recibo mi nuevo AccessToken con el nuevo tiempo de expiración 
                return data.email;
            }

        }

        #region ------------------SEGURIDAD & RESULTADOS EMAIL----------------------------------------
        public ActionResult ErrorSeguridad()
        {
            ViewBag.Email = Session["Email"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ErrorSeguridad(String p)
        {

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ResultadoMail(string msgTest)
        {
            return View();
        }
        #endregion

        // GET: Email
        public ActionResult Index()
        {
            return View();
        }
    }
}