using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShedlR.Domain.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Executor { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string Description { get; set; }
        public int ExecutionTime { get; set; }
        public bool Approved { get; set; }
    }
}