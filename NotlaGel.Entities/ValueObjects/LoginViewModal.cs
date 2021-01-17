using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotlaGel.Entities.ValueObjects
{
    public class LoginViewModal
    {
        [DisplayName("kullanıcı adı"), Required(ErrorMessage = "{0} alanı boş geçilmez"), StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı")]
        public string Username { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilmez"),DataType(DataType.Password), StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı")]
        public string Password { get; set; }
    }
}