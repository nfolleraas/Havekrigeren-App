﻿using Google.Cloud.Firestore;

namespace HavekrigerenApp.Models
{
    [FirestoreData]
    public class Category
    {
        [FirestoreProperty]
        public int Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        // Parameterless contructor for Firebase cus they stupid
        public Category()
        {
        }
    }
}
