using System.ComponentModel.DataAnnotations;

namespace ExamTask.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name {  get; set; }
        [MinLength(5)]
        [MaxLength(30)]
        public string Surname { get; set; }
        [Required]
        public string Username {  get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email {  get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword {  get; set; }

    }
}
