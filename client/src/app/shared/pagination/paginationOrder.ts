import { IOrderBrief } from "../models/orderBrief";


export interface IPaginationOrder {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IOrderBrief[];
}

 
 export class PaginationOrder implements IPaginationOrder {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IOrderBrief[]=[];
 }