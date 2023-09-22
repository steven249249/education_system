using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using education_system.Dto;
using education_system.Interfaces;
using education_system.Models;

namespace education_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : Controller
    {
        
        private readonly ITeacherRepository _TeacherRepository;
        private readonly IMapper _mapper;

        public TeacherController(ITeacherRepository TeacherRepository, IMapper mapper)
        {
            _TeacherRepository = TeacherRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Teacher>))]
        public IActionResult GetTeachers()
        {
            var Teachers = _mapper.Map<List<TeacherDto>>(_TeacherRepository.GetTeachers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Teachers);
        }

        [HttpGet("{TeacherId}")]
        [ProducesResponseType(200, Type = typeof(Teacher))]
        [ProducesResponseType(400)]
        public IActionResult GetTeacher(int TeacherId)
        {
            if (!_TeacherRepository.TeacherExists(TeacherId))
                return NotFound();

            var Teacher = _mapper.Map<TeacherDto>(_TeacherRepository.GetTeacher(TeacherId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Teacher);
        }



        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTeacher([FromBody] TeacherDto TeacherCreate)
        {
            if (TeacherCreate == null)
                return BadRequest(ModelState);

            var Teacher = _TeacherRepository.GetTeachers()
                .Where(c => c.Name.Trim().ToUpper() == TeacherCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (Teacher != null)
            {
                ModelState.AddModelError("", "Teacher already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var TeacherMap = _mapper.Map<Teacher>(TeacherCreate);

            if (!_TeacherRepository.CreateTeacher(TeacherMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{TeacherId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTeacher(int TeacherId, [FromBody] TeacherDto updatedTeacher)
        {
            if (updatedTeacher == null)
                return BadRequest(ModelState);

            if (TeacherId != updatedTeacher.Id)
                return BadRequest(ModelState);

            if (!_TeacherRepository.TeacherExists(TeacherId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var TeacherMap = _mapper.Map<Teacher>(updatedTeacher);

            if (!_TeacherRepository.UpdateTeacher(TeacherMap))
            {
                ModelState.AddModelError("", "Something went wrong updating Teacher");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{TeacherId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTeacher(int TeacherId)
        {
            if (!_TeacherRepository.TeacherExists(TeacherId))
            {
                return NotFound();
            }

            var TeacherToDelete = _TeacherRepository.GetTeacher(TeacherId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_TeacherRepository.DeleteTeacher(TeacherToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting teacher");
            }

            return NoContent();
        }
    }
}
