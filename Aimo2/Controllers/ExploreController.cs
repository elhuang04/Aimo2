using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aimo2.Models;
using Aimo2.Data;
using Microsoft.AspNetCore.Authorization;
using System.Drawing.Printing;
using System.Composition;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace Aimo2.Controllers
{
    public class ExploreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExploreController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: Explore
        /*[Authorize]
        public async Task<> Index()
        {
            return _context.Explore != null ?
                        View(await _context.Explore.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Explore'  is null.");
        }*/
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (_context.Explore == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Explore'  is null.");
            }
            var explore = from m in _context.Explore
                          select m;
            explore = explore.OrderByDescending(s => s.Due_Date!).ThenBy(s => s.Task_Title!);
            return View(explore);
        }
        [HttpPost]
        public ActionResult FilterbyDate(DateTime StartDate, DateTime EndDate, string searchText)
        {

            if (_context.Explore == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Explore'  is null.");
            }
            var explore = from m in _context.Explore
                          select m;

            var filteredtasks = explore.AsQueryable();
            if (!String.IsNullOrEmpty(searchText)) filteredtasks = filteredtasks.Where(k => k.Task_Title.ToLower().Contains(searchText.ToLower()));

           // if (!String.IsNullOrEmpty(StartDate.ToString()) && !String.IsNullOrEmpty(EndDate.ToString()))
            //{
                if (StartDate.ToString("yyyy-MM-dd") == "0001-01-01")
                {
                    if (EndDate.ToString("yyyy-MM-dd") != "0001-01-01")
                    filteredtasks = filteredtasks.Where(s => s.Due_Date.Date <= EndDate);
                    else
                    filteredtasks = filteredtasks.Where(s => s.Due_Date.Date != null);
                 }
                else
                {
                     if (EndDate.ToString("yyyy-MM-dd") != "0001-01-01")
                      filteredtasks = filteredtasks.Where(s => s.Due_Date.Date >= StartDate && s.Due_Date.Date <= EndDate);
                     else
                    filteredtasks = filteredtasks.Where(s => s.Due_Date.Date >= StartDate );
                 }
                          


           // }
               
            var result = filteredtasks.ToList(); // execute query
            
            
            if (!String.IsNullOrEmpty(searchText))
                {
                TempData["FilterbyTitle"] = searchText;
                //explore = explore.Where(k => k.Task_Title.ToLower().Contains(searchText.ToLower()));
            }
            if (!String.IsNullOrEmpty(StartDate.ToString()) && !String.IsNullOrEmpty(EndDate.ToString()))
            {
                //save input values to tempdata, similar to session param

                TempData["startdt"] = StartDate.ToString("yyyy-MM-dd");
                TempData["enddt"] = EndDate.ToString("yyyy-MM-dd");


                // explore = explore.Where(s => s.Due_Date.Date >= StartDate && s.Due_Date.Date <= EndDate);

            }
            //return View("Index", explore.AsNoTracking().ToListAsync());
            return View("Index", result);
            //return View();
        }



    //Due_Date section fitler
    [Authorize]
        /*public ActionResult Due_Date(string sortOrder)
        {
            if (ViewBag.DateSortParm = sortOrder == "Due_Date" ? "date_desc" : "Date";)
           else (ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Task_Title" : "";)
        }*/

            // GET: Explore/Details/5
            [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Explore == null)
            {
                return NotFound();
            }

            var explore = await _context.Explore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (explore == null)
            {
                return NotFound();
            }

            return View(explore);
        }

        // GET: Explore/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Explore/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,People_Needed,Requester,Task_Title,Due_Date,Status,Description,People_Claimed")] Explore explore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(explore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(explore);
        }

        // GET: Explore/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Explore == null)
            {
                return NotFound();
            }

            var explore = await _context.Explore.FindAsync(id);
            if (explore == null)
            {
                return NotFound();
            }
            return View(explore);
        }

        // POST: Explore/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,People_Needed,Requester,Task_Title,Due_Date,Status,Description,People_Claimed")] Explore explore)
        {
            if (id != explore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(explore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExploreExists(explore.Id))
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
            return View(explore);
        }


        //CLAIMCSIVMIVMS
        // GET: Explore/Edit/5
        [Authorize]
        public async Task<IActionResult> Claim(int? id)
        {
            if (id == null || _context.Explore == null)
            {
                return NotFound();
            }

            var explore = await _context.Explore.FindAsync(id);
            if (explore == null)
            {
                return NotFound();
            }
            ViewData["People_Claimed"] = explore.People_Claimed;
            
            return View(explore);
        }

        // POST: Explore/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Claim(int id, [Bind("Id,People_Needed,Requester,Task_Title,Due_Date,Status,Description,People_Claimed")] Explore explore)
        {
            if (id != explore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ViewData["People_Claimed"] = explore.People_Claimed;
                    _context.Update(explore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExploreExists(explore.Id))
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
            return View(explore);
        }




        // GET: Explore/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Explore == null)
            {
                return NotFound();
            }

            var explore = await _context.Explore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (explore == null)
            {
                return NotFound();
            }

            return View(explore);
        }



        // POST: Explore/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Explore == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Explore'  is null.");
            }
            var explore = await _context.Explore.FindAsync(id);
            if (explore != null)
            {
                _context.Explore.Remove(explore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExploreExists(int id)
        {
          return (_context.Explore?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
