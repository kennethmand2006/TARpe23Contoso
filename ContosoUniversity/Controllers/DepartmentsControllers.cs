using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class DepartmentsControllers : Controller
    {
        public readonly SchoolContext _context;
        
        public DepartmentsControllers(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var schoolContex = _context.Departments.Include(d => d.Adminstrator);
            return View(await schoolContex.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string query = "SELECT * FROM Department WHERE DepartmentID = {0}";
            var department = await _context.Departments
                .FromSqlRaw(query, id)
                .Include(d => d.Adminstrator)
                .FirstOrDefaultAsync();
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        [HttpGet]

        public IActionResult Create()
        {
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Budget,StartDate,RowVersion,InstructorID,HonorStudent")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["InsturctorsID"] = new SelectList(_context.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }
        [HttpPost]
        public async Task<IActionResult> Make(int id)
        {
            var existingDepartment = await _context.Departments.FindAsync(id);
            if (existingDepartment == null)
            {
                return NotFound();
            }

            // BaseOn - loon uue osakonna olemasoleva põhjal
            var newDepartment = new Department
            {
                Name = existingDepartment.Name + " - New",
                Budget = existingDepartment.Budget,
                StartDate = DateTime.Now
                // Lisa veel omadusi kui vaja
            };

            _context.Departments.Add(newDepartment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Tagasi loetellu
        }
        [HttpPost]
        public async Task<IActionResult> MakeAndDeleteOld(int id)
        {
            var existingDepartment = await _context.Departments.FindAsync(id);
            if (existingDepartment == null)
            {
                return NotFound();
            }

            // Loo uus osakond olemasoleva põhjal
            var newDepartment = new Department
            {
                Name = existingDepartment.Name + " - New",
                Budget = existingDepartment.Budget,
                StartDate = DateTime.Now
                // Lisa muud omadused vastavalt vajadusele
            };

            _context.Departments.Add(newDepartment);

            // Kustuta vana osakond
            _context.Departments.Remove(existingDepartment);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Tagasi nimekirja
        }

    }
}
