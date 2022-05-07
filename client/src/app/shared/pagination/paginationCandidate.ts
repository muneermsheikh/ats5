import { ICandidateBriefDto } from "../models/candidateBriefDto";


export interface IPaginationCandidate {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: ICandidateBriefDto[];
}

 
 export class PaginationCandidate implements IPaginationCandidate {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: ICandidateBriefDto[]=[];
 }