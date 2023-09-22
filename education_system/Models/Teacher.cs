namespace education_system.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Department { get; set; }
        public ICollection<Course> Course { get; set; }
    }
}
