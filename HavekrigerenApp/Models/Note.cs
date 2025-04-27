using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

        public Note(string title, string content)
        {
            Title = title;
            Content = content;
            DateCreated = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Title}, {Content}, {DateCreated}";
        }
    }
}
