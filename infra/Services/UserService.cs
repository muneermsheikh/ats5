using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.Admin;
using core.Entities.Users;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class UserService : IUserService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ATSContext _context;
          public UserService(IUnitOfWork unitOfWork, ATSContext context)
          {
               _context = context;
               _unitOfWork = unitOfWork;
          }

          public async Task<Candidate> CreateCandidateAsync(CandidateToCreateDto dto)
          {
               int appNo=dto.ApplicationNo;
               if (appNo == 0) appNo = await _context.Candidates.MaxAsync(x => x.ApplicationNo)+1;
               
            //passports
               var PPs = new List<UserPassport>();
               foreach(var pp in dto.UserPassports) {
                    PPs.Add(new UserPassport(pp.PassportNo, pp.Nationality, pp.IssuedOn, pp.Validity,pp.IsValid));
               }
            //qualifications
                var Qs = new List<UserQualification>();
                foreach(var q in dto.UserQualifications) {
                   Qs.Add(new UserQualification(q.QualificationId, q.IsMain));
                }

            //addresses
            /*
                var adds = new List<Address>();
                foreach(var add in dto.Addresses) {
                    adds.Add(new Address(add.AddressType, add.Add, add.StreetAdd, 
                        add.City, add.Pin, add.State, add.District, add.Country, add.IsMain));
                }
            */
            //professions
                var profs = new List<UserProfession>();
                foreach(var p in dto.UserProfessions) {
                    profs.Add(new UserProfession(p.CandidateId, p.IndustryId, p.IsMain));
                }
            
            //attachments
                var atts = new List<UserAttachment>();
                foreach(var a in dto.UserAttachments) {
                    atts.Add(new UserAttachment(a.AppUserId, a.AttachmentType, a.AttachmentSizeInKB, a.AttachmentUrl));
                }
            
            //person
                var per = new Person(dto.Person.FirstName, dto.Person.SecondName, dto.Person.FamilyName, dto.Person.KnownAs,
                    dto.Person.DOB, dto.Person.PlaceOfBirth, dto.Person.AadharNo, dto.Person.PpNo, dto.Person.Nationality);
            //candidate
                var cand = new Candidate(dto.AppUserId, appNo, per.FirstName, per.SecondName, 
                    per.FamilyName, per.KnownAs, per.DOB, dto.City, per.PlaceOfBirth, per.AadharNo, per.Email,
                    dto.Introduction, dto.Interests, 
                    //adds, 
                    Qs, profs, PPs, atts);
               
                _unitOfWork.Repository<Candidate>().Add(cand);

                var result = await _unitOfWork.Complete();

                if (result <=0 ) return null;

                return cand;
          }

          public async Task<Candidate> GetCandidateByIdAsync(int id)
          {
               return await _unitOfWork.Repository<Candidate>().GetByIdAsync(id);
          }

          public async Task<Candidate> GetCandidateBySpecsIdentityIdAsync(string appUserId)
          {
               var spec = new CandidateSpecs(appUserId);
               return await _unitOfWork.Repository<Candidate>().GetEntityWithSpec(spec);    
          }

          public async Task<Candidate> GetCandidateBySpecsUserIdAsync(int userId)
          {
               var spec = new CandidateSpecs(userId);
               return await _unitOfWork.Repository<Candidate>().GetEntityWithSpec(spec);    
          }

       
     }
}