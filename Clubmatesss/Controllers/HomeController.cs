using Clubmatesss.Models;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    // GET: Home
    public ActionResult Index()
    {
        Student student = new Student()
        {
            StudentID = 1,
            StudentName = "John",
            StudentDateOfBirth = new DateTime(1990, 1, 1),
            Height = 5.5M,
            Weight = 150
        };

        List<Student> students = new List<Student>()
            {
                new Student() {StudentID = 1, StudentName = "John", StudentDateOfBirth = new DateTime(1990,1,1),Height = 5.5M, Weight = 150},
                new Student() {StudentID = 2, StudentName = "Rose", StudentDateOfBirth = new DateTime(1991,1,1),Height = 5.7M, Weight = 100},
                new Student() {StudentID = 3, StudentName = "David", StudentDateOfBirth = new DateTime(1992,1,1),Height = 5.9M, Weight = 170}
            };
        return View(students);
    }
    public ActionResult Student()
    {
        return View();
    }

    [HttpPost]
    public ActionResult AddStudent(Student student)
    {
        return View();
    }
    public ActionResult AddStudent()
    {
        return View();
    }



}
}