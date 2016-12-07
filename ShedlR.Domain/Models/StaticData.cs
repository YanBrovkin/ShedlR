using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShedlR.Domain.Models
{
    public class StaticData
    {
        private static List<TaskItem> _table;

        public StaticData()
        {
            _table = new List<TaskItem>();
        }

        public List<TaskItem> Table { get { return _table; } set { _table = value; } }
    }
}