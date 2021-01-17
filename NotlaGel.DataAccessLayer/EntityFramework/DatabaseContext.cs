using NotlaGel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace NotlaGel.DataAccessLayer.EntityFramework
{
    public class DatabaseContext:DbContext
    {
        public DbSet<NotlaGelUser> NotlaGelUsers { get; set; }
        public DbSet<Note> Nots { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }
    }
}
