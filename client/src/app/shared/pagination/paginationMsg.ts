import { IMessage } from "../models/message";


export interface IPaginationMsg {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IMessage[];
}

 
 export class PaginationMsg implements IPaginationMsg {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IMessage[];
 }