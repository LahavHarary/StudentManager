using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Repositories
{
    public interface IStudentRepository
    {
        // Change later - need to add more gets and lost all the ID's

        Task<IEnumerable<Student>> Get();
        Task<Student> Get(int id);
        Task<Student> GetFirstName(string firstName);
        Task<Student> GetLastName(string lastName);
        Task<Student> GetAge(int age);
        Task<Student> getAverageGrade(double avg);
        Task<Student> getSchoolName(string SchoolName);
        Task<Student> getSchoolAddress(string SchoolAdress);
        Task<Student> Create(Student student);
        Task Update(Student student, int id);
        Task Delete();
        Task Delete(int id);

    }
}
