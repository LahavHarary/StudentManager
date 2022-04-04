using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudentManager.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public StudentRepository(StudentContext context, IConnectionMultiplexer connectionMultiplexer)
        {
            _context = context;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<Student> Create(Student student)
        {
            // Add student to DB first
            if(0 <= student.Age && student.Age < 18 && student.AverageGrade <= 100 && student.AverageGrade >= 0)
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                // Add student to Redis
                var db = _connectionMultiplexer.GetDatabase();
                string jsonString = JsonSerializer.Serialize(student);
                await db.StringSetAsync(student.Id.ToString(), jsonString);

                return student;
            }

            return null;
        }
        
        public async Task<IEnumerable<Student>> Get()
        {
            /*
            Logic: In case the user asked for everything than it should come 
            back from the DB instead of redis.
            */
            return await _context.Students.ToListAsync();
        }
        
        public async Task<Student> Get(int id)
        {
            /*
            Logic: If something is inside Redis than DON'T call DB.
            Else: call db
             */

            var db = _connectionMultiplexer.GetDatabase();
            var rawData = await db.StringGetAsync(id.ToString());

            if (rawData.HasValue)
            {
                Student student = JsonSerializer.Deserialize<Student>(rawData);
                return student;
            }
            
            // look for something in the DB
            return await _context.Students.FindAsync(id);
            
        }

        public async Task<Student> GetFirstName(string firstName)
        {
            var student = _context.Students.FirstOrDefault(x => x.FirstName == firstName);
            return student;
        }

        public async Task<Student> GetLastName(string lastName)
        {
            var student = _context.Students.FirstOrDefault(x => x.LastName == lastName);
            return student;
        }

        public async Task<Student> GetAge(int age)
        {
            var student = _context.Students.FirstOrDefault(x => x.Age == age);
            return student;
        }

        public async Task<Student> getAverageGrade(double avg)
        {
            var student = _context.Students.FirstOrDefault(x => x.AverageGrade == avg);
            return student;
        }

        public async Task<Student> getSchoolName(string SchoolName)
        {
            var student = _context.Students.FirstOrDefault(x => x.SchoolName == SchoolName);
            return student;
        }

        public async Task<Student> getSchoolAddress(string SchoolAdress)
        {
            var student = _context.Students.FirstOrDefault(x => x.SchoolAddress == SchoolAdress);
            return student;
        }

        public async Task Update(Student student, int id)
        {
            try
            {
                var studentToDelete = await _context.Students.FindAsync(id);
                if (studentToDelete != null)
                {
                    // Delete from DB
                    _context.Students.Remove(studentToDelete);
                    await _context.SaveChangesAsync();

                    // Delete from Redis
                    var db = _connectionMultiplexer.GetDatabase();
                    await db.StringSetAsync(student.Id.ToString(), "");

                    student.Id = id;
                    Create(student);
                }
                
            }
            catch
            {

            }

            //_context.Entry(student).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
        }

        public async Task Delete()
        {
            //Delete from DB
             var all = from c in _context.Students select c;
             
            foreach(Student student in all)
            {
                Delete(student.Id);
            }

            
            //_context.Students.RemoveRange(all);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {

            var studentToDelete = await _context.Students.FindAsync(id);

            if (studentToDelete != null)
            {
                // Delete from DB
                _context.Students.Remove(studentToDelete);
                await _context.SaveChangesAsync();

                // Delete from Redis
                var db = _connectionMultiplexer.GetDatabase();
                await db.StringSetAsync(id.ToString(), "");
            }

        }
    }
}
