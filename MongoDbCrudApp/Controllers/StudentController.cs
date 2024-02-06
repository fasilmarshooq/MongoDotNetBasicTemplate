using Microsoft.AspNetCore.Mvc;
using MongoDbCrudApp.Data;
using MongoDbCrudApp.Services;

namespace MongoDbCrudApp.Controllers
{
    [ApiController]
    public class StudentController(IStudentService studentService) : ControllerBase
    {
        private readonly IStudentService _studentService = studentService;

        [Route("Student/Add")]
        [HttpPost]
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

        [Route("Student/GetAll")]
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            var result = _studentService.GetAllStudents();
            return Ok(result);
        }
    }
}
