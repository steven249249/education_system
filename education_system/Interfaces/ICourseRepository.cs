using education_system.Models;


namespace education_system.Interfaces
{
    public interface ICourseRepository
    {
        ICollection<Course> GetCourses();
        Course GetCourse(int id);
        Teacher GetTeacherOfACourse(int courseId);
        ICollection<Course> GetCourseByTeacher(int teacherId);
        bool CourseExists(int id);
        bool CreateCourse(Course course);
        bool UpdateCourse(Course course);
        bool DeleteCourse(Course course);
        bool Save();
    }
}
