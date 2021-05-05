using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
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
            var category = _context.Categories.FirstOrDefault(p => p.CategoryId == categoryId);
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
            return _context.Categories.FirstOrDefault(p => p.CategoryId == categoryId);
        }

        public void SaveCategory(Category entity)
        {
            if (entity.CategoryId == 0)
            {
                _context.Categories.Add(entity);
            }
            else
            {
                var category = GetById(entity.CategoryId);
                if (category != null)
                {
                    category.Name = entity.Name;
                }
            }
            _context.SaveChanges();
        }

        public void UpdateCategory(Category entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
