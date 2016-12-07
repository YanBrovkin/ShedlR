using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ShedlR.Domain.Models;

namespace ShedlR.Domain.DAL
{
    public class SRContext: DbContext
    {
        // Задачи
        public DbSet<TaskItem> WorkTasks { get; set; }
        /// <summary>
        /// Используем Fluent-Configuration при настройке БД
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
