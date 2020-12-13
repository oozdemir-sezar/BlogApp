using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private static object routeId = null;
        private ICategoryRepository _repository;

        public CategoryMenuViewComponent(ICategoryRepository repository)
        {
            _repository = repository;
        }


        public IViewComponentResult Invoke()
        {
            if (RouteData.Values["action"].ToString() == "Index")
            {
                routeId = RouteData?.Values["id"];
                ViewBag.SelectedCategory = routeId;
            }
            else
            {
                ViewBag.SelectedCategory = routeId;
            }

            return View(_repository.GetAll());
        }
    }
}
