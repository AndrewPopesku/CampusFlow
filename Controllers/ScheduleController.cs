using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CampusFlow.Data;
using CampusFlow.Models;
using CampusFlow.ViewModels;
using CampusFlow.Extensions;
using System.Globalization;

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
        public async Task<IActionResult> Index(int? groupSelected, DateTime startDate, 
            string weekMove, WeekType weekTypeSelected = 0)
        {
            if (startDate == default(DateTime))
            {
                startDate = DateTime.Today.DateByWeekDay(DayOfWeek.Monday);
            }

            if (!string.IsNullOrEmpty(weekMove))
            {
                startDate = (weekMove.Equals("Forward")) ? startDate.AddDays(7) : startDate.AddDays(-7);
            }

            var endDate = startDate.AddDays(6);

            ViewData["CurrentDates"] = Enumerable.Range(0, (endDate - startDate).Days + 1)
                                                 .Select(d => startDate.AddDays(d))
                                                 .ToList();
            ViewData["Groups"] = new SelectList(_context.Groups, "Id", "Name", groupSelected);
            ViewData["SelectedWeekType"] = weekTypeSelected;
            ViewData["StartDate"] = startDate;

            var currentWeekSchedule = await _context.Schedules
                .Where(s => s.Group.Id == (groupSelected ?? 8) && s.WeekType == weekTypeSelected)
                .Include(s => s.ScheduleDates.Where(sd => sd.Date >= startDate && sd.Date <= endDate))
                .Include(s => s.TimeSlot)
                .Include(s => s.Class)
                    .ThenInclude(c => c.Teacher)
                .ToListAsync();

            foreach (var currentSchedule in currentWeekSchedule)
            {
                if (!currentSchedule.ScheduleDates.Any())
                {
                    var currentDate = startDate.AddDays((int) currentSchedule.DayOfWeek - 1);
                    var scheduleDate = new ScheduleDate()
                    {
                        ScheduleId = currentSchedule.Id,
                        Date = currentDate
                    };

                    await _context.AddAsync(scheduleDate);
                    await _context.SaveChangesAsync();

                    
                }
            }

            ViewData["TimeSlots"] = await _context.TimeSlot.ToListAsync();
            var currentWeekScheduleViewModel = currentWeekSchedule.Select(s => new ScheduleViewModel
            {
                ClassName = s.Class.Name,
                DayOfWeek = s.DayOfWeek,
                TimeSlot = s.TimeSlot,
                GroupId = s.GroupId,
                TeacherName = s.Class.Teacher.FullName,
                ClassType = s.Class.ClassType.ToString(),
                Location = s.Class.Location,
                ScheduleDateId = s.ScheduleDates.Select(sd => sd.Id).SingleOrDefault(),
            }).ToList();

            return View(currentWeekScheduleViewModel);
        }

        public async Task<IActionResult> EditableIndex(int? groupSelected, WeekType weekTypeSelected)
        {
            ViewData["Days"] = EditableScheduleViewModel.Days;
            ViewData["Groups"] = new SelectList(_context.Groups, "Id", "Name", groupSelected);
            ViewData["SelectedWeekType"] = weekTypeSelected;

            var schedules = _context.Schedules
                .Where(s => s.Group.Id == (groupSelected ?? 8) && s.WeekType == weekTypeSelected)
                .Include(s => s.TimeSlot)
                .Include(s => s.Class)
                .OrderBy(s => s.DayOfWeek);
            
            var timeslots = await _context.TimeSlot.ToListAsync();
            var viewModelList = new List<EditableScheduleViewModel>();

            foreach (var item in timeslots)
            {
                viewModelList.Add(new EditableScheduleViewModel(await schedules.Where(s => s.TimeSlotId == item.Id).ToListAsync(),
                    item));
            }
            return View(viewModelList);
        }


        // GET: Schedule/Create
        public IActionResult Create()
        {
            SetScheduleViewData();
            return View();
        }

        // POST: Schedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassId,TimeSlotId,DayOfWeek,WeekType,SemesterId,GroupId")] Schedule schedule)
        {
            SetScheduleViewData(schedule);

            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(schedule);
        }

        // GET: Schedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            SetScheduleViewData(schedule);
            return View(schedule);
        }

        // POST: Schedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassId,TimeSlotId,DayOfWeek,WeekType,SemesterId,GroupId")] Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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

            SetScheduleViewData(schedule);
            return View(schedule);
        }

        // GET: Schedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Class)
                .Include(s => s.Group)
                .Include(s => s.Semester)
                .Include(s => s.TimeSlot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Schedules == null)
            {
                return Problem("Entity set 'CampusContext.Schedules'  is null.");
            }
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
          return (_context.Schedules?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public void SetScheduleViewData(Schedule schedule = null)
        {
            ModelState.Remove("Class");
            ModelState.Remove("Group");
            ModelState.Remove("Semester");
            ModelState.Remove("TimeSlot");
            ModelState.Remove("ScheduleDates");

            if (schedule == null)
            {
                ViewData["Class"] = new SelectList(_context.Classes, "Id", "Name");
                ViewData["Group"] = new SelectList(_context.Groups, "Id", "Name");
                ViewData["Semester"] = new SelectList(_context.Semesters, "Id", "Name");
                ViewData["TimeSlot"] = new SelectList(_context.TimeSlot, "Id", "Id");
                return;
            }

            ViewData["Class"] = new SelectList(_context.Classes, "Id", "Name", schedule.ClassId);
            ViewData["Groud"] = new SelectList(_context.Groups, "Id", "Name", schedule.GroupId);
            ViewData["Semester"] = new SelectList(_context.Semesters, "Id", "Name", schedule.SemesterId);
            ViewData["TimeSlot"] = new SelectList(_context.TimeSlot, "Id", "Id", schedule.TimeSlotId);
        }
    }
}