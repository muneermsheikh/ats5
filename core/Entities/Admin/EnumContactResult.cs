using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace core.Entities.Admin
{
    public enum EnumContactResult
    {
        //100 to 1000 - connectivity problems
        [Display(Name = "PHone Not Reachable."), EnumMember(Value="Phone Not Reachable")] PhoneNotReachable = 100,
        [Display(Name = "Phone out of network."), EnumMember(Value="Phone Out of network")] PhoneOutOfNetwork = 200,
        [Display(Name = "Phone unanswered."), EnumMember(Value="Phone unanswered")] PhoneUnanswered = 300,
        [Display(Name = "PHone answered by someone else."), EnumMember(Value="Phone answered by someone else")] PhoneAnsweredBySomeoneElse = 400,
        [Display(Name = "Wrong phone number."), EnumMember(Value="Phone Wrong number")] WrongNumber = 500,
        //1000+ to 2000 - connected but negative status
        [Display(Name = "Not interested in the job."), EnumMember(Value="Not Interested in the Job")] NotInterested = 1100,
        [Display(Name = "Wants salary increased."), EnumMember(Value="Wants salary increased")] WantsMoreSalary = 1200,
        [Display(Name = "Cannot join as PP not in his possession."), EnumMember(Value="Cannot join as PP not in his possession")] PPNotInPossession = 1300,
        [Display(Name = "Service Charges not acceptable."), EnumMember(Value="Service Charges not acceptable")] ServiceChargesNotAcceptable = 1400,
        [Display(Name = "Not interested due to unknown reasons."), EnumMember(Value="Not Interested due to unknown reasons")] NotInterestedUnKnownReasons = 1500,
        [Display(Name = "Medically Unfit."), EnumMember(Value="Medically Unfit")] MedicallyUnfit = 1600,
        //positive status - 2000+ - 3000
        [Display(Name = "Interested in Offer."), EnumMember(Value="Interested In offer")] Interested = 2100,
        [Display(Name = "Offer Accepted."), EnumMember(Value="Offer Accepted")] OfferAccepted = 2200,
        //3000+ to 4000 - Video Conference connectivity issues
        [Display(Name = "Unable to connect video conference application."), EnumMember(Value="Unable to connect Video Conference Application")] VideoConfToolUnableToConnect = 3100,
        [Display(Name = "Video Conference Tool Test Succeeded."), EnumMember(Value="Video Conference Tool Test Succeeded")] TestingOk = 3200,
        //4000+ to 5000 - video conference succeeded
        [Display(Name = "Video Conference Interiew concluded."), EnumMember(Value="Video Conference Interview Concluded")] VideoConferenceInterviewConcluded = 4100,
        
        [Display(Name = "Video Conference Interview completed."), EnumMember(Value="Video Conference Interview Completed")] VideoConferenceInterviewCompleted = 1000
    }
}