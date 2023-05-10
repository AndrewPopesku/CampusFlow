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

        // GET: Attendances
        public async Task<IActionResult> Index(int scheduleDateId)
        {
            if (!await _context.Attendances.AnyAsync(a => a.ScheduleDateId == scheduleDateId))
            {
                var students = await _context.Students.ToListAsync();
                var newAttends = new List<Attendance>();
                foreach (var student in students)
                {
                    newAttends.Add(new Attendance()
                    {
                        ScheduleDateId = scheduleDateId,
                        StudentId = student.Id,
                        IsPresent = true
                    });
                }

                await _context.AddRangeAsync(newAttends);
                await _context.SaveChangesAsync();
            }

            var attends = await _context.Attendances
                .Where(a => a.ScheduleDateId == scheduleDateId)
                .Include(a => a.Student)
                .ToListAsync();

            ViewData["Date"] = await _context.ScheduleDates
                .Where(sd => sd.Id == scheduleDateId)
                .Select(sd => sd.Date)
                .SingleOrDefaultAsync();

            return View(attends);
        }


        public async Task<IActionResult> UpdateAll(int scheduleDateId, Dictionary<int, bool> attendanceStatuses)
        {
            if (attendanceStatuses != null)
            {
                foreach (var attendanceStatus in attendanceStatuses)
                {
                    var attendance = await _context.Attendances.SingleOrDefaultAsync(a => a.StudentId == attendanceStatus.Key 
                                                && a.ScheduleDateId == scheduleDateId);
                    attendance.IsPresent = attendanceStatus.Value;
                    _context.Entry(attendance).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { scheduleDateId = scheduleDateId });
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
