using AutoMapper;
using education_system.Dto;
using education_system.Interfaces;
using education_system.Models;
using education_system.Repository;
using Microsoft.AspNetCore.Mvc;

namespace education_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly ICourseRepository _CourseRepository;
        private readonly ITeacherRepository _TeacherRepository ;
        private readonly IMapper _mapper;

        public CourseController(ICourseRepository CourseRepository,
            ITeacherRepository TeacherRepository,
            IMapper mapper)
        {
            _CourseRepository = CourseRepository;
            _TeacherRepository = TeacherRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Course>))]
        public IActionResult GetCourses()
        {
            var Courses = _mapper.Map<List<CourseDto>>(_CourseRepository.GetCourses());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Courses);
        }

        [HttpGet("get/{CourseId}")]
        [ProducesResponseType(200, Type = typeof(Course))]
        [ProducesResponseType(400)]
        public IActionResult GetCourse(int CourseId)
        {
            if (!_CourseRepository.CourseExists(CourseId))
                return NotFound();

            var Course = _mapper.Map<CourseDto>(_CourseRepository.GetCourse(CourseId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Course);
        }

        [HttpGet("{teacherId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Course>))]
        [ProducesResponseType(400)]
        public IActionResult GetCourseByTeacher(int teacherId)
        {
            if (!_TeacherRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }

            var courses = _mapper.Map<List<CourseDto>>(
                _CourseRepository.GetCourseByTeacher(teacherId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(courses);
        }

        [HttpGet("teacher/{courseId}")]
        [ProducesResponseType(200, Type = typeof(Teacher))]
        [ProducesResponseType(400)]
        public IActionResult GetTeacherOfACourse(int courseId)
        {
            if (!_CourseRepository.CourseExists(courseId))
            {
                return NotFound();
            }


            var courses = _mapper.Map<TeacherDto>(
                _CourseRepository.GetTeacherOfACourse(courseId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(courses);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCourse([FromQuery] int teacherId,[FromBody] CourseDto CourseCreate)
        {
            if (CourseCreate == null)
                return BadRequest(ModelState);

            var Course = _CourseRepository.GetCourses()
                .Where(c => c.Name.Trim().ToUpper() == CourseCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (Course != null)
            {
                ModelState.AddModelError("", "Course already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var CourseMap = _mapper.Map<Course>(CourseCreate);
            CourseMap.Teacher = _TeacherRepository.GetTeacher(teacherId);
            if (!_CourseRepository.CreateCourse(CourseMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{CourseId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCourse(int CourseId, [FromBody] CourseDto updatedCourse)
        {
            if (updatedCourse == null)
                return BadRequest(ModelState);

            if (CourseId != updatedCourse.Id)
                return BadRequest(ModelState);

            if (!_CourseRepository.CourseExists(CourseId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var CourseMap = _mapper.Map<Course>(updatedCourse);

            if (!_CourseRepository.UpdateCourse(CourseMap))
            {
                ModelState.AddModelError("", "Something went wrong updating Course");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{CourseId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCourse(int CourseId)
        {
            if (!_CourseRepository.CourseExists(CourseId))
            {
                return NotFound();
            }

            var CourseToDelete = _CourseRepository.GetCourse(CourseId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_CourseRepository.DeleteCourse(CourseToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Course");
            }

            return NoContent();
        }
    }
}
