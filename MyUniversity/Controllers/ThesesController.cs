using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyUniversity.Data;
using MyUniversity.Models;

namespace MyUniversity.Controllers
{
    [Authorize(Roles ="Student")]
    public class ThesesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThesesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Theses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Theses.Include(t => t.Student).Include(t => t.Subject);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Theses/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thesis = await _context.Theses
                .Include(t => t.Student)
                .Include(t => t.Subject)
                .FirstOrDefaultAsync(m => m.ThesisId == id);
            if (thesis == null)
            {
                return NotFound();
            }

            return View(thesis);
        }

        // GET: Theses/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Students, "UserId", "UserId");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName");
            return View();
        }

        // POST: Theses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ThesisId,ThesisName,ThesisDescription,SubjectId,StudentId,StartDate,EndDate,CompletionPercent")] Thesis thesis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thesis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "UserId", "UserId", thesis.StudentId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", thesis.SubjectId);
            return View(thesis);
        }

        // GET: Theses/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thesis = await _context.Theses.FindAsync(id);
            if (thesis == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "UserId", "UserId", thesis.StudentId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", thesis.SubjectId);
            return View(thesis);
        }

        // POST: Theses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("ThesisId,ThesisName,ThesisDescription,SubjectId,StudentId,StartDate,EndDate,CompletionPercent")] Thesis thesis)
        {
            if (id != thesis.ThesisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thesis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThesisExists(thesis.ThesisId))
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
            ViewData["StudentId"] = new SelectList(_context.Students, "UserId", "UserId", thesis.StudentId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectName", thesis.SubjectId);
            return View(thesis);
        }

        // GET: Theses/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thesis = await _context.Theses
                .Include(t => t.Student)
                .Include(t => t.Subject)
                .FirstOrDefaultAsync(m => m.ThesisId == id);
            if (thesis == null)
            {
                return NotFound();
            }

            return View(thesis);
        }

        // POST: Theses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var thesis = await _context.Theses.FindAsync(id);
            _context.Theses.Remove(thesis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThesisExists(short id)
        {
            return _context.Theses.Any(e => e.ThesisId == id);
        }
    }
}
