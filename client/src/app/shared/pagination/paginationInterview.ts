import { IInterview } from "../models/hr/interview";
import { IInterviewBrief } from "../models/hr/interviewBrief";


export interface IPaginationInterview {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IInterviewBrief[];
}

 
 export class PaginationInterview implements IPaginationInterview {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IInterviewBrief[]=[];
 }