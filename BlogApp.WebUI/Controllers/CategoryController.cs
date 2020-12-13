using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.WebUI.Controllers
{
    public class CategoryController : Controller
    {

        private ICategoryRepository repository;

        public CategoryController(ICategoryRepository repo)
        {
            repository = repo;
        }



        public IActionResult Index()
        {
            return View();
        }


        public IActionResult List()
        {
            return View(repository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Category entity)
        {
            if (ModelState.IsValid)
            {
                repository.AddCategory(entity);
                return RedirectToAction("List");
            }

            return View(entity);
        }

        [HttpGet]
        public IActionResult AddOrUpdate(int? id)
        {
            if (id == null)
            {
                ViewData["Title"] = "Ekleme";
                return View(new Category());
            }
            else
            {
                ViewData["Title"] = "Güncelleme";
                var category = repository.GetById(id.Value);

                return View(category);
            }
        }


        [HttpPost]
        public IActionResult AddOrUpdate(Category entity)
        {

            if (ModelState.IsValid)
            {
                repository.SaveCategory(entity);
                TempData["message"] = $"{entity.Name} kayıt edildi";

                return RedirectToAction("List");
            }

            return View(entity);
        }

    }
}