namespace ContactManager.Models
{
    public class Category
    {
        public Category()
        {
            Contacts = new HashSet<Contact>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
