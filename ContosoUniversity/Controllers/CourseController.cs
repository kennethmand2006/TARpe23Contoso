using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;

        public CoursesController(SchoolContext context)
        {
            _context = context;
        }

        // Index meetod
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.ToListAsync());
        }

        // Details ja Delete - kasutavad sama vaadet
        public async Task<IActionResult> DetailsDelete(int? id, string action)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseID == id);
            if (course == null) return NotFound();

            if (action == "Details")
            {
                ViewBag.ShowCloneButton = true;
                ViewBag.ActionType = "Details";
            }
            else if (action == "Delete")
            {
                ViewBag.ShowCloneButton = false;
                ViewBag.ActionType = "Delete";
            }

            return View("DetailsDelete", course);
        }

        // Create meetod
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // Edit meetod
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            return View("CreateEdit", course);  // Kasutame sama vaadet mis Create jaoks
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.CourseID) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("CreateEdit", course);
        }

        // Delete meetod (kustutamine)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Clone meetod
        public async Task<IActionResult> Clone(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            var clonedCourse = new Course
            {
                Title = course.Title,
                Credits = course.Credits,
                Description = course.Description
            };

            _context.Courses.Add(clonedCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

}
