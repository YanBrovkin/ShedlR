using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShedlR.Domain.Models;

namespace ShedlR.Domain.DAL
{
    public class SRInitializer : CreateDatabaseIfNotExists<SRContext>
    {
        protected override void Seed(SRContext context)
        {
            var worktasks = new List<TaskItem> 
            { 
                new TaskItem { Customer = "Заказчик 1", Executor = "Исполнитель 1", Description = "принести чай", ExecutionTime = 1, RegisteredAt = DateTime.Now, Approved = false },
                new TaskItem { Customer = "Заказчик 1", Executor = "Исполнитель 1", Description = "принести кофе", ExecutionTime = 1, RegisteredAt = DateTime.Now, Approved = false },
                new TaskItem { Customer = "Заказчик 2", Executor = "Исполнитель 2", Description = "решить задачу", ExecutionTime = 2, RegisteredAt = DateTime.Now, Approved = false },
                new TaskItem { Customer = "Заказчик 2", Executor = "Исполнитель 2", Description = "дать ответ", ExecutionTime = 2, RegisteredAt = DateTime.Now, Approved = false },
                new TaskItem { Customer = "Заказчик 1", Executor = "Исполнитель 3", Description = "пойти домой", ExecutionTime = 1, RegisteredAt = DateTime.Now, Approved = false }
            };
            worktasks.ForEach(s => context.WorkTasks.Add(s));
            context.SaveChanges();
        }
    }
}
