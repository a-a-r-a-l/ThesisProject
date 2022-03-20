using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyUniversity.Data;
using MyUniversity.Models;

namespace MyUniversity.Controllers
{
    public class SubmissionDetailsController : Controller
    {
        private const string BlobContainerNAME = "projectdocuments";

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<SubmissionDetailsController> _logger;

        public SubmissionDetailsController(
            ApplicationDbContext context,
            IConfiguration config,
            ILogger<SubmissionDetailsController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
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
        private async Task<string> SaveFileToBlobAsync(IFormFile projectDocuments)
        {
            string storageConnection1 = _config.GetValue<string>("MyAzureSettings:StorageAccountKey1");
            string storageConnection2 = _config.GetValue<string>("MyAzureSettings:StorageAccountKey2");
            string fileName = projectDocuments.FileName;
            string tempFilePath = string.Empty;
            string photoUrl;

            if (projectDocuments != null && projectDocuments.Length > 0)
            {
                // Save the uploaded file on to a TEMP file.
                tempFilePath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(tempFilePath))
                {
                    projectDocuments.CopyToAsync(stream).Wait();
                }
            }
            // Get a reference to a container 
            BlobContainerClient blobContainerClient = new BlobContainerClient(storageConnection1, BlobContainerNAME);

            // Create the container if it does not exist - granting PUBLIC access.
            await blobContainerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);

            // Create the client to the Blob Item
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            // Open the file and upload its data
            using (FileStream uploadFileStream = System.IO.File.OpenRead(tempFilePath))
            {
                await blobClient.UploadAsync(uploadFileStream, overwrite: true);
                uploadFileStream.Close();
            }

            // Delete the TEMP file since it is no longer needed.
            System.IO.File.Delete(tempFilePath);

            // Return the URI of the item in the Blob Storage
            photoUrl = blobClient.Uri.ToString();
            return photoUrl;
        }
    }
}
