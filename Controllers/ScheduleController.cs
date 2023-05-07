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
        public async Task<IActionResult> Index(string weektype = "Odd", int groupSelected = 8)
        {
            var classes = _context.Classes
                .Where(s => s.GroupId == groupSelected 
                    && s.WeekType == Enum.Parse<WeekType>(weektype))
                .Include(s => s.Teacher)
                .Include(s => s.Subject)
                .OrderBy(s => s.DayOfWeek);

            var timeslots = await _context.TimeSlot.ToListAsync();
            var viewModelList = new List<ScheduleViewModel>();

            foreach (var item in timeslots)
            {
                viewModelList.Add(new ScheduleViewModel(await classes.Where(s => s.TimeSlotId == item.TimeSlotId).ToListAsync(),
                    item));
            }

            ViewData["Days"] = ScheduleViewModel.Days;
            ViewData["Group"] = new SelectList(_context.Groups, "Id", "Name", groupSelected);
            ViewData["WeekType"] = weektype;

            return View(viewModelList);
        }

        // GET: Schedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var studyClass = await _context.Classes
                .Include(s => s.Group)
                .Include(s => s.Teacher)
                .Include(s => s.Subject)
                .Include(s => s.TimeSlot)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (studyClass == null)
            {
                return NotFound();
            }

            return View(studyClass);
        }

        // GET: Schedule/Create
        public async Task<IActionResult> Create()
        {
            GetScheduleViewData();
            return View();
        }

        // POST: Schedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DayOfWeek,ClassType,WeekType,Location,TeacherId,GroupId,SubjectId,TimeSlotId")] Class studyClass)
        {
            ModelState.Remove("Subject");
            ModelState.Remove("TimeSlot");
            ModelState.Remove("Teacher");
            ModelState.Remove("Group");
            var classes = _context.Classes
                .Include(s => s.Group);
            
            if (classes.Any(s => s.TimeSlotId == studyClass.TimeSlotId
                && s.DayOfWeek == studyClass.DayOfWeek
                && s.TeacherId == studyClass.TeacherId
                && s.WeekType == studyClass.WeekType))
            {
                ModelState.AddModelError("", "This slot is already reserved!"
                                        + "\nPlease, try again");
            }

            if (ModelState.IsValid)
            {
                _context.Add(studyClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            GetScheduleViewData(studyClass);
            return View(studyClass);
        }

        // GET: Schedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var studyClass = await _context.Classes.FindAsync(id);
            if (studyClass == null)
            {
                return NotFound();
            }

            GetScheduleViewData(studyClass);
            return View(studyClass);
        }

        // POST: Schedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DayOfWeek,ClassType,WeekType,Location,TeacherId,GroupId,SubjectId,TimeSlotId")] Class studyClass)
        {
            if (id != studyClass.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Subject");
            ModelState.Remove("TimeSlot");
            ModelState.Remove("Teacher");
            ModelState.Remove("Group");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studyClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(studyClass.Id))
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

            GetScheduleViewData(studyClass);
            return View(studyClass);
        }

        // GET: Schedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var studyClass = await _context.Classes
                .Include(s => s.Group)
                .Include(s => s.Teacher)
                .Include(s => s.Subject)
                .Include(s => s.TimeSlot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studyClass == null)
            {
                return NotFound();
            }

            return View(studyClass);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Classes == null)
            {
                return Problem("Entity set 'ScheduleContext.Classes'  is null.");
            }
            var studyClass = await _context.Classes.FindAsync(id);
            if (studyClass != null)
            {
                _context.Classes.Remove(studyClass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(int id)
        {
            return (_context.Classes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void GetScheduleViewData(Class studyClass = null)
        {
            if (studyClass is null)
            {
                ViewData["Days"] = new SelectList(ScheduleViewModel.Days);
                ViewData["Teacher"] = new SelectList(_context.Teachers, "Id", "FullName");
                ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name");
                ViewData["Group"] = new SelectList(_context.Groups, "Id", "Name");
                ViewData["TimeSlot"] = new SelectList(_context.TimeSlot, "TimeSlotId", "ClassNumber");
                return;
            }

            ViewData["Days"] = new SelectList(ScheduleViewModel.Days, studyClass.DayOfWeek);
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name", studyClass.SubjectId);
            ViewData["Teacher"] = new SelectList(_context.Teachers, "Id", "FullName", studyClass.TeacherId);
            ViewData["Group"] = new SelectList(_context.Groups, "Id", "Name", studyClass.GroupId);
            ViewData["TimeSlot"] = new SelectList(_context.TimeSlot, "TimeSlotId", "ClassNumber", studyClass.TimeSlotId);
        }
    }
}
