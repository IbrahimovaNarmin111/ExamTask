using System.ComponentModel.DataAnnotations.Schema;

namespace ExamTask.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string FullName {  get; set; }
        public string Description { get; set; }
        public string Work {  get; set; }
        public string? Image {  get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
