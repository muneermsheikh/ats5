import { IUserHistory } from "../models/userHistory";


export interface IPaginationCandidateHistory {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IUserHistory[];
}

 
 export class PaginationCandidateHistory implements IPaginationCandidateHistory {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: IUserHistory[]=[];
 }