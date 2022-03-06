using System;
using core.Entities.Admin;
using core.Params;

namespace core.Specifications
{
     public class UserContactForCountSpecs : BaseSpecification<UserHistory>
     {
          public UserContactForCountSpecs(UserHistorySpecParams specParams)
            : base(x => 
                (string.IsNullOrEmpty(specParams.PartyName) || (x.PartyName.ToLower().Contains(specParams.PartyName.ToLower())) &&
                (string.IsNullOrEmpty(specParams.AadharNo) || x.AadharNo == specParams.AadharNo) &&
                (string.IsNullOrEmpty(specParams.EmailId) || x.EmailId == specParams.EmailId) &&
                (string.IsNullOrEmpty(specParams.PhoneNo) || x .AadharNo == specParams.PhoneNo) &&
                (!specParams.Id.HasValue || x.Id == (int)specParams.Id) &&
                (!specParams.CandidateId.HasValue || x.CandidateId == (int)specParams.CandidateId) &&
                (!specParams.CustomerOfficialId.HasValue || x.CustomerOfficialId == (int)specParams.CustomerOfficialId) &&
                (!specParams.ApplicationNo.HasValue || x.ApplicationNo == (int)specParams.ApplicationNo))
            )
          {
          }
     }
}