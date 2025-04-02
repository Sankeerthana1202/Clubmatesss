using System.ComponentModel.DataAnnotations;

namespace Clubmatesss.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public DateTime StudentDateOfBirth { get; set; }
        public decimal Height { get; set; }
        public float Weight { get; set; }


    }
}
