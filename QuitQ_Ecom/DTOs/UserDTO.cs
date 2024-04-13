using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuitQ_Ecom.DTOs
{
    public class UserDTO
    {
        public int? UserId { get; set; }
        public int UserTypeId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime Dob { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        public int? GenderId { get; set; }
        public int? UserStatusId { get; set; }
    }
}
