using System;
using core.Entities.Admin;
using core.Params;

namespace core.Specifications
{
     public class UserContactForCountSpecs : BaseSpecification<UserContact>
     {
          public UserContactForCountSpecs(UserContactSpecParams specParams)
            : base(x => 
                (!specParams.CandidateId.HasValue || x.CandidateId == specParams.CandidateId) 
                && (!specParams.OrderItemId.HasValue || x.OrderItemId == specParams.OrderItemId) 
                && (!specParams.OrderId.HasValue || x.OrderId == specParams.OrderId) 
                && (string.IsNullOrEmpty(specParams.Subject) || 
                    x.Subject.ToLower().Contains(specParams.Subject.ToLower())) 
                && (string.IsNullOrEmpty(specParams.UserPhoneNoContacted) || 
                    x.UserPhoneNoContacted.Contains(specParams.UserPhoneNoContacted)) 
                && (!specParams.DateOfContactFrom.HasValue && !specParams.DateOfContactUpto.HasValue || 
                    (DateTime.Compare(x.DateOfContact.Date, ((DateTime)specParams.DateOfContactFrom).Date) >= 0 && 
                    DateTime.Compare(x.DateOfContact.Date, ((DateTime)specParams.DateOfContactUpto).Date) <= 0)) 
                && (!specParams.DateOfContactFrom.HasValue || 
                    x.DateOfContact.Date == ((DateTime)specParams.DateOfContactFrom).Date) 
                && (!specParams.enumContactResult.HasValue || x.enumContactResult == specParams.enumContactResult) 
                && (!specParams.loggedInUserId.HasValue || x.LoggedInUserId == specParams.loggedInUserId) 
            
            )
          {
          }

          public UserContactForCountSpecs(int id)  : base(x => x.Id == id)
        {
        }
     }
}