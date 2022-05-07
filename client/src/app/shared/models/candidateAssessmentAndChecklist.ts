import { ICandidateAssessedDto } from "./candidateAssessedDto";
import { ICandidateAssessment } from "./candidateAssessment";
import { IChecklistHR } from "./checklistHR";
import { IChecklistHRDto } from "./checklistHRDto";

export interface ICandidateAssessmentAndChecklist 
{
     assessed: ICandidateAssessment,
     checklistHRDto: IChecklistHRDto
}