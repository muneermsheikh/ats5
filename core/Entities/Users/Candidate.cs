using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using core.Entities.Admin;
using core.Entities.MasterEntities;

namespace core.Entities.Users
{
    public class Candidate: BaseEntity
    {
          public Candidate()
          {
          }

          public Candidate(string appUserId, int applicationNo, 
                string firstName, string secondName, string familyName, string knownAs, DateTime dOB, 
                string city, string placeOfBirth, string aadharNo, string email, string introduction, 
                string interests, ICollection<UserQualification> userQualifications, 
                ICollection<UserProfession> userProfessions, ICollection<UserPassport> userPassports, 
                ICollection<UserAttachment> userAttachments)
          {
               AppUserId = appUserId;
               ApplicationNo = applicationNo;
               FirstName = firstName;  SecondName = secondName; FamilyName=familyName;
               KnownAs= knownAs; DOB = dOB; AadharNo=AadharNo; Email = Email;
               City = city; PlaceOfBirth = placeOfBirth; 
               City = city;
               Introduction = introduction;
               Interests = interests;
               UserQualifications = userQualifications;
               UserProfessions = userProfessions;
               UserPassports = userPassports;
               UserAttachments = userAttachments;
          }

        public string AppUserId {get; set;}
        public int ApplicationNo { get; set; }
        public string UserType {get; set;}
        [Required]
        public string Gender { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FamilyName { get; set; }
        [Required]         
        public string KnownAs { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        public string PlaceOfBirth { get; set; }
        public string AadharNo { get; set; }
        public string PpNo { get; set; } 
        public string Nationality {get; set;}
        [EmailAddress]
        public string Email {get; set;}
        public string UserName {get; set;}
        public string City { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string Interests { get; set; }
        public int? CompanyId {get; set;}       //associate id
        public EnumCandidateStatus? CandidateStatus {get; set;} = EnumCandidateStatus.NotReferred;
        //public ICollection<Address> Addresses { get; set; }
        public ICollection<UserPhone> UserPhones {get; set;}
        public ICollection<UserQualification> UserQualifications { get; set; }
        public ICollection<UserProfession> UserProfessions {get; set;}
        public ICollection<UserPassport> UserPassports {get; set;}
        public ICollection<UserAttachment> UserAttachments { get; set; }

        public string FullName {get => FirstName + " " + SecondName + " " + FamilyName;}
    }
}