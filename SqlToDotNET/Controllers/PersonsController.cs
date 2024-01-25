using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SqlToDotNET.Models;

namespace SqlToDotNET.Controllers
{
    public class PersonsController : Controller
    {
        private readonly PersonDbContext _context;

        public PersonsController(PersonDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.People.FromSqlRaw("EXECUTE spGetAllPerson").ToListAsync());
        }
        
        
        public async Task<IActionResult> Backup()
        {
            return View(await _context.TblBackups.FromSqlRaw("EXECUTE spGetAllBackup").ToListAsync());
        }
        
        public async Task<IActionResult> Audit()
        {
            return View(await _context.PersonAudits.FromSqlRaw("EXECUTE spGetAllPersonAudit").ToListAsync());
        }


        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _context.People.FromSqlRaw("EXECUTE spGetPersonById @id", new SqlParameter("@id", id)).ToList().FirstOrDefault();

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,Name,DateOfBirth")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlRaw("EXECUTE spCreatePerson @name, @dateOfBirth",
                     new SqlParameter("@name", person.Name),
                     new SqlParameter("@dateOfBirth", person.DateOfBirth));

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,Name,DateOfBirth")] Person person)
        {
            if (id != person.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlRaw("EXECUTE spUpdatePerson @id, @name, @dateOfBirth",
                    new SqlParameter("@id", id),
                    new SqlParameter("@name", person.Name),
                     new SqlParameter("@dateOfBirth", person.DateOfBirth));
                await _context.SaveChangesAsync();
                
                
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.Database.ExecuteSqlRaw("EXECUTE spDeletePerson @id", new SqlParameter("@id", id));
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.PersonId == id);
        }
    }
}
