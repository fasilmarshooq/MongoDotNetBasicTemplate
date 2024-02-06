using Agoda.IoC.Core;
using MongoDbCrudApp.Data;

namespace MongoDbCrudApp.Services
{
    public interface IStudentService
    {
        void AddStudent(Student student);
        IEnumerable<Student> GetAllStudents();
    }
    [RegisterPerRequest]
    public class StudentService(IStudentRepository studentRepository) : IStudentService
    {
        private readonly IStudentRepository _studentRepository = studentRepository;

        public void AddStudent(Student student)
        {
            _studentRepository.AddStudent(student);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _studentRepository.GetStudents();
        }
    }
}
