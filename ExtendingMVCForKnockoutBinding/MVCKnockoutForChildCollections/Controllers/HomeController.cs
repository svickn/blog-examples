using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCKnockoutForChildCollections.Models;

namespace MVCKnockoutForChildCollections.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var list = new TaskList
                {
                    Title = "My Task List",
                    Tasks = new List<TaskItem> {
                        new TaskItem {Title = "My First Task"},
                        new TaskItem {Title = "My Second Task"}
                    }
                };

            return View(list);
        }

        [HttpPost]
        public ActionResult Index(TaskList list)
        {
            // If you debug the application, the list should 
            // show the tasks as modified in the knockout collection.
            return View(list);
        }

    }
}
