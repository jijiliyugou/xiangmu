using System.ComponentModel.DataAnnotations;

namespace Yaeher.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}