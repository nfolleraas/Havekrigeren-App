using HavekrigerenApp.Exceptions;
using HavekrigerenApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.Persistance
{
    public static class NoteRepository
    {
        private static List<Note> _notes = new List<Note>();

        public static void Add(Note note)
        {
            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_InsertNote", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Title", note.Title);
                    command.Parameters.AddWithValue("@Content", note.Content);
                    command.Parameters.AddWithValue("@DateCreated", note.DateCreated);
                    note.Id = (int)command.ExecuteScalar();
                }
            }
            _notes.Add(note);
        }

        public static List<Note> GetAll()
        {
            _notes.Clear();

            List<Note> foundNotes = new List<Note>();
            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_SelectAllNotes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string title = (string)reader["Title"];
                        string content = (string)reader["Content"];
                        DateTime dateCreated = (DateTime)reader["DateCreated"];

                        Note note = new Note(title, content)
                        {
                            Id = id,
                            DateCreated = dateCreated,
                        };

                        foundNotes.Add(note);
                        _notes.Add(note);
                    }
                }
            }
            foundNotes = foundNotes
                .OrderByDescending(note => note.DateCreated)
                .ToList();

            return foundNotes;
        }

        public static Note Get(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("The parameter id cannot be less than 0");
            }

            Note? foundNote = null;

            foreach (Note note in _notes)
            {
                if (note.Id == id)
                {
                    foundNote = note;
                }
            }

            if (foundNote == null)
            {
                throw new NotFoundException($"Category with the id '{id}' was not found in the database.");
            }

            return foundNote;
        }

        public static void Delete(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("The parameter id cannot be less than 0");
            }

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_DeleteNote", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
            List<Note> copy = new List<Note>(_notes);
            foreach (Note note in copy)
            {
                if (note.Id == id)
                {
                    _notes.Remove(note);
                }
            }
        }
    }
}
