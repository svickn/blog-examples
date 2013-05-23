using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCKnockoutForChildCollections.Models
{
    public class TaskList
    {
        public string Title { get; set; }

        public List<TaskItem> Tasks { get; set; }
    }

    public class TaskItem
    {
        public string Title { get; set; }
    }
}