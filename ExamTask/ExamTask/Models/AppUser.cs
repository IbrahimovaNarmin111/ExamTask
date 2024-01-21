using Microsoft.AspNetCore.Identity;

namespace ExamTask.Models
{
    public class AppUser:IdentityUser
    {
        public int Id {  get; set; }
        public string Name {  get; set; }
        public string Surname {  get; set; }
        public bool IsRemained { get; set; }    
    }
}
