using HavekrigerenApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.ViewModels
{
    public class NoteViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

        public NoteViewModel(Note note)
        {
            Id = note.Id;
            Title = note.Title;
            Content = note.Content;
            DateCreated = note.DateCreated;
        }
    }
}
