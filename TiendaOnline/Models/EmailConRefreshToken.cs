using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;

namespace TiendaOnline.Models
{
    [Table("EmailConRefreshToken")]
    public class EmailConRefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
         
        public static void CrearEmailConRefresh(TiendaOnlineContext _db, string email, string accessToken, string refreshToken)
        {
            EmailConRefreshToken existe = _db.EmailsConRefreshTokens.Where(e => e.Email == email.ToUpper()).FirstOrDefault();

            if (existe != null)
            {
                existe.AccessToken = accessToken;
                existe.RefreshToken = refreshToken;
            }
            else
            {

                EmailConRefreshToken nuevo = new EmailConRefreshToken();
                nuevo.Email = email.ToUpper();
                nuevo.AccessToken = accessToken;
                nuevo.RefreshToken = refreshToken;
                //_db.EmailsConRefreshTokens.Add(nuevo);
            }
            _db.SaveChanges();

        }

        public static string GetRefreshToken(TiendaOnlineContext _db, string email)
        {
            EmailConRefreshToken emailConToken = _db.EmailsConRefreshTokens.Where(e => e.Email == email.ToUpper()).FirstOrDefault();

            if (emailConToken != null)
                return emailConToken.RefreshToken;

            return "";
        }
    }
}