using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.HR;

namespace core.Interfaces
{
    public interface ICandidateAssessmentService
    {
        Task<CandidateAssessment> AssessNewCandidate(int candidateId, int orderItemId, int loggedInUserId, DateTime dateAssessed);
        Task<bool> EditCandidateAssessment(CandidateAssessment candidateAssessment);
        Task<bool> DeleteCandidateAssessment(CandidateAssessment candidateAssessment);
        
    }
}