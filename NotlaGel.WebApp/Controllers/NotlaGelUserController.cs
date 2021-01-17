using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NotlaGel.BusinessLayer;
using NotlaGel.BusinessLayer.Results;
using NotlaGel.Entities;
using NotlaGel.WebApp.Filters;

namespace NotlaGel.WebApp.Controllers
{
    [Auth]
    [AuthAdmin]
    [Exc]
    public class NotlaGelUserController : Controller
    {
        private NotlaGelUserManager notlaGelUserManager =new NotlaGelUserManager();

        // GET: NotlaGelUser
        public ActionResult Index()
        {
            return View(notlaGelUserManager.List());
        }

        // GET: NotlaGelUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotlaGelUser notlaGelUser = notlaGelUserManager.Find(x=>x.Id==id.Value);
            if (notlaGelUser == null)
            {
                return HttpNotFound();
            }
            return View(notlaGelUser);
        }

        // GET: NotlaGelUser/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NotlaGelUser notlaGelUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<NotlaGelUser> res = notlaGelUserManager.Insert(notlaGelUser);
                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(notlaGelUser);
                }
                return RedirectToAction("Index");
            }

            return View(notlaGelUser);
        }

        // GET: NotlaGelUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotlaGelUser notlaGelUser = notlaGelUserManager.Find(x => x.Id == id.Value);
            if (notlaGelUser == null)
            {
                return HttpNotFound();
            }
            return View(notlaGelUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NotlaGelUser notlaGelUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<NotlaGelUser> res = notlaGelUserManager.update(notlaGelUser);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(notlaGelUser);
                }

                return RedirectToAction("Index");
            }
            return View(notlaGelUser);
        }

        // GET: NotlaGelUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotlaGelUser notlaGelUser = notlaGelUserManager.Find(x => x.Id == id.Value);
            if (notlaGelUser == null)
            {
                return HttpNotFound();
            }
            return View(notlaGelUser);
        }

        // POST: NotlaGelUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NotlaGelUser notlaGelUser = notlaGelUserManager.Find(x => x.Id == id);
            notlaGelUserManager.Delete(notlaGelUser);
            return RedirectToAction("Index");
        }
    }
}
