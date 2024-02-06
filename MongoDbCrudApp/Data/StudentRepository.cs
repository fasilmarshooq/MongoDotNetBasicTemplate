using Agoda.IoC.Core;

namespace MongoDbCrudApp.Data
{
    public interface IStudentRepository
    {
        void AddStudent(Student student);
        void DeleteStudent(int id);
        Student? GetStudent(int id);
        List<Student> GetStudents();
    }
    [RegisterPerRequest]
    public class StudentRepository(Repository repository) : IStudentRepository
    {
        private readonly Repository _repository = repository;

        public void AddStudent(Student student)
        {
            _repository.Students.Add(student);
            _repository.SaveChanges();
        }

        public void DeleteStudent(int id)
        {
            var student = _repository.Students.Find(id);
            _repository.Students.Remove(student);
            _repository.SaveChanges();
        }

        public Student? GetStudent(int id)
        {
            return _repository.Students.FirstOrDefault(s => s.Id == id);
        }

        public List<Student> GetStudents()
        {
            return _repository.Students.ToList();
        }
    }
}
