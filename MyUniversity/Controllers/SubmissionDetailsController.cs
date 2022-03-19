using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyUniversity.Data;
using MyUniversity.Models;

namespace MyUniversity.Controllers
{
    public class SubmissionDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubmissionDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubmissionDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SubmissionDetails.Include(s => s.Thesis);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SubmissionDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submissionDetail = await _context.SubmissionDetails
                .Include(s => s.Thesis)
                .FirstOrDefaultAsync(m => m.SubmissionId == id);
            if (submissionDetail == null)
            {
                return NotFound();
            }

            return View(submissionDetail);
        }

        // GET: SubmissionDetails/Create
        public IActionResult Create()
        {
            ViewData["ThesisId"] = new SelectList(_context.Theses, "ThesisId", "ThesisDescription");
            return View();
        }

        // POST: SubmissionDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubmissionId,ThesisId,SubmissionDesc,SubmissionDueOn,SubmissionOn,SubmissionFile,FileContentType,ReviewedBy,ReviewOn,Remarks,SubmissionStatus")] SubmissionDetail submissionDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(submissionDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ThesisId"] = new SelectList(_context.Theses, "ThesisId", "ThesisDescription", submissionDetail.ThesisId);
            return View(submissionDetail);
        }

        // GET: SubmissionDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submissionDetail = await _context.SubmissionDetails.FindAsync(id);
            if (submissionDetail == null)
            {
                return NotFound();
            }
            ViewData["ThesisId"] = new SelectList(_context.Theses, "ThesisId", "ThesisDescription", submissionDetail.ThesisId);
            return View(submissionDetail);
        }

        // POST: SubmissionDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubmissionId,ThesisId,SubmissionDesc,SubmissionDueOn,SubmissionOn,SubmissionFile,FileContentType,ReviewedBy,ReviewOn,Remarks,SubmissionStatus")] SubmissionDetail submissionDetail)
        {
            if (id != submissionDetail.SubmissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(submissionDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubmissionDetailExists(submissionDetail.SubmissionId))
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
            ViewData["ThesisId"] = new SelectList(_context.Theses, "ThesisId", "ThesisDescription", submissionDetail.ThesisId);
            return View(submissionDetail);
        }

        // GET: SubmissionDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submissionDetail = await _context.SubmissionDetails
                .Include(s => s.Thesis)
                .FirstOrDefaultAsync(m => m.SubmissionId == id);
            if (submissionDetail == null)
            {
                return NotFound();
            }

            return View(submissionDetail);
        }

        // POST: SubmissionDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var submissionDetail = await _context.SubmissionDetails.FindAsync(id);
            _context.SubmissionDetails.Remove(submissionDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubmissionDetailExists(int id)
        {
            return _context.SubmissionDetails.Any(e => e.SubmissionId == id);
        }
    }
}
