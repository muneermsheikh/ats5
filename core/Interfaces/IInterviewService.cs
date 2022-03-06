using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.HR;
using core.Params;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IInterviewService
    {
        Task<Interview> AddInterview (InterviewToAddDto interview);
        Task<Interview> EditInterview(Interview interview);
        Task<bool> DeleteInterview(int interviewId);
        Task<Interview> AddInterviewCategories(int orderId);
        Task<bool>DeleteInterviewItem(InterviewItem interviewItem);
        Task<ICollection<CandidateBriefDto>> GetCandidatesMatchingInterviewCategory(InterviewSpecParams interviewParams);
        Task<ICollection<Interview>> GetInterviews (string interviewStatus);
        Task<ICollection<Interview>> GetInterviewsWithItems (string interviewStatus);
        Task<InterviewItem> EditInterviewItem (InterviewItem interviewItem);
        Task<ICollection<InterviewItemCandidate>> AddCandidatesToInterviewItem (int interviewItemId, DateTime scheduledTime, 
            int durationInMinutes, string interviewMode,  List<int> CandidateIds);
        Task<bool> DeleteFromInterviewItemCandidates(List<int> InterviewItemCandidateIds);

        Task<InterviewDto> GetInterviewAttendanceOfAProject(int orderId, List<int> attendanceStatusIds);
        Task<ICollection<Interview>> GetOpenInterviews();
        Task<bool> RegisterCandidateReportedForInterview(int interviewItemCandidateId, DateTime ReportedAt);
        Task<bool> UpdateCandidateScheduledAttendanceStatus(int candidateInterviewId, int attendanceStatusId);
        Task<bool> RegisterCandidateAsInterviewed(int candidateInterviewedId, string interviewMode, DateTime interviewedAt);
        Task<bool> RegisterCandidateInterviewedWithResult(
            int candidateInterviewedId, string interviewMode, DateTime interviewedAt, int selectionStatusId);

    }
}