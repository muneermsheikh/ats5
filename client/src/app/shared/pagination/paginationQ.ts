import { IAssessmentQBank } from "../models/assessmentQBank";


export interface IPaginationQ {
     pageIndex: number;
     pageSize: number;
     
     count: number;
     data: IAssessmentQBank[];
}

 
 export class PaginationQ implements IPaginationQ {
     pageIndex: number;
     pageSize: number;

     count: number;
     data: IAssessmentQBank[]=[];
 }