using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApp.Data.Concrete.EfCore
{
    public class BlogContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
    }
}
