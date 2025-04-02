using Clubmatesss.Models;
using Microsoft.AspNetCore.Mvc;

public class DepartmentController : Controller

{

    // GET: Department

    public ActionResult Index()

    {

        Department department = new Department()

        {

            DepartmentID = 123,

            DepartmentName = "ECE",

        };

        List<Department> departments = new List<Department>()

            {

                new Department() { DepartmentName = "ECE", DepartmentID = 123 },

                new Department() { DepartmentName = "CSE", DepartmentID = 126 }

            };

        return View(departments);

    }

    public ActionResult Department()

    {

        return View();

    }

    [HttpPost]

    public ActionResult AddDepartment(Department department)

    {

        return View();

    }

    public ActionResult AddDepartment()

    {

        return View(new Department());


    }

}

}
 