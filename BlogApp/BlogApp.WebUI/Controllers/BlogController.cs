using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository _blogRepo;
        private ICategoryRepository _categoryRepo;
        public BlogController(IBlogRepository blogRepo, ICategoryRepository categoryRepo)
        {
            _blogRepo = blogRepo;
            _categoryRepo = categoryRepo;
        }
        public IActionResult Index(int? id, string q)
        {
            var query = _blogRepo.GetAll().Where(i => i.isApproved);
            if (id != null)
            {
                query = query.Where(i => i.CategoryId == id);
            }
            if (!string.IsNullOrEmpty(q))
            {
                //query = query.Where(i=>i.Title.Contains(q) || i.Description.Contains(q) || i.Body.Contains(q));
                query = query.Where(i => EF.Functions.Like(i.Title, "%" + q + "%") || EF.Functions.Like(i.Description, "%" + q + "%") || EF.Functions.Like(i.Body, "%" + q + "%"));
            }
            return View(query.OrderByDescending(i => i.Date));


        }
        public IActionResult List()
        {
            return View(_blogRepo.GetAll());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "CategoryId", "Name");
            return View(new Blog());
        }
        [HttpPost]
        public IActionResult Create(Blog entity)
        {
            entity.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                _blogRepo.AddBlog(entity);
                TempData["message"] = $"{entity.Title} kayıt edildi.";
                return RedirectToAction("List", "Blog");
            }
            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "CategoryId", "Name");
            return View(entity);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "CategoryId", "Name");
            return View(_blogRepo.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Blog entity, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        entity.Image = file.FileName;
                    }
                }
                _blogRepo.UpdateBlog(entity);
                TempData["message"] = $"{entity.Title} güncellendi.";
                return RedirectToAction("List", "Blog");
            }
            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "CategoryId", "Name");
            return View(entity);
        }
        //[HttpGet]
        //public IActionResult AddOrUpdate(int? id)
        //{
        //    ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "CategoryId", "Name");
        //    if (id == null)
        //    {
        //        return View(new Blog());
        //    }
        //    else
        //    {
        //        return View(_blogRepo.GetById((int)id));
        //    }
        //}
        //[HttpPost]
        //public IActionResult AddOrUpdate(Blog entity)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _blogRepo.SaveBlog(entity);
        //        TempData["message"] = $"{entity.Title} kayıt edildi.";
        //        return RedirectToAction("List");
        //    }
        //    ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "CategoryId", "Name");
        //    return View(entity);
        //}
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_blogRepo.GetById(id));
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int BlogId)
        {
            _blogRepo.DeleteBlog(BlogId);
            TempData["message"] = $"{BlogId} numaralı kayıt silindi.";
            return RedirectToAction("List");
        }
        public IActionResult Details(int id)
        {
            return View(_blogRepo.GetById(id));
        }
    }
}
