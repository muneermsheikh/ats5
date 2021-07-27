using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.HR;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface ICandidateAssessmentService
    {
        Task<CandidateAssessment> AssessNewCandidate(CandidateAssessmentParams candidateAssessParams);
        Task<bool> EditCandidateAssessment(CandidateAssessment candidateAssessment);
        Task<bool> DeleteCandidateAssessment(CandidateAssessment candidateAssessment);
        Task<bool> DeleteCandidateAssessmentItem(CandidateAssessmentItem assessmentItem);
        Task<CandidateAssessment> GetCandidateAssessment(int candidateId, int orderItemId);   
    }
}