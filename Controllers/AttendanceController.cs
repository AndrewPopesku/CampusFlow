using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CampusFlow.Data;
using CampusFlow.Models;

namespace CampusFlow.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly CampusContext _context;

        public AttendancesController(CampusContext context)
        {
            _context = context;
        }

        // GET: Attendances
        public async Task<IActionResult> Index(int classId)
        {
            var attends = _context.Attendances
                .Where(a => a.ScheduleId == classId)
                .Include(a => a.Student)
                .Include(a => a.Schedule);
            
            if (!attends.Any())
            {
                var students = _context.Students;
                var newList = new List<Attendance>();
                foreach (var student in students)
                {
                    newList.Add(new Attendance() { ScheduleId = classId, StudentId = student.Id });
                }

                _context.AddRange(newList);
                await _context.SaveChangesAsync();
                attends = _context.Attendances
                    .Where(a => a.ScheduleId == classId)
                    .Include(a => a.Student)
                    .Include(a => a.Schedule);
            }

            ViewData["Class"] = attends.Select(a => a.Schedule).First();
            return View(attends);
        }

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ScheduleId,StudentId")] Attendance attendance)
        {
            ModelState.Remove("Class");
            ModelState.Remove("Student");
            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ScheduleId,StudentId")] Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Class");
            ModelState.Remove("Student");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendancesExists(attendance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Attendances == null)
            {
                return Problem("Entity set 'CampusContext.Attendances'  is null.");
            }
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendancesExists(int id)
        {
          return (_context.Attendances?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
