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
    public class AttendanceController : Controller
    {
        private readonly CampusContext _context;

        public AttendanceController(CampusContext context)
        {
            _context = context;
        }

        // GET: Attendance
        public async Task<IActionResult> Index(int classId)
        {
            var attends = _context.Attendances
                .Where(a => a.ClassId == classId)
                .Include(a => a.Student)
                .Include(a => a.ClassCycle);
            
            if (!attends.Any())
            {
                var students = _context.Students;
                var newList = new List<Attendance>();
                foreach (var student in students)
                {
                    newList.Add(new Attendance() { ClassId = classId, StudentId = student.Id });
                }

                _context.AddRange(newList);
                await _context.SaveChangesAsync();
                attends = _context.Attendances
                    .Where(a => a.ClassId == classId)
                    .Include(a => a.Student)
                    .Include(a => a.ClassCycle);
            }

            ViewData["Class"] = attends.Select(a => a.ClassCycle).First();
            return View(attends);
        }

        // GET: Attendance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Attendance == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendance/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attendance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassId,StudentId")] Attendance attendance)
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

        // GET: Attendance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Attendance == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }

        // POST: Attendance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassId,StudentId")] Attendance attendance)
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
                    if (!AttendanceExists(attendance.Id))
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

        // GET: Attendance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Attendance == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Attendance == null)
            {
                return Problem("Entity set 'CampusContext.Attendance'  is null.");
            }
            var attendance = await _context.Attendance.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendance.Remove(attendance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(int id)
        {
          return (_context.Attendance?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
