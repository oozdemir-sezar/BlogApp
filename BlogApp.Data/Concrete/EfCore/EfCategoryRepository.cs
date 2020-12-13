using BlogApp.Data.Abstract;
using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfCategoryRepository : ICategoryRepository
    {
        private BlogContext _context;

        public EfCategoryRepository(BlogContext context)
        {
            _context = context;
        }


        public void AddCategory(Category entity)
        {
            _context.Categories.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteCategory(int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(x => x.CategoryId == categoryId);

            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }

        public Category GetById(int categoryId)
        {
            return _context.Categories.FirstOrDefault(x => x.CategoryId == categoryId);
        }

        public void UpdateCategory(Category entity)
        {
            //_context.Categories.Update(entity);
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void SaveCategory(Category entity)
        {
            if (entity.CategoryId == 0)
            {
                _context.Categories.Add(entity);
            }
            else
            {
                var blog = GetById(entity.CategoryId);

                if (blog != null)
                {
                    blog.Name = entity.Name;
                }
            }
            _context.SaveChanges();
        }
    }
}
