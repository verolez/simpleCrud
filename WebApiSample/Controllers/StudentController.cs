using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiSample.Models;

namespace WebApiSample.Controllers
{
    public class StudentController : ApiController
    {
        private StudentContext db = new StudentContext();

        public async Task<IHttpActionResult> GetStudentNames()
        {
            return Ok(await db.Students.ToListAsync());
        }

        public async Task<IHttpActionResult> DeleteStudent(int? id)
        {
            Student student = await db.Students.FindAsync(id);

            if (student == null)
                return NotFound();

            db.Students.Remove(student);
            await db.SaveChangesAsync();

            return Ok();
        }

        //public async Task<IHttpActionResult> UpdateStudent(StudentName student)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    db.Entry(student).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!StudentNameExists(student.ID))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }

        //    }

        //    return Ok();
        //}
        //public async Task<IHttpActionResult> InsertStudent(StudentName student)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    db.StudentNames.Add(student);
            
        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch(DbUpdateConcurrencyException)
        //    {
        //        if (!StudentNameExists(student.ID))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Ok();
        //}

        public async Task<IHttpActionResult> SaveStudent(Student student)
        {
            if(StudentNameExists(student.ID))
            {
                db.Entry(student).State = EntityState.Modified;
            }
            else
            {
                db.Students.Add(student);
            }

           try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (StudentNameExists(student.ID))
                {
                    return Conflict();
                }
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentNameExists(int id)
        {
            return db.Students.Count(e => e.ID == id) > 0;
        }
    }
}
