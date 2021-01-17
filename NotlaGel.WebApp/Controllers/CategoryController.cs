using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NotlaGel.BusinessLayer;
using NotlaGel.Entities;
using NotlaGel.WebApp.Filters;
using NotlaGel.WebApp.Models;

namespace NotlaGel.WebApp.Controllers
{
    [Auth]
    [AuthAdmin]
    [Exc]
    public class CategoryController : Controller
    {
        private CategoryManager categoryManager = new CategoryManager();

        // GET: Categori
        public ActionResult Index()
        {
            return View(categoryManager.List());
        }

        // GET: Categori/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x=>x.Id==id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categori/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categori/Create   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                categoryManager.Insert(category);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Category cat = categoryManager.Find(x => x.Id == category.Id);
                cat.Title = category.Title;
                cat.Description = category.Description;

                categoryManager.update(cat);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }
            return View(category);
        }

       
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categoryManager.Find(x => x.Id == id);
            categoryManager.Delete(category);
            CacheHelper.RemoveCategoriesFromCache();

            return RedirectToAction("Index");
        }

 
    }
}
