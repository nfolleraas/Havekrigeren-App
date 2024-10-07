using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp
{
    public class Task
    {
        private List<TaskManager> tasks = new List<TaskManager>();
        public Task()
        {
            tasks.Add(new TaskManager("Jens Jensen", "Vejgade 1", 12345678, "Beplantning", "30/09-2024", "Stor farlig hund fdgfdg fdg adgdf gadfgadgad fgfdag adf"));
            tasks.Add(new TaskManager("Jørgen Jørgensen", "Vejgade 2", 12345678, "Træterrasse", "30/09-2024", "Anden stor farlig hund"));
        }
        public List<TaskManager> GetTasks(string category)
        {
            return tasks.Where(task => task.Category == category).ToList();
        }
    }
}
