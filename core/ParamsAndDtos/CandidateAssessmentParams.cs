using System;
using core.Entities.HR;

namespace core.ParamsAndDtos
{
    public class CandidateAssessmentParams: ParamPages
    {
        public int CandidateId {get; set;}
        public int OrderDetailId {get; set;}
        public EnumCandidateAssessmentResult AssessmentResult {get; set;}
        public int AssessedById {get; set;}
        public DateTime DateAssessed {get; set;}
        public string LoggedInIdentityUserId {get; set;}
    }
}