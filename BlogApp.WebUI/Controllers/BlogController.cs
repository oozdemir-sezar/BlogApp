using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository _blogRepository;
        private ICategoryRepository _categoryRepository;

        public BlogController(IBlogRepository blogRepo, ICategoryRepository categoryRepo)
        {
            _blogRepository = blogRepo;
            _categoryRepository = categoryRepo;
        }


        public IActionResult Index(int? id, string filter)
        {
            var blogs = _blogRepository.GetAll().Where(x => x.IsApproved);

            if (id != null)
            {
                blogs = blogs.Where(x => x.CategoryId == id);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                //blogs = blogs.Where(x => x.Title.Contains(filter) || x.Description.Contains(filter) || x.Body.Contains(filter));

                // ef core ile gelen özellik
                blogs = blogs.Where(x => 
                    EF.Functions.Like(x.Title, "%" + filter + "%") ||
                    EF.Functions.Like(x.Description, "%" + filter + "%") ||
                    EF.Functions.Like(x.Body, "%" + filter + "%"));
            }

            return View(blogs.OrderByDescending(x=> x.Date));
        }


        public IActionResult Details(int id)
        {
            return View(_blogRepository.GetById(id));
        }


        public IActionResult List()
        {
            return View(_blogRepository.GetAll());
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            //ViewData["Title"] = "Ekleme";

            return View(new Blog());
        }


        [HttpPost]
        public IActionResult Create(Blog entity)
        {
            if (ModelState.IsValid)
            {
                _blogRepository.SaveBlog(entity);
                TempData["message"] = $"{entity.Title} kayıt edildi";

                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");

            return View(_blogRepository.GetById(id));
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Blog entity, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if(file != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    entity.Image = file.FileName;
                }


                _blogRepository.SaveBlog(entity);

                TempData["message"] = $"{entity.Title} güncellendi";

                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);
        }


        [HttpGet]
        public IActionResult AddOrUpdate(int? id)
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");

            if (id == null)
            {
                ViewData["Title"] = "Ekleme";
                return View(new Blog());
            }
            else
            {
                ViewData["Title"] = "Güncelleme";
                var blog = _blogRepository.GetById(id.Value);

                return View(blog);
            }
        }


        [HttpPost]
        public IActionResult AddOrUpdate(Blog entity)
        {
            //ModelState.Remove("BlogId");

            if (ModelState.IsValid)
            {
                _blogRepository.SaveBlog(entity);
                TempData["message"] = $"{entity.Title} kayıt edildi";

                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_blogRepository.GetById(id));
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _blogRepository.DeleteBlog(id);
            TempData["message"] = $"{id} numaralı kayıt silindi";

            return RedirectToAction("List");
        }
    }
}