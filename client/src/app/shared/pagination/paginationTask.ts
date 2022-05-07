import { IApplicationTaskInBrief } from "../models/applicationTaskInBrief";


export interface IPaginationTask {
     pageIndex: number;
     pageSize: number;
     includeTaskItems: boolean;
     count: number;
     data: IApplicationTaskInBrief[];
}

 
 export class PaginationTask implements IPaginationTask {
     pageIndex: number;
     pageSize: number;
     includeTaskItems: boolean;
     count: number;
     data: IApplicationTaskInBrief[]=[];
 }