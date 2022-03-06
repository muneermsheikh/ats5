export interface ICVReviewDto {
     candidateId: number;
     orderItemId: number;
     execRemarks: string;
     charges: number;
     assignedToId: number;
}

export class cvReviewDto implements ICVReviewDto {
     candidateId: number;
     orderItemId: number;
     execRemarks: string;
     charges: number;
     assignedToId: number;
}