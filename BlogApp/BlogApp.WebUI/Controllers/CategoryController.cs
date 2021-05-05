using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository _repository;
        public CategoryController(ICategoryRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View(_repository.GetAll());
        }
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Create(Category entity)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _repository.AddCategory(entity);
        //        return RedirectToAction("List", "Category");
        //    }
        //    return View(entity);
        //}
        [HttpGet]
        public IActionResult AddOrUpdate(int? id)
        {
            if (id == null)
            {
                return View(new Category());
            }
            else
            {
                return View(_repository.GetById((int)id));
            }
        }
        [HttpPost]
        public IActionResult AddOrUpdate(Category entity)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveCategory(entity);
                TempData["message"] = $"{entity.Name} kayıt edildi.";
                return RedirectToAction("List");
            }
            return View(entity);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_repository.GetById(id));
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int CategoryId)
        {
            _repository.DeleteCategory(CategoryId);
            TempData["message"] = $"{CategoryId} numaralı kayıt silindi.";
            return RedirectToAction("List");
        }
    }
}
