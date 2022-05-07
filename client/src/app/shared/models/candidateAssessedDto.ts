export interface ICandidateAssessedDto
{
     id: number;
     checked: boolean;
     orderItemId: number;
     orderItemRef: string;
     applicationNo: number;
     customerName: string;
     requireInternalReview: boolean;
     candidateId: number;
     candidateName: string;
     assessedById: number;
     checklistedByName: string;
     charges: string;
     assessedByName: string;
     assessedResult: string;
     assessedOn: Date;
     
     remarks: string;
}