import { ICandidateBriefDto } from "../models/candidateBriefDto";


export interface IPaginationCandidateBriefDto {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: ICandidateBriefDto[];
}

 
 export class PaginationCandidateBrief implements IPaginationCandidateBriefDto {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: ICandidateBriefDto[]=[];
 }