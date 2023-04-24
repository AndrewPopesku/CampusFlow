using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CampusFlow.Data;
using CampusFlow.Models;
using CampusFlow.ViewModels;

namespace CampusFlow.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly CampusContext _context;

        public ScheduleController(CampusContext context)
        {
            _context = context;
        }

        // GET: Schedule
        public async Task<IActionResult> Index()
        {
            ViewData["Days"] = ScheduleViewModel.Days;
            var schedules = _context.Schedules
                .Include(s => s.Subject)
                .OrderBy(s => s.DayOfWeek);
            var timeslots = await _context.TimeSlot.ToListAsync();
            var viewModelList = new List<ScheduleViewModel>();

            foreach (var item in timeslots)
            {
                viewModelList.Add(new ScheduleViewModel(await schedules.Where(s => s.TimeSlotId == item.TimeSlotId).ToListAsync(),
                    item));
            }
            return View(viewModelList);
        }

        // GET: Schedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var studentSchedule = await _context.Schedules
                .Include(s => s.Subject)
                .Include(s => s.TimeSlot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentSchedule == null)
            {
                return NotFound();
            }

            return View(studentSchedule);
        }

        // GET: Schedule/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Days"] = new SelectList(ScheduleViewModel.Days);
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name");
            ViewData["TimeSlot"] = new SelectList(_context.TimeSlot, "TimeSlotId", "ClassNumber");
            return View(); 
        }

        // POST: Schedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DayOfWeek,ClassType,SubjectId,TimeSlotId")] StudentSchedule studentSchedule)
        {
            ModelState.Remove("Subject");
            ModelState.Remove("TimeSlot");
            var schedules = _context.Schedules;
            if (schedules.Any(s => s.TimeSlotId == studentSchedule.TimeSlotId && s.DayOfWeek == studentSchedule.DayOfWeek))
            {
                ModelState.AddModelError("", "This slot is already occupied!");
            }
            else if (ModelState.IsValid)
            {
                _context.Add(studentSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Days"] = new SelectList(ScheduleViewModel.Days, studentSchedule.DayOfWeek);
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name", studentSchedule.SubjectId);
            ViewData["TimeSlot"] = new SelectList(_context.TimeSlot, "TimeSlotId", "ClassNumber", studentSchedule.TimeSlotId);
            return View(studentSchedule);
        }

        // GET: Schedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var studentSchedule = await _context.Schedules.FindAsync(id);
            if (studentSchedule == null)
            {
                return NotFound();
            }
            ViewData["Days"] = new SelectList(ScheduleViewModel.Days);
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name", studentSchedule.SubjectId);
            ViewData["TimeSlot"] = new SelectList(_context.TimeSlot, "TimeSlotId", "ClassNumber", studentSchedule.TimeSlotId);
            return View(studentSchedule);
        }

        // POST: Schedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DayOfWeek,ClassType,SubjectId,TimeSlotId")] StudentSchedule studentSchedule)
        {
            if (id != studentSchedule.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Subject");
            ModelState.Remove("TimeSlot");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentScheduleExists(studentSchedule.Id))
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
            ViewData["Days"] = new SelectList(ScheduleViewModel.Days, studentSchedule.DayOfWeek);
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name", studentSchedule.SubjectId);
            ViewData["TimeSlot"] = new SelectList(_context.TimeSlot, "TimeSlotId", "ClassNumber", studentSchedule.TimeSlotId);
            return View(studentSchedule);
        }

        // GET: Schedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var studentSchedule = await _context.Schedules
                .Include(s => s.Subject)
                .Include(s => s.TimeSlot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentSchedule == null)
            {
                return NotFound();
            }

            return View(studentSchedule);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Schedules == null)
            {
                return Problem("Entity set 'ScheduleContext.Schedules'  is null.");
            }
            var studentSchedule = await _context.Schedules.FindAsync(id);
            if (studentSchedule != null)
            {
                _context.Schedules.Remove(studentSchedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentScheduleExists(int id)
        {
          return (_context.Schedules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
