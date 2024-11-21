namespace HavekrigerenApp.Models
{
    public class Category
    {
        private int index = 0;
        public int Id { get; set; }
        public string Name { get; set; }

        public Category(string name)
        {
            Id = index++;
            Name = name;
        }
    }
}
