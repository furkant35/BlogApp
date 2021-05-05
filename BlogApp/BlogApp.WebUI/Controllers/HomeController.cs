using BlogApp.Data.Abstract;
using BlogApp.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IBlogRepository _blogRepo;
        private ICategoryRepository _categoryRepo;
        public HomeController(IBlogRepository blogRepo, ICategoryRepository categoryRepo)
        {
            _blogRepo = blogRepo;
            _categoryRepo = categoryRepo;
        }
        public IActionResult Index()
        {
            HomeBlogModel model = new HomeBlogModel();
            model.HomeBlogs = _blogRepo.GetAll().Where(i => i.isApproved && i.isHome).ToList();
            model.SliderBlogs = _blogRepo.GetAll().Where(i => i.isApproved && i.isSlider).ToList();
            return View(model);
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }

    }
}
