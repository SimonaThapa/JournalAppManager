namespace SimsAppJournal.Models
{
    public class Category
    {
        public string Name { get; set; }

        public Category(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

