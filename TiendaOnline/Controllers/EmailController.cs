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

            int pista = 0;
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


                    pista = 10;

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

        // GET: Email
        public ActionResult Index()
        {
            return View();
        }
    }
}