import { IApplicationTask } from "../models/applicationTask";
import { IApplicationTaskInBrief } from "../models/applicationTaskInBrief";


export interface IPaginationAppTask {
     pageIndex: number;
     pageSize: number;
     includeTaskItems: boolean;
     count: number;
     data: IApplicationTask[];
}

 
 export class PaginationAppTask implements IPaginationAppTask {
     pageIndex: number;
     pageSize: number;
     includeTaskItems: boolean;
     count: number;
     data: IApplicationTask[]=[];
 }