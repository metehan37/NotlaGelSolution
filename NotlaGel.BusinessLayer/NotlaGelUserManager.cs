using NotlaGel.BusinessLayer.Abstract;
using NotlaGel.BusinessLayer.Results;
using NotlaGel.Common.Helpers;
using NotlaGel.DataAccessLayer.EntityFramework;
using NotlaGel.Entities;
using NotlaGel.Entities.Messages;
using NotlaGel.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotlaGel.BusinessLayer
{
    public class NotlaGelUserManager : ManagerBase<NotlaGelUser>
    {
        
        public BusinessLayerResult<NotlaGelUser> RegisterUser(RegisterViewModel data)
        {
          NotlaGelUser user =  Find(x => x.UserName == data.Username || x.Email == data.Email);
            BusinessLayerResult<NotlaGelUser> res = new BusinessLayerResult<NotlaGelUser>();

            if (user != null)
            {
                if (user.UserName == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı");
                }
                if (user.Email==data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "E-posta adresi kayıtlı");
                }

            }
            else
            {
               int dbResult= base.Insert(new NotlaGelUser()
                {
                    UserName = data.Username,
                    Email=data.Email,
                    ProfileImageFilename="img_avatar",
                    Password=data.Password,
                    ActivateGuid=Guid.NewGuid(),
                    
                    IsActive =false,
                    IsAdmin=false
                    
                });
                if (dbResult>0)
                {
                  res.Result =  Find(x => x.Email == data.Email && x.UserName == data.Username);

                    //layerResult.Result.ActivateGuid
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.UserName};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, res.Result.Email,"NotlaGel Hesap Aktifleştirme"); 
                }
            }
            return res;
        }

        public BusinessLayerResult<NotlaGelUser> GetUserById(int id)
        {
            BusinessLayerResult<NotlaGelUser> res = new BusinessLayerResult<NotlaGelUser>();
            res.Result = Find(x => x.Id == id);

            if (res.Result==null)
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Kullanıcı Bulunamadı");
            }

            return res;
        }

        public BusinessLayerResult<NotlaGelUser> LoginUser(LoginViewModal data)
        {
            
            BusinessLayerResult<NotlaGelUser> res = new BusinessLayerResult<NotlaGelUser>();
            res.Result = Find(x => x.UserName == data.Username && x.Password == data.Password);
           

            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz");

                }

                
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrOassWrong, "Kullanıcı adı yada şifre uyuşmuyor");
            }
            return res;
        }

        public BusinessLayerResult<NotlaGelUser> UpdateProfile(NotlaGelUser data)
        {
            NotlaGelUser db_user = Find(x => x.UserName != data.UserName && (x.UserName == data.UserName || x.Email == data.Email));
            BusinessLayerResult<NotlaGelUser> res = new BusinessLayerResult<NotlaGelUser>();

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result =Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.SurName = data.SurName;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (base.update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<NotlaGelUser> RemoveUserById(int id)
        {
            BusinessLayerResult<NotlaGelUser> res = new BusinessLayerResult<NotlaGelUser>();
            NotlaGelUser user = Find(x => x.Id == id);

            if (user!=null)
            {
                if (Delete(user)==0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<NotlaGelUser> ActivateUser(Guid ActivateId)
        {
            BusinessLayerResult<NotlaGelUser> res = new BusinessLayerResult<NotlaGelUser>();
            res.Result = Find(x => x.ActivateGuid== ActivateId);

            if (res.Result !=null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }
                res.Result.IsActive = true;
                update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Aktifleştirilecek kullanıcı bulunamadı");
            }
            return res;
        }

        public new BusinessLayerResult<NotlaGelUser> Insert(NotlaGelUser data)
        {
            NotlaGelUser user = Find(x => x.UserName == data.UserName || x.Email == data.Email);
            BusinessLayerResult<NotlaGelUser> res = new BusinessLayerResult<NotlaGelUser>();

            res.Result = data;

            if (user != null)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "E-posta adresi kayıtlı");
                }

            }
            else
            {
                res.Result.ProfileImageFilename = "img_avatar";
                res.Result.ActivateGuid = Guid.NewGuid();



                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi");
                }
               
            }
            return res;
        }

        public new BusinessLayerResult<NotlaGelUser> update(NotlaGelUser data)
        {
            NotlaGelUser db_user = Find(x => x.UserName ==data.UserName || x.Email==data.Email);
            BusinessLayerResult<NotlaGelUser> res = new BusinessLayerResult<NotlaGelUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.SurName = data.SurName;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

           

            if (base.update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotFindUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }
    }
}
