using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dal
{
    public class DataContext : DbContext
    {
        private string _connectString = "Server=THREE\\SQLEXPRESS;Database=ApartmentDB;User Id=sa;Password=sa;";
        public DataContext() : this("")
        {
        }
        public DataContext(string connectString)
        {
            _connectString = string.IsNullOrEmpty(connectString) ? _connectString : connectString;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer(_connectString);

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Cupboard> Cupboards { get; set; }
        public DbSet<Grid> Grids { get; set; }
        public DbSet<Thing> Things { get; set; }
       
    }
}
