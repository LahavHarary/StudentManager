using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models;
using StudentManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            if (studentRepository is null)
            {
                throw new ArgumentNullException(nameof(studentRepository));
            }
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetStudents()
        {
            try
            {
                return Ok(await _studentRepository.Get());
            }catch(Exception e){
                // Optionally add logs
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetStudentsAccordingToId(int id)
        {
            try
            {
                return Ok(await _studentRepository.Get(id));
            }
            catch(Exception e)
            {
                // Optionally add logs
                return StatusCode(500);
            }
        }

        [HttpGet("firstname")]
        public async Task<ActionResult> GetStudentsAccordingToFirstName(string firstName)
        {
            try
            {
                return Ok(await _studentRepository.GetFirstName(firstName));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
         
        }

        [HttpGet("lastname")]
        public async Task<ActionResult> GetStudentsAccordingToLastName(string lastName)
        {
            try
            {
                return Ok(await _studentRepository.GetLastName(lastName));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }

        [HttpGet("age")]
        public async Task<ActionResult> GetStudentsAccordingToAge(int age)
        {
            try
            {
                return Ok(await _studentRepository.GetAge(age));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }

        [HttpGet("avg")]
        public async Task<ActionResult> GetStudentsAccordingToAvg(int avg)
        {
            try
            {
                return Ok(await _studentRepository.getAverageGrade(avg));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }

        [HttpGet("schoolname")]
        public async Task<ActionResult> GetStudentsAccordingToSchholName(string schoolName)
        {
            try
            {
                return Ok(await _studentRepository.getSchoolName(schoolName));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }

        [HttpGet("schooladdress")]
        public async Task<ActionResult> GetStudentsAccordingToSchholAddress(string schoolAddress)
        {
            try
            {
                return Ok(await _studentRepository.getSchoolAddress(schoolAddress));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }

        [HttpPost]
        public async Task<ActionResult> PostStudents([FromBody] Student student)
        {
            try
            {
                // For status 201 - Created response
                var newStudent = await _studentRepository.Create(student);
                if(newStudent == null)
                {
                    return StatusCode(400, "Student can not be created !\n" +
                        "Please check the parameters that you are posting.");    
                }

                return CreatedAtAction(nameof(GetStudents), new { id = newStudent.Id }, newStudent);
            }
            catch(Exception e)
            {
                // Optionally add logs
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutStudents(int id,[FromBody] Student student)
        {
            try
            {
                await _studentRepository.Update(student, id);

                return NoContent();
            }
            catch(Exception e)
            {
                // Optionally add logs
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var studentToDelete = await _studentRepository.Get(id);
                if (studentToDelete == null)
                {
                    return NotFound();
                }

                await _studentRepository.Delete(studentToDelete.Id);
                return NoContent();
            }
            catch (Exception e)
            {
                // Optionally add logs
                return StatusCode(500);
            }
        }


        [HttpDelete("")]
        public async Task<ActionResult> Delete()
        {
            try
            {
                await _studentRepository.Delete();
                return NoContent();
            }
            catch (Exception e)
            {
                // Optionally add logs
                return StatusCode(500);
            }
        }
    }
}
