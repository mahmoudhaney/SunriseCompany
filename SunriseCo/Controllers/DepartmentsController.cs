using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using SunriseCo.Data;
using SunriseCo.Models;

namespace SunriseCo.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet] // Not Necessary - Get by Default
        public IActionResult GetIndexView()
        {
            return View("Index", _context.Departments);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_context.Departments);
        }

        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Department department = _context.Departments.Include(d => d.Employees).FirstOrDefault(Item => Item.Id == id);
            if (department == null)
                return NotFound();
            return View("Details", department);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Department department = _context.Departments.Include(d => d.Employees).FirstOrDefault(Item => Item.Id == id);
            if (department == null)
                return NotFound();
            return View(department);
        }

        [HttpGet]
        public IActionResult GetCreateView()
        {
            return View("Details");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken] // To Prevent CSRF attacks
        [HttpPost] // It's necessary to define the method
        // [Bind()] => prevents over-posting attacks
        public IActionResult AddDepartment([Bind("Id,Name,Description")] Department department)
        {
            if (ModelState.IsValid == true)
            {
                _context.Departments.Add(department);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                return View("Create");
            }

        }

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Department department = _context.Departments.FirstOrDefault(Item => Item.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            return View("Edit", department);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Department department = _context.Departments.FirstOrDefault(Item => Item.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [ValidateAntiForgeryToken] // To Prevent CSRF attacks
        [HttpPost] // It's necessary to define the method
        // [Bind()] => prevents over-posting attacks
        public IActionResult EditDepartment(int id, [Bind("Id,Name,Description")] Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _context.Departments.Update(department);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                return View("Edit");
            }
        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Department department = _context.Departments.FirstOrDefault(Item => Item.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            return View("Delete", department);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Department department = _context.Departments.FirstOrDefault(Item => Item.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost] // It's necessary to define the method
        public IActionResult DeleteDepartment(int id)
        {
            Department department = _context.Departments.FirstOrDefault(Item => Item.Id == id);
            _context.Departments.Remove(department);
            _context.SaveChanges();
            return RedirectToAction("GetIndexView");
        }
    }
}
