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
    public class ClassCycleController : Controller
    {
        private readonly CampusContext _context;

        public ClassCycleController(CampusContext context)
        {
            _context = context;
        }

        // GET: ClassCycle
        public async Task<IActionResult> Index()
        {
            var campusContext = _context.ClassCycle.Include(c => c.Class).Include(c => c.Semester);
            return View(await campusContext.ToListAsync());
        }

        // GET: ClassCycle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClassCycle == null)
            {
                return NotFound();
            }

            var classCycle = await _context.ClassCycle
                .Include(c => c.Class)
                .Include(c => c.Semester)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classCycle == null)
            {
                return NotFound();
            }

            return View(classCycle);
        }

        // GET: ClassCycle/Create
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Id");
            ViewData["SemesterId"] = new SelectList(_context.Set<Semester>(), "Id", "Id");
            return View();
        }

        // POST: ClassCycle/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,ClassId,SemesterId")] ClassCycle classCycle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classCycle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Id", classCycle.ClassId);
            ViewData["SemesterId"] = new SelectList(_context.Set<Semester>(), "Id", "Id", classCycle.SemesterId);
            return View(classCycle);
        }

        // GET: ClassCycle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClassCycle == null)
            {
                return NotFound();
            }

            var classCycle = await _context.ClassCycle.FindAsync(id);
            if (classCycle == null)
            {
                return NotFound();
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Id", classCycle.ClassId);
            ViewData["SemesterId"] = new SelectList(_context.Set<Semester>(), "Id", "Id", classCycle.SemesterId);
            return View(classCycle);
        }

        // POST: ClassCycle/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,ClassId,SemesterId")] ClassCycle classCycle)
        {
            if (id != classCycle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classCycle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassCycleExists(classCycle.Id))
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
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Id", classCycle.ClassId);
            ViewData["SemesterId"] = new SelectList(_context.Set<Semester>(), "Id", "Id", classCycle.SemesterId);
            return View(classCycle);
        }

        // GET: ClassCycle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClassCycle == null)
            {
                return NotFound();
            }

            var classCycle = await _context.ClassCycle
                .Include(c => c.Class)
                .Include(c => c.Semester)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classCycle == null)
            {
                return NotFound();
            }

            return View(classCycle);
        }

        // POST: ClassCycle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClassCycle == null)
            {
                return Problem("Entity set 'CampusContext.ClassCycle'  is null.");
            }
            var classCycle = await _context.ClassCycle.FindAsync(id);
            if (classCycle != null)
            {
                _context.ClassCycle.Remove(classCycle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassCycleExists(int id)
        {
          return (_context.ClassCycle?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
