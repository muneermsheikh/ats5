import { ICandidateAssessment } from "../models/candidateAssessment";

export interface ICandidateAssessmentWithErrorStringDto
{
     candidateAssessment: ICandidateAssessment;
     errorString: string;
}