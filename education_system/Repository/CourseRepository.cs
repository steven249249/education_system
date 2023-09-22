using education_system.Data;
using education_system.Interfaces;
using education_system.Models;

namespace education_system.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DataContext _context;
        public CourseRepository(DataContext context)
        {
            _context = context;
        }

        public bool CourseExists(int id)
        {
            return _context.Courses.Any(o => o.Id == id);
        }

        public bool CreateCourse(Course course)
        {
            _context.Add(course);
            return Save();
        }

        public bool DeleteCourse(Course course)
        {
            _context.Remove(course);
            return Save();
        }

        public Course GetCourse(int id)
        {
            return _context.Courses.Where(e => e.Id == id).FirstOrDefault();
        }

        public ICollection<Course> GetCourseByTeacher(int teacherId)
        {
            return _context.Courses.Where(p => p.Teacher.Id == teacherId).ToList();
        }
    

        public ICollection<Course> GetCourses()
        {
            return _context.Courses.ToList();
        }

        public Teacher GetTeacherOfACourse(int courseId)
        {
            return _context.Courses.Where(o => o.Id == courseId).Select(p => p.Teacher).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCourse(Course course)
        {
            _context.Update(course);
            return Save();
        }
    }
}
