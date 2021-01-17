using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotlaGel.Entities
{
    [Table("NotlaGelUser")]
    public class NotlaGelUser:MyEntityBase
    {
        [DisplayName("İsim"),StringLength(25,ErrorMessage ="{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; }
        [DisplayName("Soyad"), StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string SurName { get; set; }
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage ="{0} alanı gereklidir"),StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string UserName { get; set; }
        [DisplayName("Email"), Required(ErrorMessage = "{0} alanı gereklidir"), StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı gereklidir"), StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Password { get; set; }
        [StringLength(30), ScaffoldColumn(false)]
        public string ProfileImageFilename { get; set; }
        [DisplayName("Onaylanmış Hesap")]
        public bool IsActive { get; set; }
        [DisplayName("Ynetici Hesap")]
        public bool IsAdmin { get; set; }
        [Required, ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
