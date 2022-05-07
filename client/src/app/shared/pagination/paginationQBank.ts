import { IAssessmentQBank } from "../models/assessmentQBank";


export interface IPaginationQBank {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: IAssessmentQBank[];
}

 
 export class paginationQBank implements IPaginationQBank {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: IAssessmentQBank[]=[];
 }