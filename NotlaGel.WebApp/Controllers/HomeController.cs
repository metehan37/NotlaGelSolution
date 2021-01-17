using NotlaGel.BusinessLayer;
using NotlaGel.BusinessLayer.Results;
using NotlaGel.Common.Helpers;
using NotlaGel.Entities;
using NotlaGel.Entities.Messages;
using NotlaGel.Entities.ValueObjects;
using NotlaGel.WebApp.Filters;
using NotlaGel.WebApp.Models;
using NotlaGel.WebApp.ViewModals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace NotlaGel.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private NotlaGelUserManager NotlaGelUserManager = new NotlaGelUserManager();
        
        public ActionResult Index()
        {
            //if (TempData["mm"] != null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}

            
            return View(noteManager.ListQueryable().Where(x => x.IsDraft==false).OrderByDescending(x=>x.ModifiedOn).ToList());
            //return View(noteManager.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            //Category cat = categoryManager.Find(x=>x.Id==id.Value);
            //if (cat == null)
            //{
            //    return HttpNotFound();

            //}
            //List<Note> notes = cat.Notes.Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList();

            List<Note> notes = noteManager.ListQueryable().Where(
                x => x.IsDraft == false && x.CategoryId == id).OrderByDescending(
                x => x.ModifiedOn).ToList();

            return View("Index",notes);
        }
        public ActionResult MostLiked()
        {
            
            return View("Index",noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();
        }
        [Auth]
        public ActionResult ShowProfile()
        {
            
            BusinessLayerResult<NotlaGelUser> res= NotlaGelUserManager.GetUserById(CurrentSession.user.Id);

            if (res.Errors.Count>0)
            {
                ErrorViewModal errorNotifyObj = new ErrorViewModal()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
        [Auth]
        public ActionResult EditProfile()
        {
            
            BusinessLayerResult<NotlaGelUser> res =NotlaGelUserManager.GetUserById(CurrentSession.user.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModal errorNotifyObj = new ErrorViewModal()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
        [Auth]
        [HttpPost]
        public ActionResult EditProfile(NotlaGelUser model,HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if (ProfileImage != null && (ProfileImage.ContentType == "image/jpeg" || ProfileImage.ContentType == "image/jpg" || ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }
                
                BusinessLayerResult<NotlaGelUser> res = NotlaGelUserManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModal errorNotifyObj = new ErrorViewModal()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);
                }
                CurrentSession.Set<NotlaGelUser>("login", res.Result);

                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }
        [Auth]
        public ActionResult DeleteProfile()
        {
            
            BusinessLayerResult<NotlaGelUser> res = NotlaGelUserManager.RemoveUserById(CurrentSession.user.Id);
            if (res.Errors.Count>0)
            {
                ErrorViewModal errorNotifyObject = new ErrorViewModal()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", errorNotifyObject);
            }
            Session.Clear();

            return RedirectToAction("Index");
        }
        public ActionResult TestNotify()
        {
            ErrorViewModal modal = new ErrorViewModal()
            {
                Header = "Yönlendirme..",
                Title = "Error Test",
                RedirectingTimeout = 3000,
                Items = new List<ErrorMessageObj>() { 
                        new ErrorMessageObj() { Message= " Test başarılı 1 " },
                        new ErrorMessageObj() { Message = " Test başarılı 2 " } }
            };
            return View("Error", modal);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModal model)
        {
            if (ModelState.IsValid)
            {
                
                BusinessLayerResult<NotlaGelUser> res = NotlaGelUserManager.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate";
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                 
                    return View(model);
                }
                CurrentSession.Set<NotlaGelUser>("login", res.Result);
                return RedirectToAction("Index");
            }
           

            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                BusinessLayerResult<NotlaGelUser> res= NotlaGelUserManager.RegisterUser(model);

                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModal notifyobj = new OkViewModal()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl="/Home/Login",
                };
                notifyobj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz.Hesabınızı aktive etmeden not ekleyemez ve beğenme yapamazsınız.");
                return View("Ok", notifyobj);
            }
            //Kullanıcı username kontrolü
            return View(model);
        }
    
        public ActionResult UserActivate(Guid id)
        {
            
            BusinessLayerResult<NotlaGelUser> res= NotlaGelUserManager.ActivateUser(id);

            if (res.Errors.Count>0)
            {
                ErrorViewModal errorNotifyObj = new ErrorViewModal()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                
                return View("Error",errorNotifyObj);
            }
            OkViewModal okNotifyObj = new OkViewModal()
            {
                Title="Hesap Aktifleştirildi",
                RedirectingUrl="/Home/Login",
            };
            okNotifyObj.Items.Add("Hesabınız Aktifleştirildi. Artık not paylaşabilirsiniz.");

            return View("Ok",okNotifyObj);
        }
       
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult HasError()
        {
            return View();
        }
    }
}