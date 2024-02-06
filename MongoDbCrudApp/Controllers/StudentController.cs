using Microsoft.AspNetCore.Mvc;
using MongoDbCrudApp.Data;
using MongoDbCrudApp.Services;

namespace MongoDbCrudApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost(Name = "AddStudent")]
        public ActionResult AddStudent(Student student)
        {
            try
            {
                _studentService.AddStudent(student);
                return Ok("Successfully added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet(Name = "GetAllStudents")]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            var result = _studentService.GetAllStudents();
            return Ok(result);
        }
    }
}
