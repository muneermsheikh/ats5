using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using core.Entities.Identity;
using core.Entities.Users;

namespace api.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string UserType {get; set;}
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string UserName {get; set;}
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", 
            ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 6 characters")]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Gender {get; set;}="M";
        [Required]
        public string FirstName {get; set;}
        public string SecondName {get; set;}
        public string FamilyName {get; set;}
        public string Nationality {get; set;}="Indian";
        [Required]
        public DateTime DOB {get; set;}
        public int? CompanyId {get; set;}
        public string UserRole { get; set; }
        public Address Address {get; set;}
        public ICollection<UserQualification> UserQualifications {get; set;}
        public ICollection<UserExp> UserExperiences {get; set;}
        public ICollection<UserProfession> UserProfessions {get; set;}

    }
}