import { ICandidateAssessedDto } from "../models/candidateAssessedDto";

export interface IPaginationAssessedCVs {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: ICandidateAssessedDto[];
}

 
 export class paginationAssessedCVs implements IPaginationAssessedCVs {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: ICandidateAssessedDto[]=[];
 }