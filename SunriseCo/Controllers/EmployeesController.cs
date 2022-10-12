using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunriseCo.Data;
using SunriseCo.Models;

namespace SunriseCo.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public string GreetVisitor()
        {
            return "Welcome to sunrise for software solution";
        }

        public string GreetUser(string name)
        {
            return "Hi "+ name + "!";
        }

        public double GetPrice (double cost, double profitRatio)
        {
            return cost + cost * profitRatio;
        }

        [Route("[controller]/List")]
        [HttpGet] // Not Necessary - Get by Default
        public IActionResult GetIndexView(string? search, string? sortType, string? sortOrder, int pageSize = 2, int pageNumber = 1)
        {
            //List<Employee> emps = new List<Employee> ();
            IQueryable<Employee> emps = _context.Employees.AsQueryable();

            if(!string.IsNullOrWhiteSpace(search))
            {
                //emps = _context.Employees.ToList ();
                search = search.Trim();
                emps = _context.Employees.Where(e => e.FullName.Contains(search));
            }
            //else
            //{
            //    search = search.Trim();
            //    emps = _context.Employees.Where(e => e.FullName.Contains(search)).ToList();
                
            //}

            if(!string.IsNullOrWhiteSpace(sortType) && !string.IsNullOrWhiteSpace(sortOrder))
            {
                if(sortType == "FullName")
                {
                    if (sortOrder == "asc")
                        emps = emps.OrderBy(e => e.FullName);
                    else if(sortOrder == "desc")
                        emps = emps.OrderByDescending(e => e.FullName);
                }

                else if (sortType == "Position")
                {
                    if (sortOrder == "asc")
                        emps = emps.OrderBy(e => e.Position);
                    else if (sortOrder == "desc")
                        emps = emps.OrderByDescending(e => e.Position);
                }

                else if (sortType == "Salary")
                {
                    if (sortOrder == "asc")
                        emps = emps.OrderBy(e => e.Salary);
                    else if (sortOrder == "desc")
                        emps = emps.OrderByDescending(e => e.Salary);
                }
            }

            ViewBag.CurrentSearch = search;

            emps = emps.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            ViewBag.pageSize = pageSize;
            ViewBag.pageNumber = pageNumber;

            return View("Index", emps.ToList());
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_context.Employees);
        }

        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Employee employee = _context.Employees.Include(e => e.Department).FirstOrDefault(Item => Item.Id == id);
            if(employee == null)
                return NotFound();
            return View("Details", employee);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Employee employee = _context.Employees.Include(e => e.Department).FirstOrDefault(Item => Item.Id == id);
            if (employee == null)
                return NotFound();
            return View(employee);
        }

        [HttpGet]
        public IActionResult GetCreateView()
        {
            ViewBag.AllDepartments = _context.Departments.ToList(); // For Selection Department When creating new Employee
            return View("Details");
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.AllDepartments = _context.Departments.ToList();
            return View();
        }

        [ValidateAntiForgeryToken] // To Prevent CSRF attacks
        [HttpPost] // It's necessary to define the method
        // [Bind()] => prevents over-posting attacks
        public IActionResult AddEmployee([Bind("Id,FullName,Position,Salary,Bonus,PhoneNumber,Email,ConfirmEmail,Password,ConfirmPassword," +
            "HiringDateTime,BirthDate,AttendanceTime,LeavingTime,CreatedAt,LastUpdateAt,DepartmentId")] Employee employee,
            IFormFile? imageFile)
        {
            if (ModelState.IsValid == true)
            {
                if(imageFile == null)
                {
                    employee.ImageUrl = "\\images\\No_Image.png";
                }
                else
                {
                    string imgUrl = "\\images\\" + Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                    employee.ImageUrl = imgUrl;

                    string imgPath = _webHostEnvironment.WebRootPath + imgUrl;

                    FileStream imgStream = new FileStream(imgPath, FileMode.Create);
                    imageFile.CopyTo(imgStream);
                    imgStream.Dispose();
                }

                employee.CreatedAt = DateTime.Now;

                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Create");
            }
           
        }

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Employee employee = _context.Employees.FirstOrDefault(Item => Item.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.AllDepartments = _context.Departments.ToList();
            return View("Edit", employee);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = _context.Employees.FirstOrDefault(Item => Item.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.AllDepartments = _context.Departments.ToList();
            return View(employee);
        }

        [ValidateAntiForgeryToken] // To Prevent CSRF attacks
        [HttpPost] // It's necessary to define the method
                   // [Bind()] => prevents over-posting attacks
        public IActionResult EditEmployee(int id, [Bind("Id,FullName,Position,Salary,Bonus,PhoneNumber,Email,ConfirmEmail,Password," +
            "ConfirmPassword,HiringDateTime,BirthDate,AttendanceTime,LeavingTime,CreatedAt,LastUpdateAt,DepartmentId," +
            "ImageUrl")] Employee employee, IFormFile? imageFile)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid == true)
            {
                if(imageFile != null)
                {
                    if(employee.ImageUrl != "\\images\\No_Image.png")
                    {
                        string oldImgPath = _webHostEnvironment.WebRootPath + employee.ImageUrl;
                        if(System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }
                    //string imgExtension = Path.GetExtension(imageFile.FileName);
                    //Guid imgGuid = Guid.NewGuid();
                    //string imgName = imgGuid + imgExtension;
                    string imgUrl = "\\images\\" + Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                    //string imgUrl = "\\images\\" + imgName;
                    employee.ImageUrl = imgUrl;

                    string imgPath = _webHostEnvironment.WebRootPath + imgUrl;
                    FileStream imgStream = new FileStream(imgPath, FileMode.Create);
                    imageFile.CopyTo(imgStream);
                    imgStream.Dispose();
                }

                employee.LastUpdateAt = DateTime.Now;

                _context.Employees.Update(employee);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Edit", employee);
            }
        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Employee employee = _context.Employees.FirstOrDefault(Item => Item.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View("Delete", employee);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Employee employee = _context.Employees.FirstOrDefault(Item => Item.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost] // It's necessary to define the method
        public IActionResult DeleteEmployee(int id)
        {
            Employee employee = _context.Employees.FirstOrDefault(Item => Item.Id == id);

            if (employee.ImageUrl != "\\images\\No_Image.png")
            {
                string imgPath = _webHostEnvironment.WebRootPath + employee.ImageUrl;
                System.IO.File.Delete(imgPath);
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction("GetIndexView");
        }
    }
}
